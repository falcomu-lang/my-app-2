using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CameraCaptureApp.Models;
using CameraCaptureApp.Services;

namespace CameraCaptureApp.Forms
{
    public partial class MainForm : Form
    {
        private const int MaxConcurrentSnapshotSaves = 5;

        private readonly ICameraService _cameraService;
        private readonly ISettingsService _settingsService;
        private readonly Controls.CameraDisplayControl _cameraDisplayControl;
        private readonly FrameRecorder _frameRecorder;
        private readonly System.Windows.Forms.Timer _statusRefreshTimer;
        private CameraSettings _settings;
        private CancellationTokenSource _imageLoadTokenSource;
        private CancellationTokenSource _previewFrameTokenSource;
        private readonly object _pendingPreviewFramesLock = new object();
        private readonly Queue<Bitmap> _pendingRollingPreviewFrames = new Queue<Bitmap>();
        private readonly SemaphoreSlim _snapshotSaveGate = new SemaphoreSlim(MaxConcurrentSnapshotSaves, MaxConcurrentSnapshotSaves);
        private readonly object _snapshotProgressLock = new object();
        private bool _autoConnectAttempted;
        private int _pendingSnapshotSaveCount;
        private int _snapshotSaveQueueCount;
        private int _snapshotSaveActiveCount;
        private int _snapshotSaveCompletedCount;
        private int _snapshotSaveFailedCount;
        private SaveProgressForm _snapshotProgressForm;
        private Bitmap _pendingPreviewFrame;
        private int _previewFrameUiUpdateQueued;

        public MainForm(ICameraService cameraService, ISettingsService settingsService)
        {
            _cameraService = cameraService;
            _settingsService = settingsService;
            _settings = _settingsService.Load() ?? CameraSettings.CreateDefault();

            InitializeComponent();

            _frameRecorder = new FrameRecorder();
            _cameraDisplayControl = new Controls.CameraDisplayControl();
            _cameraDisplayControl.Dock = DockStyle.Fill;
            _cameraDisplayControl.SaveSnapshotRequested += CameraDisplayControl_SaveSnapshotRequested;
            panelViewerHost.Controls.Add(_cameraDisplayControl);

            _cameraService.FrameReady += CameraService_FrameReady;

            ApplySettingsToUi();
            UpdateStatus();

            _statusRefreshTimer = new System.Windows.Forms.Timer();
            _statusRefreshTimer.Interval = 300;
            _statusRefreshTimer.Tick += StatusRefreshTimer_Tick;
            _statusRefreshTimer.Start();

            this.Shown += MainForm_Shown;
        }

        private void buttonCameraSettings_Click(object sender, EventArgs e)
        {
            var dialogSettings = _settings ?? CameraSettings.CreateDefault();
            CameraSettingsForm form = null;
            try
            {
                form = new CameraSettingsForm(dialogSettings, _cameraService);
                DialogResult dialogResult;
                dialogResult = form.ShowDialog(this);

                if (dialogResult != DialogResult.OK && !form.SettingsApplied)
                {
                    return;
                }

                _settings = form.Settings.Clone();
                _settingsService.Save(_settings);
                _cameraService.ApplySettings(_settings);
                ApplySettingsToUi();
                UpdateStatus();
            }
            catch (Exception ex)
            {
                AppLogger.Log("Camera Settings dialog open failed.", ex);
                MessageBox.Show(
                    this,
                    "Camera Settings could not be opened.\r\n" + ex.Message + "\r\n\r\nLog: " + AppLogger.GetLogPath(),
                    "Camera Settings Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                labelFooterMessageValue.Text = "Camera Settings open failed: " + ex.Message;
            }
            finally
            {
                if (form != null)
                {
                    form.Dispose();
                }
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            _cameraService.ApplySettings(_settings);
            if (_cameraService.Connect())
            {
                _settings = _cameraService.CurrentSettings;
                _settingsService.Save(_settings);
                ApplySettingsToUi();
            }
            UpdateStatus();
        }

        private void buttonStartPreview_Click(object sender, EventArgs e)
        {
            _frameRecorder.ClearRolling();
            _cameraService.StartPreview();
            UpdateStatus();
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            _cameraService.Disconnect();
            UpdateStatus();
        }

        private async void buttonStop_Click(object sender, EventArgs e)
        {
            _cameraService.StopPreview();
            UpdateStatus();
            if (_settings.RollingCaptureEnabled)
            {
                await ShowRollingSnapshotForReviewAsync();
            }
        }

        private void buttonCapture_Click(object sender, EventArgs e)
        {
            if (_cameraService.CaptureFrame())
            {
                Interlocked.Increment(ref _pendingSnapshotSaveCount);
                labelFooterMessageValue.Text = "Capture requested. Waiting for the next frame to save...";
            }

            UpdateStatus();
        }

        private async void SaveLatestRecordedFrameIfRequestedAsync()
        {
            if (Interlocked.CompareExchange(ref _pendingSnapshotSaveCount, 0, 0) <= 0)
            {
                return;
            }

            if (Interlocked.Decrement(ref _pendingSnapshotSaveCount) < 0)
            {
                Interlocked.Exchange(ref _pendingSnapshotSaveCount, 0);
                return;
            }

            if (_settings.RollingCaptureEnabled)
            {
                try
                {
                    var progressForm = EnsureSaveProgressForm();
                    var settings = _settings.Clone();
                    var savedPath = await RunQueuedSnapshotSaveAsync(
                        progressForm,
                        () => SaveRollingCaptureSnapshot(settings));
                    SetFooterMessage("Captured image saved: " + Path.GetFileName(savedPath));
                }
                catch (Exception ex)
                {
                    SetFooterMessage("Captured image save failed: " + ex.Message);
                }
                finally
                {
                    CompleteQueuedSnapshotSave();
                }

                return;
            }

            using (var snapshot = _frameRecorder.SnapshotLatest())
            {
                if (snapshot == null)
                {
                    return;
                }

                try
                {
                    var progressForm = EnsureSaveProgressForm();
                    var settings = _settings.Clone();
                    var savedPath = await RunQueuedSnapshotSaveAsync(
                        progressForm,
                        () => SaveSnapshotBitmap(snapshot, settings));
                    SetFooterMessage("Captured image saved: " + Path.GetFileName(savedPath));
                }
                catch (Exception ex)
                {
                    SetFooterMessage("Captured image save failed: " + ex.Message);
                }
                finally
                {
                    CompleteQueuedSnapshotSave();
                }
            }
        }

        private async void CameraDisplayControl_SaveSnapshotRequested(object sender, EventArgs e)
        {
            var status = _cameraService.Status;
            if (status == null || (status.IsPreviewing && !_settings.RollingCaptureEnabled))
            {
                labelFooterMessageValue.Text = "Stop preview before saving a snapshot.";
                return;
            }

            if (_settings.RollingCaptureEnabled)
            {
                var rollingSnapshot = _frameRecorder.SnapshotRollingFrames();
                if (rollingSnapshot == null)
                {
                    labelFooterMessageValue.Text = "No rolling image is available to save.";
                    return;
                }

                try
                {
                    var progressForm = EnsureSaveProgressForm();
                    var direction = _settings.RollingCaptureDirection;
                    var saveFormat = _settings.ImageSaveFormat;
                    var savedPath = await RunQueuedSnapshotSaveAsync(
                        progressForm,
                        () => SaveManualRollingSnapshot(rollingSnapshot, saveFormat, direction, progressForm.Report));
                    labelFooterMessageValue.Text = "Snapshot saved: " + Path.GetFileName(savedPath);
                }
                catch (Exception ex)
                {
                    labelFooterMessageValue.Text = "Snapshot save failed: " + ex.Message;
                }
                finally
                {
                    CompleteQueuedSnapshotSave();
                    rollingSnapshot.Dispose();
                }

                return;
            }

            using (var snapshot = _frameRecorder.SnapshotLatest())
            {
                if (snapshot == null)
                {
                    labelFooterMessageValue.Text = "No recorded image is available to save.";
                    return;
                }

                try
                {
                    var progressForm = EnsureSaveProgressForm();
                    var saveFormat = _settings.ImageSaveFormat;
                    progressForm.Report(15, "Saving image...");
                    var savedPath = await RunQueuedSnapshotSaveAsync(
                        progressForm,
                        () => SaveManualSnapshotBitmap(snapshot, saveFormat));
                    progressForm.Report(100, "Image saved.");
                    labelFooterMessageValue.Text = "Snapshot saved: " + Path.GetFileName(savedPath);
                }
                catch (Exception ex)
                {
                    labelFooterMessageValue.Text = "Snapshot save failed: " + ex.Message;
                }
                finally
                {
                    CompleteQueuedSnapshotSave();
                }
            }
        }

        private async Task<string> RunQueuedSnapshotSaveAsync(SaveProgressForm progressForm, Func<string> saveAction)
        {
            ReportSnapshotSaveCounts(progressForm);
            progressForm.Report(0, "Waiting for a save slot...");
            await _snapshotSaveGate.WaitAsync();
            Interlocked.Increment(ref _snapshotSaveActiveCount);
            ReportSnapshotSaveCounts(progressForm);
            try
            {
                progressForm.Report(5, "Saving image...");
                var savedPath = await Task.Run(saveAction);
                Interlocked.Increment(ref _snapshotSaveCompletedCount);
                ReportSnapshotSaveCounts(progressForm);
                return savedPath;
            }
            catch
            {
                Interlocked.Increment(ref _snapshotSaveFailedCount);
                ReportSnapshotSaveCounts(progressForm);
                throw;
            }
            finally
            {
                Interlocked.Decrement(ref _snapshotSaveActiveCount);
                _snapshotSaveGate.Release();
                ReportSnapshotSaveCounts(progressForm);
            }
        }

        private SaveProgressForm EnsureSaveProgressForm()
        {
            var queuedCount = Interlocked.Increment(ref _snapshotSaveQueueCount);
            lock (_snapshotProgressLock)
            {
                if (_snapshotProgressForm == null || _snapshotProgressForm.IsDisposed)
                {
                    Interlocked.Exchange(ref _snapshotSaveCompletedCount, 0);
                    Interlocked.Exchange(ref _snapshotSaveFailedCount, 0);
                    _snapshotProgressForm = new SaveProgressForm();
                    _snapshotProgressForm.FormClosed += SnapshotProgressForm_FormClosed;
                    _snapshotProgressForm.Show(this);
                }

                _snapshotProgressForm.Report(0, "Queued save jobs: " + queuedCount + ".");
                ReportSnapshotSaveCounts(_snapshotProgressForm);
                return _snapshotProgressForm;
            }
        }

        private void CompleteQueuedSnapshotSave()
        {
            var remaining = Interlocked.Decrement(ref _snapshotSaveQueueCount);
            if (remaining > 0)
            {
                return;
            }

            lock (_snapshotProgressLock)
            {
                if (_snapshotProgressForm == null || _snapshotProgressForm.IsDisposed)
                {
                    return;
                }

                _snapshotProgressForm.Report(100, "All save jobs completed.");
                ReportSnapshotSaveCounts(_snapshotProgressForm);
                _snapshotProgressForm.Close();
                _snapshotProgressForm = null;
            }
        }

        private void ReportSnapshotSaveCounts(SaveProgressForm progressForm)
        {
            if (progressForm == null || progressForm.IsDisposed)
            {
                return;
            }

            var outstandingCount = Interlocked.CompareExchange(ref _snapshotSaveQueueCount, 0, 0);
            var activeCount = Interlocked.CompareExchange(ref _snapshotSaveActiveCount, 0, 0);
            var completedCount = Interlocked.CompareExchange(ref _snapshotSaveCompletedCount, 0, 0);
            var failedCount = Interlocked.CompareExchange(ref _snapshotSaveFailedCount, 0, 0);
            var waitingCount = Math.Max(0, outstandingCount - activeCount);
            progressForm.ReportCounts(activeCount, waitingCount, completedCount, failedCount);
        }

        private void SnapshotProgressForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            lock (_snapshotProgressLock)
            {
                if (ReferenceEquals(_snapshotProgressForm, sender))
                {
                    _snapshotProgressForm = null;
                }
            }
        }

        private async Task ShowRollingSnapshotForReviewAsync()
        {
            labelFooterMessageValue.Text = "Preparing rolling image for review...";
            try
            {
                var filePath = Path.Combine(
                    Path.GetTempPath(),
                    "CameraCaptureApp_rolling_review_" + DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + ".png");

                await Task.Run(
                    () =>
                    {
                        _frameRecorder.SaveRollingPng(filePath, _settings.RollingCaptureDirection, null, null);
                    });

                if (!File.Exists(filePath))
                {
                    labelFooterMessageValue.Text = "No rolling image is available for review.";
                    return;
                }

                await LoadImageForReviewAsync(filePath);
                labelFooterMessageValue.Text = "Rolling image ready for review.";
            }
            catch (OperationCanceledException)
            {
                labelFooterMessageValue.Text = "Rolling image review cancelled.";
            }
            catch (Exception ex)
            {
                labelFooterMessageValue.Text = "Rolling image review failed: " + ex.Message;
            }
        }

        private async Task LoadImageForReviewAsync(string filePath)
        {
            CancelPendingImageLoad();
            _imageLoadTokenSource = new CancellationTokenSource();
            await _cameraDisplayControl.LoadImageFromFileAsync(filePath, _imageLoadTokenSource.Token);
        }

        private async void buttonLoadImage_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Image Files|*.bmp;*.png;*.jpg;*.jpeg;*.tif;*.tiff|All Files|*.*";
                dialog.Title = "Load preview image";

                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                CancelPendingImageLoad();
                _imageLoadTokenSource = new CancellationTokenSource();
                var token = _imageLoadTokenSource.Token;
                labelFooterMessageValue.Text = "Loading image...";

                try
                {
                    await _cameraDisplayControl.LoadImageFromFileAsync(dialog.FileName, token);
                    labelFooterMessageValue.Text = "Loaded image: " + Path.GetFileName(dialog.FileName);
                }
                catch (OperationCanceledException)
                {
                    labelFooterMessageValue.Text = "Image load cancelled.";
                }
                catch (Exception ex)
                {
                    labelFooterMessageValue.Text = "Image load failed: " + ex.Message;
                }
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _cameraService.FrameReady -= CameraService_FrameReady;
            _cameraDisplayControl.SaveSnapshotRequested -= CameraDisplayControl_SaveSnapshotRequested;
            _statusRefreshTimer.Stop();
            _statusRefreshTimer.Dispose();
            CancelPendingImageLoad();
            CancelPendingPreviewFrame();
            _frameRecorder.Dispose();
            ClearPendingRollingPreviewFrames();
            var pendingFrame = Interlocked.Exchange(ref _pendingPreviewFrame, null);
            if (pendingFrame != null)
            {
                pendingFrame.Dispose();
            }

            _cameraService.Disconnect();
            base.OnFormClosed(e);
        }

        private void CameraService_FrameReady(object sender, CameraFrameEventArgs e)
        {
            if (IsDisposed || !IsHandleCreated)
            {
                e.Frame.Dispose();
                return;
            }

            _frameRecorder.StoreLatest(e.Frame);
            if (_settings.RollingCaptureEnabled)
            {
                _frameRecorder.StoreRolling(e.Frame, _settings.RollingCaptureFrameCount);
            }
            else
            {
                _frameRecorder.ClearRolling();
            }
            SaveLatestRecordedFrameIfRequestedAsync();
            if (_settings.RollingCaptureEnabled)
            {
                lock (_pendingPreviewFramesLock)
                {
                    _pendingRollingPreviewFrames.Enqueue(e.Frame);
                }
            }
            else
            {
                ClearPendingRollingPreviewFrames();
                var previousFrame = Interlocked.Exchange(ref _pendingPreviewFrame, e.Frame);
                if (previousFrame != null)
                {
                    previousFrame.Dispose();
                }
            }

            if (Interlocked.Exchange(ref _previewFrameUiUpdateQueued, 1) == 0)
            {
                BeginInvoke(new Action(ProcessPendingPreviewFrameAsync));
            }
        }

        private async void ProcessPendingPreviewFrameAsync()
        {
            Bitmap frame = null;
            try
            {
                frame = Interlocked.Exchange(ref _pendingPreviewFrame, null);
                if (frame == null)
                {
                    frame = DequeuePendingRollingPreviewFrame();
                }

                if (frame == null || IsDisposed)
                {
                    return;
                }

                await DisplayPreviewFrameAsync(frame);
                frame = null;
            }
            finally
            {
                if (frame != null)
                {
                    frame.Dispose();
                }

                Interlocked.Exchange(ref _previewFrameUiUpdateQueued, 0);
                if ((Interlocked.CompareExchange(ref _pendingPreviewFrame, null, null) != null || HasPendingRollingPreviewFrames()) &&
                    Interlocked.Exchange(ref _previewFrameUiUpdateQueued, 1) == 0 &&
                    !IsDisposed &&
                    IsHandleCreated)
                {
                    BeginInvoke(new Action(ProcessPendingPreviewFrameAsync));
                }
            }
        }

        private Bitmap DequeuePendingRollingPreviewFrame()
        {
            lock (_pendingPreviewFramesLock)
            {
                return _pendingRollingPreviewFrames.Count > 0 ? _pendingRollingPreviewFrames.Dequeue() : null;
            }
        }

        private bool HasPendingRollingPreviewFrames()
        {
            lock (_pendingPreviewFramesLock)
            {
                return _pendingRollingPreviewFrames.Count > 0;
            }
        }

        private void ClearPendingRollingPreviewFrames()
        {
            lock (_pendingPreviewFramesLock)
            {
                while (_pendingRollingPreviewFrames.Count > 0)
                {
                    _pendingRollingPreviewFrames.Dequeue().Dispose();
                }
            }
        }

        private async Task DisplayPreviewFrameAsync(Bitmap frame)
        {
            CancelPendingPreviewFrame();
            _previewFrameTokenSource = new CancellationTokenSource();
            var token = _previewFrameTokenSource.Token;
            var displayOwnsFrame = false;

            try
            {
                await _cameraDisplayControl.ShowFrameAsync(
                    frame,
                    token,
                    _settings.RollingCaptureEnabled,
                    _settings.RollingCaptureFrameCount,
                    _settings.RollingCaptureDirection);
                displayOwnsFrame = true;
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                if (!displayOwnsFrame)
                {
                    frame.Dispose();
                }
            }
        }

        private void StatusRefreshTimer_Tick(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            TryAutoConnectOnStart();
        }

        private void ApplySettingsToUi()
        {
            labelHeaderResolutionValue.Text = _settings.Width + " x " + _settings.Height;
            labelHeaderTriggerValue.Text = GetTriggerModeDisplay(_settings.TriggerMode);
            labelHeaderCameraValue.Text = _settings.CameraName;
            _cameraDisplayControl.ResolutionText = _settings.Width + " x " + _settings.Height;
            _cameraDisplayControl.OverlayText = "Ready for preview";
        }

        private void UpdateStatus()
        {
            var status = _cameraService.Status;

            labelHeaderConnectionValue.Text = status.IsConnected ? "Connected" : "Offline";
            labelHeaderSignalValue.Text = status.HasSignal ? "Detected" : "Missing";
            labelFooterLinesValue.Text = status.ScannedLineCount.ToString();
            labelFooterPreviewValue.Text = status.IsPreviewing ? "Running" : "Stopped";
            labelFooterMessageValue.Text = status.LastMessage;
            labelFooterScanStateValue.Text = status.ScanStateText;
            if (!string.IsNullOrWhiteSpace(status.CameraName))
            {
                labelHeaderCameraValue.Text = status.CameraName;
            }

            UpdateCommandStates(status);
        }

        private void SetFooterMessage(string message)
        {
            if (IsDisposed)
            {
                return;
            }

            if (InvokeRequired)
            {
                try
                {
                    BeginInvoke(new Action<string>(SetFooterMessage), message);
                }
                catch (ObjectDisposedException)
                {
                }
                catch (InvalidOperationException)
                {
                }

                return;
            }

            labelFooterMessageValue.Text = message;
        }

        private void UpdateCommandStates(CameraStatus status)
        {
            var isConnected = status != null && status.IsConnected;
            var isPreviewing = status != null && status.IsPreviewing;

            buttonCameraSettings.Enabled = true;
            buttonConnect.Enabled = !isConnected;
            buttonDisconnect.Enabled = isConnected;
            buttonStartPreview.Enabled = isConnected && !isPreviewing;
            buttonStop.Enabled = isConnected && isPreviewing;
            buttonCapture.Enabled = isConnected && !isPreviewing;
            buttonLoadImage.Enabled = !isPreviewing;
            _cameraDisplayControl.SaveSnapshotButtonEnabled = isConnected && (!isPreviewing || _settings.RollingCaptureEnabled);
        }

        private void CancelPendingImageLoad()
        {
            if (_imageLoadTokenSource == null)
            {
                return;
            }

            _imageLoadTokenSource.Cancel();
            _imageLoadTokenSource.Dispose();
            _imageLoadTokenSource = null;
        }

        private void CancelPendingPreviewFrame()
        {
            if (_previewFrameTokenSource == null)
            {
                return;
            }

            _previewFrameTokenSource.Cancel();
            _previewFrameTokenSource.Dispose();
            _previewFrameTokenSource = null;
        }

        private static string GetTriggerModeDisplay(TriggerMode mode)
        {
            switch (mode)
            {
                case TriggerMode.SingleFrame:
                    return "Single";
                case TriggerMode.SoftwareTrigger:
                    return "Software";
                case TriggerMode.ExternalTrigger:
                    return "External";
                default:
                    return "Free Run";
            }
        }

        private void TryAutoConnectOnStart()
        {
            if (_autoConnectAttempted || !_settings.AutoConnect)
            {
                return;
            }

            _autoConnectAttempted = true;
            _cameraService.ApplySettings(_settings);
            if (_cameraService.Connect())
            {
                _settings = _cameraService.CurrentSettings;
                _settingsService.Save(_settings);
                ApplySettingsToUi();
            }

            UpdateStatus();
        }

        private static string SaveSnapshotBitmap(Bitmap bitmap, CameraSettings settings)
        {
            var outputFolder = ResolveSnapshotFolder(settings);
            Directory.CreateDirectory(outputFolder);

            var filePath = BuildSnapshotPath(outputFolder, settings);
            FrameRecorder.SaveBitmapImage(bitmap, filePath, settings.ImageSaveFormat, null);
            return filePath;
        }

        private string SaveRollingCaptureSnapshot(CameraSettings settings)
        {
            var outputFolder = ResolveSnapshotFolder(settings);
            Directory.CreateDirectory(outputFolder);

            var filePath = BuildSnapshotPath(outputFolder, settings);
            if (!_frameRecorder.SaveRollingImage(filePath, settings.ImageSaveFormat, settings.RollingCaptureDirection, null, null))
            {
                throw new InvalidOperationException("No rolling image is available to save.");
            }

            return filePath;
        }

        private string SaveManualRollingSnapshot(
            FrameRecorder.RollingFrameSnapshot rollingSnapshot,
            ImageSaveFormat saveFormat,
            RollingCaptureDirection direction,
            Action<int, string> reportProgress)
        {
            var outputFolder = Path.Combine(Application.StartupPath, "snapshot");
            Directory.CreateDirectory(outputFolder);

            var baseName = "rolling_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
            var filePath = BuildManualSnapshotPath(outputFolder, baseName, saveFormat);
            rollingSnapshot.SaveImage(filePath, saveFormat, direction, reportProgress, null);

            if (!File.Exists(filePath))
            {
                throw new IOException("Snapshot file was not created: " + filePath);
            }

            return filePath;
        }

        private static string SaveManualSnapshotBitmap(Bitmap bitmap, ImageSaveFormat saveFormat)
        {
            var outputFolder = Path.Combine(Application.StartupPath, "snapshot");
            Directory.CreateDirectory(outputFolder);

            var baseName = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
            var filePath = BuildManualSnapshotPath(outputFolder, baseName, saveFormat);
            FrameRecorder.SaveBitmapImage(bitmap, filePath, saveFormat, null);
            if (!File.Exists(filePath))
            {
                throw new IOException("Snapshot file was not created: " + filePath);
            }

            return filePath;
        }

        private static string BuildManualSnapshotPath(string outputFolder, string baseName, ImageSaveFormat saveFormat)
        {
            var extension = GetImageExtension(saveFormat);
            var candidatePath = Path.Combine(outputFolder, baseName + extension);
            if (!File.Exists(candidatePath))
            {
                return candidatePath;
            }

            var suffix = 1;
            while (true)
            {
                var nextPath = Path.Combine(outputFolder, baseName + "_" + suffix.ToString("000") + extension);
                if (!File.Exists(nextPath))
                {
                    return nextPath;
                }

                suffix++;
            }
        }

        private static string ResolveSnapshotFolder(CameraSettings settings)
        {
            if (!string.IsNullOrWhiteSpace(settings.SaveFolder))
            {
                return settings.SaveFolder.Trim();
            }

            return Path.Combine(Application.StartupPath, "Captures");
        }

        private static string BuildSnapshotPath(string outputFolder, CameraSettings settings)
        {
            var baseName = FormatFileNamePattern(settings.FileNamePattern);
            if (string.IsNullOrWhiteSpace(baseName))
            {
                baseName = "capture_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
            }

            baseName = SanitizeFileName(baseName);
            var extension = GetImageExtension(settings.ImageSaveFormat);
            var candidatePath = Path.Combine(outputFolder, baseName + extension);
            if (!File.Exists(candidatePath))
            {
                return candidatePath;
            }

            var suffix = 1;
            while (true)
            {
                var nextPath = Path.Combine(outputFolder, baseName + "_" + suffix.ToString("000") + extension);
                if (!File.Exists(nextPath))
                {
                    return nextPath;
                }

                suffix++;
            }
        }

        private static string FormatFileNamePattern(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern))
            {
                return string.Empty;
            }

            var result = pattern;
            var startIndex = result.IndexOf('{');
            while (startIndex >= 0)
            {
                var endIndex = result.IndexOf('}', startIndex + 1);
                if (endIndex <= startIndex)
                {
                    break;
                }

                var format = result.Substring(startIndex + 1, endIndex - startIndex - 1);
                string replacement;
                try
                {
                    replacement = DateTime.Now.ToString(format);
                }
                catch (FormatException)
                {
                    replacement = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                }

                result = result.Substring(0, startIndex) + replacement + result.Substring(endIndex + 1);
                startIndex = result.IndexOf('{', startIndex + replacement.Length);
            }

            return result;
        }

        private static string GetImageExtension(ImageSaveFormat saveFormat)
        {
            switch (saveFormat)
            {
                case ImageSaveFormat.Tif:
                case ImageSaveFormat.UncompressedTif:
                    return ".tif";
                default:
                    return ".png";
            }
        }

        private static string SanitizeFileName(string fileName)
        {
            var sanitized = fileName;
            foreach (var invalidChar in Path.GetInvalidFileNameChars())
            {
                sanitized = sanitized.Replace(invalidChar, '_');
            }

            return sanitized.Trim();
        }
    }
}

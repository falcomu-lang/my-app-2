using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
        private bool _autoConnectAttempted;
        private int _pendingSnapshotSaveCount;
        private int _manualSnapshotSaveInProgress;
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
                    var savedPath = await Task.Run(() => SaveRollingCaptureSnapshot(_settings));
                    SetFooterMessage("Captured image saved: " + Path.GetFileName(savedPath));
                }
                catch (Exception ex)
                {
                    SetFooterMessage("Captured image save failed: " + ex.Message);
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
                    var savedPath = await Task.Run(() => SaveSnapshotBitmap(snapshot, _settings));
                    SetFooterMessage("Captured image saved: " + Path.GetFileName(savedPath));
                }
                catch (Exception ex)
                {
                    SetFooterMessage("Captured image save failed: " + ex.Message);
                }
            }
        }

        private async void CameraDisplayControl_SaveSnapshotRequested(object sender, EventArgs e)
        {
            if (Interlocked.Exchange(ref _manualSnapshotSaveInProgress, 1) == 1)
            {
                labelFooterMessageValue.Text = "Snapshot save is already running.";
                return;
            }

            var status = _cameraService.Status;
            if (status == null || (status.IsPreviewing && !_settings.RollingCaptureEnabled))
            {
                labelFooterMessageValue.Text = "Stop preview before saving a snapshot.";
                Interlocked.Exchange(ref _manualSnapshotSaveInProgress, 0);
                return;
            }

            SaveProgressForm progressForm = null;
            if (_settings.RollingCaptureEnabled)
            {
                try
                {
                    progressForm = ShowSaveProgressForm();
                    var savedPath = await Task.Run(() => SaveManualRollingSnapshot(progressForm.Report));
                    if (!status.IsPreviewing)
                    {
                        await LoadImageForReviewAsync(savedPath);
                    }

                    labelFooterMessageValue.Text = "Snapshot saved: " + Path.GetFileName(savedPath);
                }
                catch (Exception ex)
                {
                    labelFooterMessageValue.Text = "Snapshot save failed: " + ex.Message;
                }
                finally
                {
                    CloseSaveProgressForm(progressForm);
                    Interlocked.Exchange(ref _manualSnapshotSaveInProgress, 0);
                }

                return;
            }

            using (var snapshot = _frameRecorder.SnapshotLatest())
            {
                if (snapshot == null)
                {
                    labelFooterMessageValue.Text = "No recorded image is available to save.";
                    Interlocked.Exchange(ref _manualSnapshotSaveInProgress, 0);
                    return;
                }

                try
                {
                    progressForm = ShowSaveProgressForm();
                    progressForm.Report(15, "Saving image...");
                    var savedPath = await Task.Run(() => SaveManualSnapshotBitmap(snapshot, _settings.RollingCaptureEnabled));
                    progressForm.Report(100, "Image saved.");
                    labelFooterMessageValue.Text = "Snapshot saved: " + Path.GetFileName(savedPath);
                }
                catch (Exception ex)
                {
                    labelFooterMessageValue.Text = "Snapshot save failed: " + ex.Message;
                }
                finally
                {
                    CloseSaveProgressForm(progressForm);
                    Interlocked.Exchange(ref _manualSnapshotSaveInProgress, 0);
                }
            }
        }

        private SaveProgressForm ShowSaveProgressForm()
        {
            var form = new SaveProgressForm();
            form.Show(this);
            form.Report(0, "Preparing image...");
            return form;
        }

        private static void CloseSaveProgressForm(SaveProgressForm form)
        {
            if (form == null || form.IsDisposed)
            {
                return;
            }

            form.Report(100, "Done.");
            form.Close();
            form.Dispose();
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
                        _frameRecorder.SaveRollingPng(filePath);
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
                    _settings.RollingCaptureFrameCount);
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
            bitmap.Save(filePath, ImageFormat.Png);
            return filePath;
        }

        private string SaveRollingCaptureSnapshot(CameraSettings settings)
        {
            var outputFolder = ResolveSnapshotFolder(settings);
            Directory.CreateDirectory(outputFolder);

            var filePath = BuildSnapshotPath(outputFolder, settings);
            if (!_frameRecorder.SaveRollingPng(filePath))
            {
                throw new InvalidOperationException("No rolling image is available to save.");
            }

            return filePath;
        }

        private string SaveManualRollingSnapshot(Action<int, string> reportProgress)
        {
            var outputFolder = Path.Combine(Application.StartupPath, "snapshot");
            Directory.CreateDirectory(outputFolder);

            var baseName = "rolling_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
            var filePath = BuildManualSnapshotPath(outputFolder, baseName);
            if (!_frameRecorder.SaveRollingPng(filePath, reportProgress))
            {
                throw new InvalidOperationException("No rolling image is available to save.");
            }

            if (!File.Exists(filePath))
            {
                throw new IOException("Snapshot file was not created: " + filePath);
            }

            return filePath;
        }

        private static string SaveManualSnapshotBitmap(Bitmap bitmap, bool rollingCaptureEnabled)
        {
            var outputFolder = Path.Combine(Application.StartupPath, "snapshot");
            Directory.CreateDirectory(outputFolder);

            var baseName = rollingCaptureEnabled
                ? "rolling_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")
                : DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
            var filePath = BuildManualSnapshotPath(outputFolder, baseName);
            bitmap.Save(filePath, ImageFormat.Png);
            if (!File.Exists(filePath))
            {
                throw new IOException("Snapshot file was not created: " + filePath);
            }

            return filePath;
        }

        private static string BuildManualSnapshotPath(string outputFolder, string baseName)
        {
            var candidatePath = Path.Combine(outputFolder, baseName + ".png");
            if (!File.Exists(candidatePath))
            {
                return candidatePath;
            }

            var suffix = 1;
            while (true)
            {
                var nextPath = Path.Combine(outputFolder, baseName + "_" + suffix.ToString("000") + ".png");
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
            var candidatePath = Path.Combine(outputFolder, baseName + ".png");
            if (!File.Exists(candidatePath))
            {
                return candidatePath;
            }

            var suffix = 1;
            while (true)
            {
                var nextPath = Path.Combine(outputFolder, baseName + "_" + suffix.ToString("000") + ".png");
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

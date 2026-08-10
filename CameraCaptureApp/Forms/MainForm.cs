using System;
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
        private bool _autoConnectAttempted;
        private int _pendingSnapshotSaveCount;
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
            _cameraService.StartPreview();
            UpdateStatus();
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            _cameraService.Disconnect();
            UpdateStatus();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            _cameraService.StopPreview();
            UpdateStatus();
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

            using (var snapshot = _frameRecorder.SnapshotLatest())
            {
                if (snapshot == null)
                {
                    return;
                }

                try
                {
                    var savedPath = await Task.Run(() => SaveSnapshotBitmap(snapshot, _settings));
                    labelFooterMessageValue.Text = "Captured image saved: " + Path.GetFileName(savedPath);
                }
                catch (Exception ex)
                {
                    labelFooterMessageValue.Text = "Captured image save failed: " + ex.Message;
                }
            }
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
            _statusRefreshTimer.Stop();
            _statusRefreshTimer.Dispose();
            CancelPendingImageLoad();
            CancelPendingPreviewFrame();
            _frameRecorder.Dispose();
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
            SaveLatestRecordedFrameIfRequestedAsync();
            var previousFrame = Interlocked.Exchange(ref _pendingPreviewFrame, e.Frame);
            if (previousFrame != null)
            {
                previousFrame.Dispose();
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
                if (Interlocked.CompareExchange(ref _pendingPreviewFrame, null, null) != null &&
                    Interlocked.Exchange(ref _previewFrameUiUpdateQueued, 1) == 0 &&
                    !IsDisposed &&
                    IsHandleCreated)
                {
                    BeginInvoke(new Action(ProcessPendingPreviewFrameAsync));
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
                await _cameraDisplayControl.ShowFrameAsync(frame, token);
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
            bitmap.Save(filePath, ImageFormat.Bmp);
            return filePath;
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
            var candidatePath = Path.Combine(outputFolder, baseName + ".bmp");
            if (!File.Exists(candidatePath))
            {
                return candidatePath;
            }

            var suffix = 1;
            while (true)
            {
                var nextPath = Path.Combine(outputFolder, baseName + "_" + suffix.ToString("000") + ".bmp");
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

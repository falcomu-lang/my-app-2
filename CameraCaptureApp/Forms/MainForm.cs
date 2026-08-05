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
        private readonly System.Windows.Forms.Timer _statusRefreshTimer;
        private CameraSettings _settings;
        private CancellationTokenSource _imageLoadTokenSource;
        private CancellationTokenSource _previewFrameTokenSource;
        private bool _autoConnectAttempted;
        private int _pendingSnapshotSaveCount;

        public MainForm(ICameraService cameraService, ISettingsService settingsService)
        {
            _cameraService = cameraService;
            _settingsService = settingsService;
            _settings = _settingsService.Load() ?? CameraSettings.CreateDefault();

            InitializeComponent();

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
            using (var form = new CameraSettingsForm(dialogSettings, _cameraService))
            {
                if (form.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                _settings = form.Settings.Clone();
                _settingsService.Save(_settings);
                _cameraService.ApplySettings(_settings);
                ApplySettingsToUi();
                UpdateStatus();
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
                labelFooterMessageValue.Text = "Capture requested. Waiting for frame to save...";
            }

            UpdateStatus();
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

            BeginInvoke(new Action(() => DisplayPreviewFrameAsync(e.Frame)));
        }

        private async void DisplayPreviewFrameAsync(Bitmap frame)
        {
            await SaveFrameIfRequestedAsync(frame);
            CancelPendingPreviewFrame();
            _previewFrameTokenSource = new CancellationTokenSource();
            var token = _previewFrameTokenSource.Token;

            try
            {
                await _cameraDisplayControl.ShowFrameAsync(frame, token);
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                frame.Dispose();
            }
        }

        private async Task SaveFrameIfRequestedAsync(Bitmap frame)
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

            using (var snapshot = new Bitmap(frame))
            {
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

            buttonConnect.Enabled = !status.IsConnected;
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

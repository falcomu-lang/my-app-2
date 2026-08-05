using System;
using System.Drawing;
using System.IO;
using System.Threading;
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

        public MainForm(ICameraService cameraService, ISettingsService settingsService)
        {
            _cameraService = cameraService;
            _settingsService = settingsService;
            _settings = _settingsService.Load();

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
        }

        private void buttonCameraSettings_Click(object sender, EventArgs e)
        {
            using (var form = new CameraSettingsForm(_settings, _cameraService))
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
            _cameraService.CaptureFrame();
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

        private void StatusRefreshTimer_Tick(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void ApplySettingsToUi()
        {
            labelHeaderResolutionValue.Text = _settings.Width + " x " + _settings.Height;
            labelHeaderTriggerValue.Text = GetTriggerModeDisplay(_settings.TriggerMode);
            labelHeaderCameraValue.Text = _settings.CameraName;
            _cameraDisplayControl.ResolutionText = _settings.Width + " x " + _settings.Height;
            _cameraDisplayControl.OverlayText = "Ready for preview";
            textBoxConfigPath.Text = _settings.ConfigFilePath;
            textBoxSaveFolder.Text = _settings.SaveFolder;
        }

        private void UpdateStatus()
        {
            var status = _cameraService.Status;

            labelHeaderConnectionValue.Text = status.IsConnected ? "Connected" : "Offline";
            labelHeaderSignalValue.Text = status.HasSignal ? "Detected" : "Missing";
            labelFooterLinesValue.Text = status.ScannedLineCount.ToString();
            labelFooterPreviewValue.Text = status.IsPreviewing ? "Running" : "Stopped";
            labelFooterImageSizeValue.Text = status.FrameWidth + " x " + status.FrameHeight;
            labelFooterMessageValue.Text = status.LastMessage;
            labelFooterUpdateRateValue.Text = status.UpdateRateHz + " Hz";
            labelFooterScanStateValue.Text = status.ScanStateText;
            if (!string.IsNullOrWhiteSpace(status.CameraName))
            {
                labelHeaderCameraValue.Text = status.CameraName;
            }
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
    }
}

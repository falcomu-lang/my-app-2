using System;
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
        private CameraSettings _settings;
        private CancellationTokenSource _imageLoadTokenSource;
        private readonly System.Windows.Forms.Timer _statusRefreshTimer;

        public MainForm(ICameraService cameraService, ISettingsService settingsService)
        {
            _cameraService = cameraService;
            _settingsService = settingsService;
            _settings = _settingsService.Load();

            InitializeComponent();
            _cameraDisplayControl = new Controls.CameraDisplayControl();
            _cameraDisplayControl.Dock = DockStyle.Fill;
            panelViewerHost.Controls.Add(_cameraDisplayControl);
            ApplySettingsToUi();
            UpdateStatus();

            _statusRefreshTimer = new System.Windows.Forms.Timer();
            _statusRefreshTimer.Interval = 300;
            _statusRefreshTimer.Tick += StatusRefreshTimer_Tick;
            _statusRefreshTimer.Start();
        }

        private void buttonCameraSettings_Click(object sender, EventArgs e)
        {
            using (var form = new CameraSettingsForm(_settings))
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
            _cameraService.Connect();
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
                dialog.Title = "載入線掃描測試圖片";

                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                CancelPendingImageLoad();
                _imageLoadTokenSource = new CancellationTokenSource();
                var token = _imageLoadTokenSource.Token;
                labelFooterMessageValue.Text = "載入線掃描圖片中...";

                try
                {
                    await _cameraDisplayControl.LoadImageFromFileAsync(dialog.FileName, token);
                    labelFooterMessageValue.Text = "已載入長圖: " + Path.GetFileName(dialog.FileName);
                }
                catch (OperationCanceledException)
                {
                    labelFooterMessageValue.Text = "已取消載入圖片。";
                }
                catch (Exception ex)
                {
                    labelFooterMessageValue.Text = "載入失敗: " + ex.Message;
                }
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _statusRefreshTimer.Stop();
            _statusRefreshTimer.Dispose();
            CancelPendingImageLoad();
            _cameraService.Disconnect();
            base.OnFormClosed(e);
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
            _cameraDisplayControl.OverlayText = "線掃描長圖預覽區";
            textBoxConfigPath.Text = _settings.ConfigFilePath;
            textBoxSaveFolder.Text = _settings.SaveFolder;
        }

        private void UpdateStatus()
        {
            var status = _cameraService.Status;

            labelHeaderConnectionValue.Text = status.IsConnected ? "已連線" : "未連線";
            labelHeaderSignalValue.Text = status.HasSignal ? "正常" : "無訊號";
            labelFooterLinesValue.Text = status.ScannedLineCount.ToString();
            labelFooterPreviewValue.Text = status.IsPreviewing ? "掃描中" : "已停止";
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

        private static string GetTriggerModeDisplay(TriggerMode mode)
        {
            switch (mode)
            {
                case TriggerMode.SingleFrame:
                    return "單張";
                case TriggerMode.SoftwareTrigger:
                    return "軟體觸發";
                case TriggerMode.ExternalTrigger:
                    return "外部觸發";
                default:
                    return "連續";
            }
        }
    }
}

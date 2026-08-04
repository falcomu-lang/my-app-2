namespace CameraCaptureApp.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Panel panelViewerHost;
        private System.Windows.Forms.Button buttonCameraSettings;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Button buttonStartPreview;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonCapture;
        private System.Windows.Forms.Button buttonLoadImage;
        private System.Windows.Forms.GroupBox groupBoxQuickInfo;
        private System.Windows.Forms.TextBox textBoxConfigPath;
        private System.Windows.Forms.TextBox textBoxSaveFolder;
        private System.Windows.Forms.Label labelConfigPath;
        private System.Windows.Forms.Label labelSaveFolder;
        private System.Windows.Forms.Label labelHeaderConnection;
        private System.Windows.Forms.Label labelHeaderConnectionValue;
        private System.Windows.Forms.Label labelHeaderCamera;
        private System.Windows.Forms.Label labelHeaderCameraValue;
        private System.Windows.Forms.Label labelHeaderResolution;
        private System.Windows.Forms.Label labelHeaderResolutionValue;
        private System.Windows.Forms.Label labelHeaderTrigger;
        private System.Windows.Forms.Label labelHeaderTriggerValue;
        private System.Windows.Forms.Label labelHeaderSignal;
        private System.Windows.Forms.Label labelHeaderSignalValue;
        private System.Windows.Forms.Label labelFooterLines;
        private System.Windows.Forms.Label labelFooterLinesValue;
        private System.Windows.Forms.Label labelFooterPreview;
        private System.Windows.Forms.Label labelFooterPreviewValue;
        private System.Windows.Forms.Label labelFooterImageSize;
        private System.Windows.Forms.Label labelFooterImageSizeValue;
        private System.Windows.Forms.Label labelFooterUpdateRate;
        private System.Windows.Forms.Label labelFooterUpdateRateValue;
        private System.Windows.Forms.Label labelFooterScanState;
        private System.Windows.Forms.Label labelFooterScanStateValue;
        private System.Windows.Forms.Label labelFooterMessage;
        private System.Windows.Forms.Label labelFooterMessageValue;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelHeader = new System.Windows.Forms.Panel();
            this.labelHeaderSignalValue = new System.Windows.Forms.Label();
            this.labelHeaderSignal = new System.Windows.Forms.Label();
            this.labelHeaderTriggerValue = new System.Windows.Forms.Label();
            this.labelHeaderTrigger = new System.Windows.Forms.Label();
            this.labelHeaderResolutionValue = new System.Windows.Forms.Label();
            this.labelHeaderResolution = new System.Windows.Forms.Label();
            this.labelHeaderCameraValue = new System.Windows.Forms.Label();
            this.labelHeaderCamera = new System.Windows.Forms.Label();
            this.labelHeaderConnectionValue = new System.Windows.Forms.Label();
            this.labelHeaderConnection = new System.Windows.Forms.Label();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.groupBoxQuickInfo = new System.Windows.Forms.GroupBox();
            this.textBoxSaveFolder = new System.Windows.Forms.TextBox();
            this.labelSaveFolder = new System.Windows.Forms.Label();
            this.textBoxConfigPath = new System.Windows.Forms.TextBox();
            this.labelConfigPath = new System.Windows.Forms.Label();
            this.buttonLoadImage = new System.Windows.Forms.Button();
            this.buttonCapture = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonStartPreview = new System.Windows.Forms.Button();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.buttonCameraSettings = new System.Windows.Forms.Button();
            this.panelContent = new System.Windows.Forms.Panel();
            this.panelViewerHost = new System.Windows.Forms.Panel();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.labelFooterMessageValue = new System.Windows.Forms.Label();
            this.labelFooterMessage = new System.Windows.Forms.Label();
            this.labelFooterScanStateValue = new System.Windows.Forms.Label();
            this.labelFooterScanState = new System.Windows.Forms.Label();
            this.labelFooterUpdateRateValue = new System.Windows.Forms.Label();
            this.labelFooterUpdateRate = new System.Windows.Forms.Label();
            this.labelFooterImageSizeValue = new System.Windows.Forms.Label();
            this.labelFooterImageSize = new System.Windows.Forms.Label();
            this.labelFooterPreviewValue = new System.Windows.Forms.Label();
            this.labelFooterPreview = new System.Windows.Forms.Label();
            this.labelFooterLinesValue = new System.Windows.Forms.Label();
            this.labelFooterLines = new System.Windows.Forms.Label();
            this.panelHeader.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.groupBoxQuickInfo.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(84, 120, 196);
            this.panelHeader.Controls.Add(this.labelHeaderSignalValue);
            this.panelHeader.Controls.Add(this.labelHeaderSignal);
            this.panelHeader.Controls.Add(this.labelHeaderTriggerValue);
            this.panelHeader.Controls.Add(this.labelHeaderTrigger);
            this.panelHeader.Controls.Add(this.labelHeaderResolutionValue);
            this.panelHeader.Controls.Add(this.labelHeaderResolution);
            this.panelHeader.Controls.Add(this.labelHeaderCameraValue);
            this.panelHeader.Controls.Add(this.labelHeaderCamera);
            this.panelHeader.Controls.Add(this.labelHeaderConnectionValue);
            this.panelHeader.Controls.Add(this.labelHeaderConnection);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1382, 64);
            // 
            // Header labels
            // 
            this.labelHeaderConnection.AutoSize = true;
            this.labelHeaderConnection.Font = new System.Drawing.Font("Microsoft JhengHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelHeaderConnection.ForeColor = System.Drawing.Color.White;
            this.labelHeaderConnection.Location = new System.Drawing.Point(18, 21);
            this.labelHeaderConnection.Text = "連線狀態";
            this.labelHeaderConnectionValue.AutoSize = true;
            this.labelHeaderConnectionValue.Font = new System.Drawing.Font("Microsoft JhengHei UI", 10F);
            this.labelHeaderConnectionValue.ForeColor = System.Drawing.Color.White;
            this.labelHeaderConnectionValue.Location = new System.Drawing.Point(106, 21);
            this.labelHeaderConnectionValue.Text = "未連線";
            this.labelHeaderCamera.AutoSize = true;
            this.labelHeaderCamera.Font = new System.Drawing.Font("Microsoft JhengHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelHeaderCamera.ForeColor = System.Drawing.Color.White;
            this.labelHeaderCamera.Location = new System.Drawing.Point(220, 21);
            this.labelHeaderCamera.Text = "線掃描相機";
            this.labelHeaderCameraValue.AutoSize = true;
            this.labelHeaderCameraValue.Font = new System.Drawing.Font("Microsoft JhengHei UI", 10F);
            this.labelHeaderCameraValue.ForeColor = System.Drawing.Color.White;
            this.labelHeaderCameraValue.Location = new System.Drawing.Point(322, 21);
            this.labelHeaderCameraValue.Text = "Default Camera";
            this.labelHeaderResolution.AutoSize = true;
            this.labelHeaderResolution.Font = new System.Drawing.Font("Microsoft JhengHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelHeaderResolution.ForeColor = System.Drawing.Color.White;
            this.labelHeaderResolution.Location = new System.Drawing.Point(536, 21);
            this.labelHeaderResolution.Text = "介面基準";
            this.labelHeaderResolutionValue.AutoSize = true;
            this.labelHeaderResolutionValue.Font = new System.Drawing.Font("Microsoft JhengHei UI", 10F);
            this.labelHeaderResolutionValue.ForeColor = System.Drawing.Color.White;
            this.labelHeaderResolutionValue.Location = new System.Drawing.Point(624, 21);
            this.labelHeaderResolutionValue.Text = "1280 x 720";
            this.labelHeaderTrigger.AutoSize = true;
            this.labelHeaderTrigger.Font = new System.Drawing.Font("Microsoft JhengHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelHeaderTrigger.ForeColor = System.Drawing.Color.White;
            this.labelHeaderTrigger.Location = new System.Drawing.Point(809, 21);
            this.labelHeaderTrigger.Text = "更新模式";
            this.labelHeaderTriggerValue.AutoSize = true;
            this.labelHeaderTriggerValue.Font = new System.Drawing.Font("Microsoft JhengHei UI", 10F);
            this.labelHeaderTriggerValue.ForeColor = System.Drawing.Color.White;
            this.labelHeaderTriggerValue.Location = new System.Drawing.Point(897, 21);
            this.labelHeaderTriggerValue.Text = "連續";
            this.labelHeaderSignal.AutoSize = true;
            this.labelHeaderSignal.Font = new System.Drawing.Font("Microsoft JhengHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelHeaderSignal.ForeColor = System.Drawing.Color.White;
            this.labelHeaderSignal.Location = new System.Drawing.Point(1046, 21);
            this.labelHeaderSignal.Text = "訊號狀態";
            this.labelHeaderSignalValue.AutoSize = true;
            this.labelHeaderSignalValue.Font = new System.Drawing.Font("Microsoft JhengHei UI", 10F);
            this.labelHeaderSignalValue.ForeColor = System.Drawing.Color.White;
            this.labelHeaderSignalValue.Location = new System.Drawing.Point(1134, 21);
            this.labelHeaderSignalValue.Text = "無訊號";
            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = System.Drawing.Color.FromArgb(10, 14, 24);
            this.panelLeft.Controls.Add(this.groupBoxQuickInfo);
            this.panelLeft.Controls.Add(this.buttonLoadImage);
            this.panelLeft.Controls.Add(this.buttonCapture);
            this.panelLeft.Controls.Add(this.buttonStop);
            this.panelLeft.Controls.Add(this.buttonStartPreview);
            this.panelLeft.Controls.Add(this.buttonConnect);
            this.panelLeft.Controls.Add(this.buttonCameraSettings);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 64);
            this.panelLeft.Padding = new System.Windows.Forms.Padding(16);
            this.panelLeft.Size = new System.Drawing.Size(290, 697);
            // 
            // Buttons
            // 
            this.buttonCameraSettings.BackColor = System.Drawing.Color.FromArgb(84, 120, 196);
            this.buttonCameraSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCameraSettings.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.buttonCameraSettings.ForeColor = System.Drawing.Color.White;
            this.buttonCameraSettings.Location = new System.Drawing.Point(19, 20);
            this.buttonCameraSettings.Size = new System.Drawing.Size(252, 54);
            this.buttonCameraSettings.Text = "攝影機設定";
            this.buttonCameraSettings.Click += new System.EventHandler(this.buttonCameraSettings_Click);
            this.buttonConnect.BackColor = System.Drawing.Color.FromArgb(36, 51, 84);
            this.buttonConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonConnect.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.buttonConnect.ForeColor = System.Drawing.Color.White;
            this.buttonConnect.Location = new System.Drawing.Point(19, 86);
            this.buttonConnect.Size = new System.Drawing.Size(252, 48);
            this.buttonConnect.Text = "連線";
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            this.buttonStartPreview.BackColor = System.Drawing.Color.FromArgb(36, 51, 84);
            this.buttonStartPreview.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonStartPreview.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.buttonStartPreview.ForeColor = System.Drawing.Color.White;
            this.buttonStartPreview.Location = new System.Drawing.Point(19, 146);
            this.buttonStartPreview.Size = new System.Drawing.Size(252, 48);
            this.buttonStartPreview.Text = "開始掃描";
            this.buttonStartPreview.Click += new System.EventHandler(this.buttonStartPreview_Click);
            this.buttonStop.BackColor = System.Drawing.Color.FromArgb(36, 51, 84);
            this.buttonStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonStop.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.buttonStop.ForeColor = System.Drawing.Color.White;
            this.buttonStop.Location = new System.Drawing.Point(19, 206);
            this.buttonStop.Size = new System.Drawing.Size(252, 48);
            this.buttonStop.Text = "停止掃描";
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            this.buttonCapture.BackColor = System.Drawing.Color.FromArgb(84, 120, 196);
            this.buttonCapture.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCapture.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.buttonCapture.ForeColor = System.Drawing.Color.White;
            this.buttonCapture.Location = new System.Drawing.Point(19, 266);
            this.buttonCapture.Size = new System.Drawing.Size(120, 42);
            this.buttonCapture.Text = "單次擷取";
            this.buttonCapture.Click += new System.EventHandler(this.buttonCapture_Click);
            this.buttonLoadImage.BackColor = System.Drawing.Color.FromArgb(36, 51, 84);
            this.buttonLoadImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLoadImage.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.buttonLoadImage.ForeColor = System.Drawing.Color.White;
            this.buttonLoadImage.Location = new System.Drawing.Point(151, 266);
            this.buttonLoadImage.Size = new System.Drawing.Size(120, 42);
            this.buttonLoadImage.Text = "載入長圖";
            this.buttonLoadImage.Click += new System.EventHandler(this.buttonLoadImage_Click);
            // 
            // groupBoxQuickInfo
            // 
            this.groupBoxQuickInfo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.groupBoxQuickInfo.Controls.Add(this.textBoxSaveFolder);
            this.groupBoxQuickInfo.Controls.Add(this.labelSaveFolder);
            this.groupBoxQuickInfo.Controls.Add(this.textBoxConfigPath);
            this.groupBoxQuickInfo.Controls.Add(this.labelConfigPath);
            this.groupBoxQuickInfo.ForeColor = System.Drawing.Color.White;
            this.groupBoxQuickInfo.Location = new System.Drawing.Point(19, 320);
            this.groupBoxQuickInfo.Size = new System.Drawing.Size(252, 358);
            this.groupBoxQuickInfo.Text = "快速資訊";
            this.labelConfigPath.AutoSize = true;
            this.labelConfigPath.Location = new System.Drawing.Point(16, 39);
            this.labelConfigPath.Text = "設定檔";
            this.textBoxConfigPath.Location = new System.Drawing.Point(19, 61);
            this.textBoxConfigPath.Multiline = true;
            this.textBoxConfigPath.ReadOnly = true;
            this.textBoxConfigPath.Size = new System.Drawing.Size(214, 102);
            this.labelSaveFolder.AutoSize = true;
            this.labelSaveFolder.Location = new System.Drawing.Point(16, 192);
            this.labelSaveFolder.Text = "儲存路徑";
            this.textBoxSaveFolder.Location = new System.Drawing.Point(19, 214);
            this.textBoxSaveFolder.Multiline = true;
            this.textBoxSaveFolder.ReadOnly = true;
            this.textBoxSaveFolder.Size = new System.Drawing.Size(214, 102);
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.Color.Black;
            this.panelContent.Controls.Add(this.panelViewerHost);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(290, 64);
            this.panelContent.Padding = new System.Windows.Forms.Padding(24);
            this.panelContent.Size = new System.Drawing.Size(1092, 581);
            // 
            // panelViewerHost
            // 
            this.panelViewerHost.BackColor = System.Drawing.Color.Black;
            this.panelViewerHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelViewerHost.Location = new System.Drawing.Point(24, 24);
            this.panelViewerHost.Name = "panelViewerHost";
            this.panelViewerHost.Size = new System.Drawing.Size(1044, 533);
            // 
            // panelFooter
            // 
            this.panelFooter.BackColor = System.Drawing.Color.FromArgb(10, 14, 24);
            this.panelFooter.Controls.Add(this.labelFooterMessageValue);
            this.panelFooter.Controls.Add(this.labelFooterMessage);
            this.panelFooter.Controls.Add(this.labelFooterScanStateValue);
            this.panelFooter.Controls.Add(this.labelFooterScanState);
            this.panelFooter.Controls.Add(this.labelFooterUpdateRateValue);
            this.panelFooter.Controls.Add(this.labelFooterUpdateRate);
            this.panelFooter.Controls.Add(this.labelFooterImageSizeValue);
            this.panelFooter.Controls.Add(this.labelFooterImageSize);
            this.panelFooter.Controls.Add(this.labelFooterPreviewValue);
            this.panelFooter.Controls.Add(this.labelFooterPreview);
            this.panelFooter.Controls.Add(this.labelFooterLinesValue);
            this.panelFooter.Controls.Add(this.labelFooterLines);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(290, 645);
            this.panelFooter.Size = new System.Drawing.Size(1092, 116);
            this.labelFooterLines.AutoSize = true;
            this.labelFooterLines.Font = new System.Drawing.Font("Microsoft JhengHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelFooterLines.ForeColor = System.Drawing.Color.White;
            this.labelFooterLines.Location = new System.Drawing.Point(27, 22);
            this.labelFooterLines.Text = "累積行數";
            this.labelFooterLinesValue.AutoSize = true;
            this.labelFooterLinesValue.Font = new System.Drawing.Font("Microsoft JhengHei UI", 10F);
            this.labelFooterLinesValue.ForeColor = System.Drawing.Color.White;
            this.labelFooterLinesValue.Location = new System.Drawing.Point(115, 22);
            this.labelFooterLinesValue.Text = "0";
            this.labelFooterPreview.AutoSize = true;
            this.labelFooterPreview.Font = new System.Drawing.Font("Microsoft JhengHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelFooterPreview.ForeColor = System.Drawing.Color.White;
            this.labelFooterPreview.Location = new System.Drawing.Point(210, 22);
            this.labelFooterPreview.Text = "掃描狀態";
            this.labelFooterPreviewValue.AutoSize = true;
            this.labelFooterPreviewValue.Font = new System.Drawing.Font("Microsoft JhengHei UI", 10F);
            this.labelFooterPreviewValue.ForeColor = System.Drawing.Color.White;
            this.labelFooterPreviewValue.Location = new System.Drawing.Point(298, 22);
            this.labelFooterPreviewValue.Text = "已停止";
            this.labelFooterImageSize.AutoSize = true;
            this.labelFooterImageSize.Font = new System.Drawing.Font("Microsoft JhengHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelFooterImageSize.ForeColor = System.Drawing.Color.White;
            this.labelFooterImageSize.Location = new System.Drawing.Point(399, 22);
            this.labelFooterImageSize.Text = "長圖尺寸";
            this.labelFooterImageSizeValue.AutoSize = true;
            this.labelFooterImageSizeValue.Font = new System.Drawing.Font("Microsoft JhengHei UI", 10F);
            this.labelFooterImageSizeValue.ForeColor = System.Drawing.Color.White;
            this.labelFooterImageSizeValue.Location = new System.Drawing.Point(487, 22);
            this.labelFooterImageSizeValue.Text = "1280 x 720";
            this.labelFooterUpdateRate.AutoSize = true;
            this.labelFooterUpdateRate.Font = new System.Drawing.Font("Microsoft JhengHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelFooterUpdateRate.ForeColor = System.Drawing.Color.White;
            this.labelFooterUpdateRate.Location = new System.Drawing.Point(655, 22);
            this.labelFooterUpdateRate.Text = "更新率";
            this.labelFooterUpdateRateValue.AutoSize = true;
            this.labelFooterUpdateRateValue.Font = new System.Drawing.Font("Microsoft JhengHei UI", 10F);
            this.labelFooterUpdateRateValue.ForeColor = System.Drawing.Color.White;
            this.labelFooterUpdateRateValue.Location = new System.Drawing.Point(729, 22);
            this.labelFooterUpdateRateValue.Text = "5 Hz";
            this.labelFooterScanState.AutoSize = true;
            this.labelFooterScanState.Font = new System.Drawing.Font("Microsoft JhengHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelFooterScanState.ForeColor = System.Drawing.Color.White;
            this.labelFooterScanState.Location = new System.Drawing.Point(824, 22);
            this.labelFooterScanState.Text = "累積模式";
            this.labelFooterScanStateValue.AutoSize = true;
            this.labelFooterScanStateValue.Font = new System.Drawing.Font("Microsoft JhengHei UI", 10F);
            this.labelFooterScanStateValue.ForeColor = System.Drawing.Color.White;
            this.labelFooterScanStateValue.Location = new System.Drawing.Point(912, 22);
            this.labelFooterScanStateValue.Text = "待命";
            this.labelFooterMessage.AutoSize = true;
            this.labelFooterMessage.Font = new System.Drawing.Font("Microsoft JhengHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelFooterMessage.ForeColor = System.Drawing.Color.White;
            this.labelFooterMessage.Location = new System.Drawing.Point(27, 68);
            this.labelFooterMessage.Text = "系統訊息";
            this.labelFooterMessageValue.AutoSize = true;
            this.labelFooterMessageValue.Font = new System.Drawing.Font("Microsoft JhengHei UI", 10F);
            this.labelFooterMessageValue.ForeColor = System.Drawing.Color.White;
            this.labelFooterMessageValue.Location = new System.Drawing.Point(115, 68);
            this.labelFooterMessageValue.Text = "Line-scan camera scaffold ready. SDK integration pending.";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1382, 761);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelFooter);
            this.Controls.Add(this.panelLeft);
            this.Controls.Add(this.panelHeader);
            this.MinimumSize = new System.Drawing.Size(1200, 700);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Camera Capture App - Line Scan";
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelLeft.ResumeLayout(false);
            this.groupBoxQuickInfo.ResumeLayout(false);
            this.groupBoxQuickInfo.PerformLayout();
            this.panelContent.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            this.panelFooter.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}

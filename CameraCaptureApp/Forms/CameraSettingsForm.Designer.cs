namespace CameraCaptureApp.Forms
{
    partial class CameraSettingsForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TabControl tabControlSettings;
        private System.Windows.Forms.TabPage tabPageConnection;
        private System.Windows.Forms.TabPage tabPageImage;
        private System.Windows.Forms.TabPage tabPageTrigger;
        private System.Windows.Forms.TabPage tabPageSaving;
        private System.Windows.Forms.TabPage tabPageDiagnostic;
        private System.Windows.Forms.ComboBox comboBoxCameraName;
        private System.Windows.Forms.TextBox textBoxConfigFile;
        private System.Windows.Forms.TextBox textBoxServerName;
        private System.Windows.Forms.TextBox textBoxServerIndex;
        private System.Windows.Forms.TextBox textBoxResourceIndex;
        private System.Windows.Forms.CheckBox checkBoxAutoConnect;
        private System.Windows.Forms.Button buttonBrowseSapera;
        private System.Windows.Forms.Button buttonTestConnection;
        private System.Windows.Forms.NumericUpDown numericWidth;
        private System.Windows.Forms.NumericUpDown numericHeight;
        private System.Windows.Forms.NumericUpDown numericExposure;
        private System.Windows.Forms.NumericUpDown numericGain;
        private System.Windows.Forms.NumericUpDown numericFrameRate;
        private System.Windows.Forms.ComboBox comboBoxPixelFormat;
        private System.Windows.Forms.ComboBox comboBoxTriggerMode;
        private System.Windows.Forms.CheckBox checkBoxAutoSave;
        private System.Windows.Forms.TextBox textBoxSaveFolder;
        private System.Windows.Forms.TextBox textBoxFileNamePattern;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelDiagnosticConnectionValue;
        private System.Windows.Forms.Label labelDiagnosticSignalValue;
        private System.Windows.Forms.Label labelDiagnosticResolutionValue;
        private System.Windows.Forms.Label labelDiagnosticMessageValue;

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
            this.tabControlSettings = new System.Windows.Forms.TabControl();
            this.tabPageConnection = new System.Windows.Forms.TabPage();
            this.buttonTestConnection = new System.Windows.Forms.Button();
            this.buttonBrowseSapera = new System.Windows.Forms.Button();
            this.checkBoxAutoConnect = new System.Windows.Forms.CheckBox();
            this.textBoxResourceIndex = new System.Windows.Forms.TextBox();
            this.textBoxServerIndex = new System.Windows.Forms.TextBox();
            this.textBoxServerName = new System.Windows.Forms.TextBox();
            this.textBoxConfigFile = new System.Windows.Forms.TextBox();
            this.comboBoxCameraName = new System.Windows.Forms.ComboBox();
            this.tabPageImage = new System.Windows.Forms.TabPage();
            this.comboBoxPixelFormat = new System.Windows.Forms.ComboBox();
            this.numericFrameRate = new System.Windows.Forms.NumericUpDown();
            this.numericGain = new System.Windows.Forms.NumericUpDown();
            this.numericExposure = new System.Windows.Forms.NumericUpDown();
            this.numericHeight = new System.Windows.Forms.NumericUpDown();
            this.numericWidth = new System.Windows.Forms.NumericUpDown();
            this.tabPageTrigger = new System.Windows.Forms.TabPage();
            this.comboBoxTriggerMode = new System.Windows.Forms.ComboBox();
            this.tabPageSaving = new System.Windows.Forms.TabPage();
            this.textBoxFileNamePattern = new System.Windows.Forms.TextBox();
            this.textBoxSaveFolder = new System.Windows.Forms.TextBox();
            this.checkBoxAutoSave = new System.Windows.Forms.CheckBox();
            this.tabPageDiagnostic = new System.Windows.Forms.TabPage();
            this.labelDiagnosticMessageValue = new System.Windows.Forms.Label();
            this.labelDiagnosticResolutionValue = new System.Windows.Forms.Label();
            this.labelDiagnosticSignalValue = new System.Windows.Forms.Label();
            this.labelDiagnosticConnectionValue = new System.Windows.Forms.Label();
            this.buttonApply = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.tabControlSettings.SuspendLayout();
            this.tabPageConnection.SuspendLayout();
            this.tabPageImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericFrameRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericGain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericExposure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericWidth)).BeginInit();
            this.tabPageTrigger.SuspendLayout();
            this.tabPageSaving.SuspendLayout();
            this.tabPageDiagnostic.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlSettings
            // 
            this.tabControlSettings.Controls.Add(this.tabPageConnection);
            this.tabControlSettings.Controls.Add(this.tabPageImage);
            this.tabControlSettings.Controls.Add(this.tabPageTrigger);
            this.tabControlSettings.Controls.Add(this.tabPageSaving);
            this.tabControlSettings.Controls.Add(this.tabPageDiagnostic);
            this.tabControlSettings.Location = new System.Drawing.Point(12, 12);
            this.tabControlSettings.Name = "tabControlSettings";
            this.tabControlSettings.SelectedIndex = 0;
            this.tabControlSettings.Size = new System.Drawing.Size(760, 380);
            this.tabControlSettings.TabIndex = 0;
            // 
            // tabPageConnection
            // 
            this.tabPageConnection.Controls.Add(new System.Windows.Forms.Label() { AutoSize = true, Location = new System.Drawing.Point(32, 25), Text = "Camera Name" });
            this.tabPageConnection.Controls.Add(this.comboBoxCameraName);
            this.tabPageConnection.Controls.Add(new System.Windows.Forms.Label() { AutoSize = true, Location = new System.Drawing.Point(32, 78), Text = "CCF File" });
            this.tabPageConnection.Controls.Add(this.textBoxConfigFile);
            this.tabPageConnection.Controls.Add(new System.Windows.Forms.Label() { AutoSize = true, Location = new System.Drawing.Point(32, 164), Text = "Acquisition Server" });
            this.tabPageConnection.Controls.Add(this.textBoxServerName);
            this.tabPageConnection.Controls.Add(new System.Windows.Forms.Label() { AutoSize = true, Location = new System.Drawing.Point(32, 217), Text = "Server Index" });
            this.tabPageConnection.Controls.Add(this.textBoxServerIndex);
            this.tabPageConnection.Controls.Add(new System.Windows.Forms.Label() { AutoSize = true, Location = new System.Drawing.Point(196, 217), Text = "Resource Index" });
            this.tabPageConnection.Controls.Add(this.textBoxResourceIndex);
            this.tabPageConnection.Controls.Add(this.checkBoxAutoConnect);
            this.tabPageConnection.Controls.Add(this.buttonBrowseSapera);
            this.tabPageConnection.Controls.Add(this.buttonTestConnection);
            this.tabPageConnection.Location = new System.Drawing.Point(4, 26);
            this.tabPageConnection.Name = "tabPageConnection";
            this.tabPageConnection.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageConnection.Size = new System.Drawing.Size(752, 350);
            this.tabPageConnection.TabIndex = 0;
            this.tabPageConnection.Text = "Basic Connection";
            this.tabPageConnection.UseVisualStyleBackColor = true;
            // 
            // comboBoxCameraName
            // 
            this.comboBoxCameraName.FormattingEnabled = true;
            this.comboBoxCameraName.Location = new System.Drawing.Point(35, 47);
            this.comboBoxCameraName.Name = "comboBoxCameraName";
            this.comboBoxCameraName.Size = new System.Drawing.Size(280, 24);
            this.comboBoxCameraName.TabIndex = 0;
            // 
            // textBoxConfigFile
            // 
            this.textBoxConfigFile.Location = new System.Drawing.Point(35, 100);
            this.textBoxConfigFile.Multiline = true;
            this.textBoxConfigFile.Name = "textBoxConfigFile";
            this.textBoxConfigFile.Size = new System.Drawing.Size(680, 52);
            this.textBoxConfigFile.TabIndex = 1;
            // 
            // textBoxServerName
            // 
            this.textBoxServerName.Location = new System.Drawing.Point(35, 186);
            this.textBoxServerName.Name = "textBoxServerName";
            this.textBoxServerName.ReadOnly = true;
            this.textBoxServerName.Size = new System.Drawing.Size(420, 23);
            this.textBoxServerName.TabIndex = 2;
            // 
            // textBoxServerIndex
            // 
            this.textBoxServerIndex.Location = new System.Drawing.Point(35, 239);
            this.textBoxServerIndex.Name = "textBoxServerIndex";
            this.textBoxServerIndex.ReadOnly = true;
            this.textBoxServerIndex.Size = new System.Drawing.Size(120, 23);
            this.textBoxServerIndex.TabIndex = 3;
            // 
            // textBoxResourceIndex
            // 
            this.textBoxResourceIndex.Location = new System.Drawing.Point(199, 239);
            this.textBoxResourceIndex.Name = "textBoxResourceIndex";
            this.textBoxResourceIndex.ReadOnly = true;
            this.textBoxResourceIndex.Size = new System.Drawing.Size(120, 23);
            this.textBoxResourceIndex.TabIndex = 4;
            // 
            // checkBoxAutoConnect
            // 
            this.checkBoxAutoConnect.AutoSize = true;
            this.checkBoxAutoConnect.Location = new System.Drawing.Point(35, 283);
            this.checkBoxAutoConnect.Name = "checkBoxAutoConnect";
            this.checkBoxAutoConnect.Size = new System.Drawing.Size(143, 20);
            this.checkBoxAutoConnect.TabIndex = 5;
            this.checkBoxAutoConnect.Text = "Auto Connect at Start";
            this.checkBoxAutoConnect.UseVisualStyleBackColor = true;
            // 
            // buttonBrowseSapera
            // 
            this.buttonBrowseSapera.Location = new System.Drawing.Point(430, 278);
            this.buttonBrowseSapera.Name = "buttonBrowseSapera";
            this.buttonBrowseSapera.Size = new System.Drawing.Size(134, 32);
            this.buttonBrowseSapera.TabIndex = 6;
            this.buttonBrowseSapera.Text = "Load From Sapera";
            this.buttonBrowseSapera.UseVisualStyleBackColor = true;
            this.buttonBrowseSapera.Click += new System.EventHandler(this.buttonBrowseSapera_Click);
            // 
            // buttonTestConnection
            // 
            this.buttonTestConnection.Location = new System.Drawing.Point(581, 278);
            this.buttonTestConnection.Name = "buttonTestConnection";
            this.buttonTestConnection.Size = new System.Drawing.Size(134, 32);
            this.buttonTestConnection.TabIndex = 7;
            this.buttonTestConnection.Text = "Check Saved Data";
            this.buttonTestConnection.UseVisualStyleBackColor = true;
            this.buttonTestConnection.Click += new System.EventHandler(this.buttonTestConnection_Click);
            // 
            // tabPageImage
            // 
            this.tabPageImage.Controls.Add(new System.Windows.Forms.Label() { AutoSize = true, Location = new System.Drawing.Point(32, 35), Text = "Width" });
            this.tabPageImage.Controls.Add(this.numericWidth);
            this.tabPageImage.Controls.Add(new System.Windows.Forms.Label() { AutoSize = true, Location = new System.Drawing.Point(208, 35), Text = "Height" });
            this.tabPageImage.Controls.Add(this.numericHeight);
            this.tabPageImage.Controls.Add(new System.Windows.Forms.Label() { AutoSize = true, Location = new System.Drawing.Point(32, 103), Text = "Exposure" });
            this.tabPageImage.Controls.Add(this.numericExposure);
            this.tabPageImage.Controls.Add(new System.Windows.Forms.Label() { AutoSize = true, Location = new System.Drawing.Point(208, 103), Text = "Gain" });
            this.tabPageImage.Controls.Add(this.numericGain);
            this.tabPageImage.Controls.Add(new System.Windows.Forms.Label() { AutoSize = true, Location = new System.Drawing.Point(384, 103), Text = "Frame Rate" });
            this.tabPageImage.Controls.Add(this.numericFrameRate);
            this.tabPageImage.Controls.Add(new System.Windows.Forms.Label() { AutoSize = true, Location = new System.Drawing.Point(32, 171), Text = "Pixel Format" });
            this.tabPageImage.Controls.Add(this.comboBoxPixelFormat);
            this.tabPageImage.Location = new System.Drawing.Point(4, 26);
            this.tabPageImage.Name = "tabPageImage";
            this.tabPageImage.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageImage.Size = new System.Drawing.Size(752, 350);
            this.tabPageImage.TabIndex = 1;
            this.tabPageImage.Text = "Image";
            this.tabPageImage.UseVisualStyleBackColor = true;
            // 
            // image controls
            this.numericWidth.Location = new System.Drawing.Point(35, 57);
            this.numericWidth.Maximum = new decimal(new int[] { 8192, 0, 0, 0 });
            this.numericWidth.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numericWidth.Size = new System.Drawing.Size(140, 23);
            this.numericWidth.Value = new decimal(new int[] { 1280, 0, 0, 0 });
            this.numericHeight.Location = new System.Drawing.Point(211, 57);
            this.numericHeight.Maximum = new decimal(new int[] { 8192, 0, 0, 0 });
            this.numericHeight.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numericHeight.Size = new System.Drawing.Size(140, 23);
            this.numericHeight.Value = new decimal(new int[] { 720, 0, 0, 0 });
            this.numericExposure.DecimalPlaces = 2;
            this.numericExposure.Location = new System.Drawing.Point(35, 125);
            this.numericExposure.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            this.numericExposure.Size = new System.Drawing.Size(140, 23);
            this.numericGain.DecimalPlaces = 2;
            this.numericGain.Location = new System.Drawing.Point(211, 125);
            this.numericGain.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            this.numericGain.Size = new System.Drawing.Size(140, 23);
            this.numericFrameRate.DecimalPlaces = 2;
            this.numericFrameRate.Location = new System.Drawing.Point(387, 125);
            this.numericFrameRate.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            this.numericFrameRate.Size = new System.Drawing.Size(140, 23);
            this.comboBoxPixelFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPixelFormat.FormattingEnabled = true;
            this.comboBoxPixelFormat.Location = new System.Drawing.Point(35, 193);
            this.comboBoxPixelFormat.Size = new System.Drawing.Size(200, 24);
            // 
            // tabPageTrigger
            // 
            this.tabPageTrigger.Controls.Add(new System.Windows.Forms.Label() { AutoSize = true, Location = new System.Drawing.Point(32, 35), Text = "Trigger Mode" });
            this.tabPageTrigger.Controls.Add(this.comboBoxTriggerMode);
            this.tabPageTrigger.Location = new System.Drawing.Point(4, 26);
            this.tabPageTrigger.Name = "tabPageTrigger";
            this.tabPageTrigger.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTrigger.Size = new System.Drawing.Size(752, 350);
            this.tabPageTrigger.TabIndex = 2;
            this.tabPageTrigger.Text = "Trigger";
            this.tabPageTrigger.UseVisualStyleBackColor = true;
            // 
            // comboBoxTriggerMode
            // 
            this.comboBoxTriggerMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTriggerMode.FormattingEnabled = true;
            this.comboBoxTriggerMode.Location = new System.Drawing.Point(35, 57);
            this.comboBoxTriggerMode.Size = new System.Drawing.Size(220, 24);
            // 
            // tabPageSaving
            // 
            this.tabPageSaving.Controls.Add(this.checkBoxAutoSave);
            this.tabPageSaving.Controls.Add(new System.Windows.Forms.Label() { AutoSize = true, Location = new System.Drawing.Point(32, 75), Text = "Save Folder" });
            this.tabPageSaving.Controls.Add(this.textBoxSaveFolder);
            this.tabPageSaving.Controls.Add(new System.Windows.Forms.Label() { AutoSize = true, Location = new System.Drawing.Point(32, 179), Text = "File Name Pattern" });
            this.tabPageSaving.Controls.Add(this.textBoxFileNamePattern);
            this.tabPageSaving.Location = new System.Drawing.Point(4, 26);
            this.tabPageSaving.Name = "tabPageSaving";
            this.tabPageSaving.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSaving.Size = new System.Drawing.Size(752, 350);
            this.tabPageSaving.TabIndex = 3;
            this.tabPageSaving.Text = "Saving";
            this.tabPageSaving.UseVisualStyleBackColor = true;
            // 
            // save controls
            this.checkBoxAutoSave.AutoSize = true;
            this.checkBoxAutoSave.Location = new System.Drawing.Point(35, 31);
            this.checkBoxAutoSave.Size = new System.Drawing.Size(82, 20);
            this.checkBoxAutoSave.Text = "Auto Save";
            this.textBoxSaveFolder.Location = new System.Drawing.Point(35, 97);
            this.textBoxSaveFolder.Multiline = true;
            this.textBoxSaveFolder.Size = new System.Drawing.Size(680, 57);
            this.textBoxFileNamePattern.Location = new System.Drawing.Point(35, 201);
            this.textBoxFileNamePattern.Size = new System.Drawing.Size(320, 23);
            // 
            // tabPageDiagnostic
            // 
            this.tabPageDiagnostic.Controls.Add(new System.Windows.Forms.Label() { AutoSize = true, Location = new System.Drawing.Point(32, 35), Text = "Connection" });
            this.tabPageDiagnostic.Controls.Add(this.labelDiagnosticConnectionValue);
            this.tabPageDiagnostic.Controls.Add(new System.Windows.Forms.Label() { AutoSize = true, Location = new System.Drawing.Point(32, 83), Text = "Signal" });
            this.tabPageDiagnostic.Controls.Add(this.labelDiagnosticSignalValue);
            this.tabPageDiagnostic.Controls.Add(new System.Windows.Forms.Label() { AutoSize = true, Location = new System.Drawing.Point(32, 131), Text = "Resolution" });
            this.tabPageDiagnostic.Controls.Add(this.labelDiagnosticResolutionValue);
            this.tabPageDiagnostic.Controls.Add(new System.Windows.Forms.Label() { AutoSize = true, Location = new System.Drawing.Point(32, 179), Text = "Message" });
            this.tabPageDiagnostic.Controls.Add(this.labelDiagnosticMessageValue);
            this.tabPageDiagnostic.Location = new System.Drawing.Point(4, 26);
            this.tabPageDiagnostic.Name = "tabPageDiagnostic";
            this.tabPageDiagnostic.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDiagnostic.Size = new System.Drawing.Size(752, 350);
            this.tabPageDiagnostic.TabIndex = 4;
            this.tabPageDiagnostic.Text = "Diagnostic";
            this.tabPageDiagnostic.UseVisualStyleBackColor = true;
            // 
            // diagnostic values
            this.labelDiagnosticConnectionValue.AutoSize = true;
            this.labelDiagnosticConnectionValue.Location = new System.Drawing.Point(153, 35);
            this.labelDiagnosticConnectionValue.Text = "Not checked";
            this.labelDiagnosticSignalValue.AutoSize = true;
            this.labelDiagnosticSignalValue.Location = new System.Drawing.Point(153, 83);
            this.labelDiagnosticSignalValue.Text = "Not checked";
            this.labelDiagnosticResolutionValue.AutoSize = true;
            this.labelDiagnosticResolutionValue.Location = new System.Drawing.Point(153, 131);
            this.labelDiagnosticResolutionValue.Text = "1280 x 720";
            this.labelDiagnosticMessageValue.Location = new System.Drawing.Point(153, 179);
            this.labelDiagnosticMessageValue.Size = new System.Drawing.Size(532, 70);
            this.labelDiagnosticMessageValue.Text = "Use the Sapera dialog once, then the saved values can be reused for later connections.";
            // 
            // buttons
            // 
            this.buttonApply.Location = new System.Drawing.Point(446, 406);
            this.buttonApply.Size = new System.Drawing.Size(96, 34);
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            this.buttonOk.Location = new System.Drawing.Point(548, 406);
            this.buttonOk.Size = new System.Drawing.Size(108, 34);
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            this.buttonCancel.Location = new System.Drawing.Point(664, 406);
            this.buttonCancel.Size = new System.Drawing.Size(108, 34);
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // CameraSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 452);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.tabControlSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CameraSettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Camera Settings";
            this.tabControlSettings.ResumeLayout(false);
            this.tabPageConnection.ResumeLayout(false);
            this.tabPageConnection.PerformLayout();
            this.tabPageImage.ResumeLayout(false);
            this.tabPageImage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericFrameRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericGain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericExposure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericWidth)).EndInit();
            this.tabPageTrigger.ResumeLayout(false);
            this.tabPageTrigger.PerformLayout();
            this.tabPageSaving.ResumeLayout(false);
            this.tabPageSaving.PerformLayout();
            this.tabPageDiagnostic.ResumeLayout(false);
            this.tabPageDiagnostic.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}

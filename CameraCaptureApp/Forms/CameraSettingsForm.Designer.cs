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
        private System.Windows.Forms.TextBox textBoxCameraName;
        private System.Windows.Forms.TextBox textBoxConfigFile;
        private System.Windows.Forms.TextBox textBoxServerName;
        private System.Windows.Forms.TextBox textBoxServerIndex;
        private System.Windows.Forms.TextBox textBoxResourceIndex;
        private System.Windows.Forms.CheckBox checkBoxAutoConnect;
        private System.Windows.Forms.Button buttonBrowseSapera;
        private System.Windows.Forms.Button buttonReadCcfToFields;
        private System.Windows.Forms.Button buttonProbeLiveFeatures;
        private System.Windows.Forms.Button buttonProbeAcquisitionParameters;
        private System.Windows.Forms.Label labelReadResult;
        private System.Windows.Forms.NumericUpDown numericExposure;
        private System.Windows.Forms.NumericUpDown numericGain;
        private System.Windows.Forms.NumericUpDown numericLength;
        private System.Windows.Forms.NumericUpDown numericInternalLineRate;
        private System.Windows.Forms.ComboBox comboBoxTriggerMode;
        private System.Windows.Forms.CheckBox checkBoxAutoSave;
        private System.Windows.Forms.TextBox textBoxSaveFolder;
        private System.Windows.Forms.TextBox textBoxFileNamePattern;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;

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
            this.labelReadResult = new System.Windows.Forms.Label();
            this.buttonProbeLiveFeatures = new System.Windows.Forms.Button();
            this.buttonProbeAcquisitionParameters = new System.Windows.Forms.Button();
            this.buttonReadCcfToFields = new System.Windows.Forms.Button();
            this.buttonBrowseSapera = new System.Windows.Forms.Button();
            this.checkBoxAutoConnect = new System.Windows.Forms.CheckBox();
            this.textBoxResourceIndex = new System.Windows.Forms.TextBox();
            this.textBoxServerIndex = new System.Windows.Forms.TextBox();
            this.textBoxServerName = new System.Windows.Forms.TextBox();
            this.textBoxConfigFile = new System.Windows.Forms.TextBox();
            this.textBoxCameraName = new System.Windows.Forms.TextBox();
            this.tabPageImage = new System.Windows.Forms.TabPage();
            this.numericInternalLineRate = new System.Windows.Forms.NumericUpDown();
            this.numericLength = new System.Windows.Forms.NumericUpDown();
            this.numericGain = new System.Windows.Forms.NumericUpDown();
            this.numericExposure = new System.Windows.Forms.NumericUpDown();
            this.tabPageTrigger = new System.Windows.Forms.TabPage();
            this.comboBoxTriggerMode = new System.Windows.Forms.ComboBox();
            this.tabPageSaving = new System.Windows.Forms.TabPage();
            this.textBoxFileNamePattern = new System.Windows.Forms.TextBox();
            this.textBoxSaveFolder = new System.Windows.Forms.TextBox();
            this.checkBoxAutoSave = new System.Windows.Forms.CheckBox();
            this.buttonApply = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.tabControlSettings.SuspendLayout();
            this.tabPageConnection.SuspendLayout();
            this.tabPageImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericInternalLineRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericGain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericExposure)).BeginInit();
            this.tabPageTrigger.SuspendLayout();
            this.tabPageSaving.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlSettings
            // 
            this.tabControlSettings.Controls.Add(this.tabPageConnection);
            this.tabControlSettings.Controls.Add(this.tabPageImage);
            this.tabControlSettings.Controls.Add(this.tabPageTrigger);
            this.tabControlSettings.Controls.Add(this.tabPageSaving);
            this.tabControlSettings.Location = new System.Drawing.Point(12, 12);
            this.tabControlSettings.Name = "tabControlSettings";
            this.tabControlSettings.SelectedIndex = 0;
            this.tabControlSettings.Size = new System.Drawing.Size(760, 380);
            this.tabControlSettings.TabIndex = 0;
            // 
            // tabPageConnection
            // 
            this.tabPageConnection.Controls.Add(new System.Windows.Forms.Label() { AutoSize = true, Location = new System.Drawing.Point(32, 25), Text = "Camera Name" });
            this.tabPageConnection.Controls.Add(this.textBoxCameraName);
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
            this.tabPageConnection.Controls.Add(this.buttonReadCcfToFields);
            this.tabPageConnection.Controls.Add(this.labelReadResult);
            this.tabPageConnection.Location = new System.Drawing.Point(4, 26);
            this.tabPageConnection.Name = "tabPageConnection";
            this.tabPageConnection.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageConnection.Size = new System.Drawing.Size(752, 350);
            this.tabPageConnection.TabIndex = 0;
            this.tabPageConnection.Text = "Basic Connection";
            this.tabPageConnection.UseVisualStyleBackColor = true;
            // 
            // textBoxCameraName
            // 
            this.textBoxCameraName.Location = new System.Drawing.Point(35, 47);
            this.textBoxCameraName.Name = "textBoxCameraName";
            this.textBoxCameraName.Size = new System.Drawing.Size(280, 23);
            this.textBoxCameraName.TabIndex = 0;
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
            this.checkBoxAutoConnect.Location = new System.Drawing.Point(35, 276);
            this.checkBoxAutoConnect.Name = "checkBoxAutoConnect";
            this.checkBoxAutoConnect.Size = new System.Drawing.Size(143, 20);
            this.checkBoxAutoConnect.TabIndex = 5;
            this.checkBoxAutoConnect.Text = "Auto Connect at Start";
            this.checkBoxAutoConnect.UseVisualStyleBackColor = true;
            // 
            // buttonBrowseSapera
            // 
            this.buttonBrowseSapera.Location = new System.Drawing.Point(430, 235);
            this.buttonBrowseSapera.Name = "buttonBrowseSapera";
            this.buttonBrowseSapera.Size = new System.Drawing.Size(138, 32);
            this.buttonBrowseSapera.TabIndex = 8;
            this.buttonBrowseSapera.Text = "Load From Sapera";
            this.buttonBrowseSapera.UseVisualStyleBackColor = true;
            this.buttonBrowseSapera.Click += new System.EventHandler(this.buttonBrowseSapera_Click);
            // 
            // buttonReadCcfToFields
            // 
            this.buttonReadCcfToFields.Location = new System.Drawing.Point(577, 235);
            this.buttonReadCcfToFields.Name = "buttonReadCcfToFields";
            this.buttonReadCcfToFields.Size = new System.Drawing.Size(138, 32);
            this.buttonReadCcfToFields.TabIndex = 10;
            this.buttonReadCcfToFields.Text = "Read CCF To Fields";
            this.buttonReadCcfToFields.UseVisualStyleBackColor = true;
            this.buttonReadCcfToFields.Click += new System.EventHandler(this.buttonReadCcfToFields_Click);
            // 
            // buttonProbeLiveFeatures
            // 
            this.buttonProbeLiveFeatures.Location = new System.Drawing.Point(430, 316);
            this.buttonProbeLiveFeatures.Name = "buttonProbeLiveFeatures";
            this.buttonProbeLiveFeatures.Size = new System.Drawing.Size(285, 32);
            this.buttonProbeLiveFeatures.TabIndex = 8;
            this.buttonProbeLiveFeatures.Text = "Probe Live Features";
            this.buttonProbeLiveFeatures.UseVisualStyleBackColor = true;
            this.buttonProbeLiveFeatures.Click += new System.EventHandler(this.buttonProbeLiveFeatures_Click);
            // 
            // buttonProbeAcquisitionParameters
            // 
            this.buttonProbeAcquisitionParameters.Location = new System.Drawing.Point(430, 240);
            this.buttonProbeAcquisitionParameters.Name = "buttonProbeAcquisitionParameters";
            this.buttonProbeAcquisitionParameters.Size = new System.Drawing.Size(285, 32);
            this.buttonProbeAcquisitionParameters.TabIndex = 8;
            this.buttonProbeAcquisitionParameters.Text = "Probe Acquisition Parameters";
            this.buttonProbeAcquisitionParameters.UseVisualStyleBackColor = true;
            this.buttonProbeAcquisitionParameters.Click += new System.EventHandler(this.buttonProbeAcquisitionParameters_Click);
            // 
            // labelReadResult
            // 
            this.labelReadResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelReadResult.Location = new System.Drawing.Point(35, 307);
            this.labelReadResult.Name = "labelReadResult";
            this.labelReadResult.Padding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.labelReadResult.Size = new System.Drawing.Size(680, 44);
            this.labelReadResult.TabIndex = 10;
            this.labelReadResult.Text = "Load Sapera settings first, then read supported CCF values into the fields.";
            // 
            // tabPageImage
            // 
            this.tabPageImage.Controls.Add(new System.Windows.Forms.Label() { AutoSize = true, Location = new System.Drawing.Point(32, 35), Text = "Exposure" });
            this.tabPageImage.Controls.Add(this.numericExposure);
            this.tabPageImage.Controls.Add(new System.Windows.Forms.Label() { AutoSize = true, Location = new System.Drawing.Point(208, 35), Text = "Gain" });
            this.tabPageImage.Controls.Add(this.numericGain);
            this.tabPageImage.Controls.Add(new System.Windows.Forms.Label() { AutoSize = true, Location = new System.Drawing.Point(384, 35), Text = "Length (Lines)" });
            this.tabPageImage.Controls.Add(this.numericLength);
            this.tabPageImage.Controls.Add(new System.Windows.Forms.Label() { AutoSize = true, Location = new System.Drawing.Point(560, 35), Text = "Internal Line Rate" });
            this.tabPageImage.Controls.Add(this.numericInternalLineRate);
            this.tabPageImage.Controls.Add(new System.Windows.Forms.Label() { AutoSize = true, Location = new System.Drawing.Point(32, 103), Size = new System.Drawing.Size(680, 16), Text = "Exposure, gain, length, and internal line rate will be written to supported Sapera parameters when available." });
            this.tabPageImage.Location = new System.Drawing.Point(4, 26);
            this.tabPageImage.Name = "tabPageImage";
            this.tabPageImage.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageImage.Size = new System.Drawing.Size(752, 350);
            this.tabPageImage.TabIndex = 1;
            this.tabPageImage.Text = "Image";
            this.tabPageImage.UseVisualStyleBackColor = true;
            // 
            // image controls
            // 
            this.numericExposure.DecimalPlaces = 2;
            this.numericExposure.Location = new System.Drawing.Point(35, 57);
            this.numericExposure.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            this.numericExposure.Size = new System.Drawing.Size(140, 23);
            this.numericGain.DecimalPlaces = 2;
            this.numericGain.Location = new System.Drawing.Point(211, 57);
            this.numericGain.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            this.numericGain.Size = new System.Drawing.Size(140, 23);
            this.numericLength.Location = new System.Drawing.Point(387, 57);
            this.numericLength.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.numericLength.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numericLength.Size = new System.Drawing.Size(140, 23);
            this.numericLength.Value = new decimal(new int[] { 1, 0, 0, 0 });
            this.numericInternalLineRate.DecimalPlaces = 2;
            this.numericInternalLineRate.Location = new System.Drawing.Point(563, 57);
            this.numericInternalLineRate.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.numericInternalLineRate.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numericInternalLineRate.Size = new System.Drawing.Size(140, 23);
            this.numericInternalLineRate.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // tabPageTrigger
            // 
            this.tabPageTrigger.Controls.Add(new System.Windows.Forms.Label() { AutoSize = true, Location = new System.Drawing.Point(32, 35), Text = "Trigger Mode" });
            this.tabPageTrigger.Controls.Add(this.comboBoxTriggerMode);
            this.tabPageTrigger.Controls.Add(new System.Windows.Forms.Label() { AutoSize = true, Location = new System.Drawing.Point(32, 103), Size = new System.Drawing.Size(600, 16), Text = "Trigger Mode will be written to supported Sapera trigger features when the camera provides them." });
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
            // 
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
            ((System.ComponentModel.ISupportInitialize)(this.numericInternalLineRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericGain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericExposure)).EndInit();
            this.tabPageTrigger.ResumeLayout(false);
            this.tabPageTrigger.PerformLayout();
            this.tabPageSaving.ResumeLayout(false);
            this.tabPageSaving.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}

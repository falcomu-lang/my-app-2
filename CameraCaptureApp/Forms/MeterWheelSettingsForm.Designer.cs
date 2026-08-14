namespace CameraCaptureApp.Forms
{
    partial class MeterWheelSettingsForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ComboBox comboBoxCardId;
        private System.Windows.Forms.TextBox textBoxCounter;
        private System.Windows.Forms.TextBox textBoxCompareValue;
        private System.Windows.Forms.Button buttonOpenScan;
        private System.Windows.Forms.Button buttonClearCounter;
        private System.Windows.Forms.Button buttonClearCompareValue;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Label labelCardId;
        private System.Windows.Forms.Label labelCounter;
        private System.Windows.Forms.Label labelCompareValue;
        private System.Windows.Forms.Label labelMultipleRate;
        private System.Windows.Forms.Label labelAutoIncrement;
        private System.Windows.Forms.Label labelCmpOutWidth;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.ComboBox comboBoxMultipleRate;
        private System.Windows.Forms.NumericUpDown numericAutoIncrement;
        private System.Windows.Forms.NumericUpDown numericCmpOutWidth;
        private System.Windows.Forms.Button buttonApplySettings;
        private System.Windows.Forms.Timer timerCounterRefresh;

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
            this.components = new System.ComponentModel.Container();
            this.comboBoxCardId = new System.Windows.Forms.ComboBox();
            this.textBoxCounter = new System.Windows.Forms.TextBox();
            this.textBoxCompareValue = new System.Windows.Forms.TextBox();
            this.buttonOpenScan = new System.Windows.Forms.Button();
            this.buttonClearCounter = new System.Windows.Forms.Button();
            this.buttonClearCompareValue = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.labelCardId = new System.Windows.Forms.Label();
            this.labelCounter = new System.Windows.Forms.Label();
            this.labelCompareValue = new System.Windows.Forms.Label();
            this.labelMultipleRate = new System.Windows.Forms.Label();
            this.labelAutoIncrement = new System.Windows.Forms.Label();
            this.labelCmpOutWidth = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.comboBoxMultipleRate = new System.Windows.Forms.ComboBox();
            this.numericAutoIncrement = new System.Windows.Forms.NumericUpDown();
            this.numericCmpOutWidth = new System.Windows.Forms.NumericUpDown();
            this.buttonApplySettings = new System.Windows.Forms.Button();
            this.timerCounterRefresh = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numericAutoIncrement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericCmpOutWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxCardId
            // 
            this.comboBoxCardId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCardId.FormattingEnabled = true;
            this.comboBoxCardId.Location = new System.Drawing.Point(34, 72);
            this.comboBoxCardId.Name = "comboBoxCardId";
            this.comboBoxCardId.Size = new System.Drawing.Size(368, 24);
            this.comboBoxCardId.TabIndex = 1;
            // 
            // textBoxCounter
            // 
            this.textBoxCounter.Font = new System.Drawing.Font("Microsoft JhengHei UI", 16F, System.Drawing.FontStyle.Bold);
            this.textBoxCounter.Location = new System.Drawing.Point(34, 142);
            this.textBoxCounter.Name = "textBoxCounter";
            this.textBoxCounter.ReadOnly = true;
            this.textBoxCounter.Size = new System.Drawing.Size(368, 41);
            this.textBoxCounter.TabIndex = 3;
            this.textBoxCounter.Text = "0";
            this.textBoxCounter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxCompareValue
            // 
            this.textBoxCompareValue.Font = new System.Drawing.Font("Microsoft JhengHei UI", 16F, System.Drawing.FontStyle.Bold);
            this.textBoxCompareValue.Location = new System.Drawing.Point(34, 216);
            this.textBoxCompareValue.Name = "textBoxCompareValue";
            this.textBoxCompareValue.ReadOnly = true;
            this.textBoxCompareValue.Size = new System.Drawing.Size(368, 41);
            this.textBoxCompareValue.TabIndex = 12;
            this.textBoxCompareValue.Text = "0";
            this.textBoxCompareValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // buttonOpenScan
            // 
            this.buttonOpenScan.Location = new System.Drawing.Point(421, 70);
            this.buttonOpenScan.Name = "buttonOpenScan";
            this.buttonOpenScan.Size = new System.Drawing.Size(128, 30);
            this.buttonOpenScan.TabIndex = 2;
            this.buttonOpenScan.Text = "Open / Scan";
            this.buttonOpenScan.UseVisualStyleBackColor = true;
            this.buttonOpenScan.Click += new System.EventHandler(this.buttonOpenScan_Click);
            // 
            // buttonClearCounter
            // 
            this.buttonClearCounter.Location = new System.Drawing.Point(421, 142);
            this.buttonClearCounter.Name = "buttonClearCounter";
            this.buttonClearCounter.Size = new System.Drawing.Size(128, 30);
            this.buttonClearCounter.TabIndex = 5;
            this.buttonClearCounter.Text = "Clear Counter";
            this.buttonClearCounter.UseVisualStyleBackColor = true;
            this.buttonClearCounter.Click += new System.EventHandler(this.buttonClearCounter_Click);
            // 
            // buttonClearCompareValue
            // 
            this.buttonClearCompareValue.Location = new System.Drawing.Point(421, 216);
            this.buttonClearCompareValue.Name = "buttonClearCompareValue";
            this.buttonClearCompareValue.Size = new System.Drawing.Size(128, 30);
            this.buttonClearCompareValue.TabIndex = 6;
            this.buttonClearCompareValue.Text = "Clear Compare";
            this.buttonClearCompareValue.UseVisualStyleBackColor = true;
            this.buttonClearCompareValue.Click += new System.EventHandler(this.buttonClearCompareValue_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(421, 444);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(128, 34);
            this.buttonClose.TabIndex = 8;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // labelCardId
            // 
            this.labelCardId.AutoSize = true;
            this.labelCardId.Location = new System.Drawing.Point(31, 47);
            this.labelCardId.Name = "labelCardId";
            this.labelCardId.Size = new System.Drawing.Size(53, 16);
            this.labelCardId.TabIndex = 0;
            this.labelCardId.Text = "Card ID";
            // 
            // labelCounter
            // 
            this.labelCounter.AutoSize = true;
            this.labelCounter.Location = new System.Drawing.Point(31, 117);
            this.labelCounter.Name = "labelCounter";
            this.labelCounter.Size = new System.Drawing.Size(53, 16);
            this.labelCounter.TabIndex = 2;
            this.labelCounter.Text = "Counter";
            // 
            // labelCompareValue
            // 
            this.labelCompareValue.AutoSize = true;
            this.labelCompareValue.Location = new System.Drawing.Point(31, 191);
            this.labelCompareValue.Name = "labelCompareValue";
            this.labelCompareValue.Size = new System.Drawing.Size(99, 16);
            this.labelCompareValue.TabIndex = 11;
            this.labelCompareValue.Text = "Compare Value";
            // 
            // labelMultipleRate
            // 
            this.labelMultipleRate.AutoSize = true;
            this.labelMultipleRate.Location = new System.Drawing.Point(31, 276);
            this.labelMultipleRate.Name = "labelMultipleRate";
            this.labelMultipleRate.Size = new System.Drawing.Size(86, 16);
            this.labelMultipleRate.TabIndex = 9;
            this.labelMultipleRate.Text = "Multiple Rate";
            // 
            // labelAutoIncrement
            // 
            this.labelAutoIncrement.AutoSize = true;
            this.labelAutoIncrement.Location = new System.Drawing.Point(31, 341);
            this.labelAutoIncrement.Name = "labelAutoIncrement";
            this.labelAutoIncrement.Size = new System.Drawing.Size(103, 16);
            this.labelAutoIncrement.TabIndex = 12;
            this.labelAutoIncrement.Text = "Auto Increment";
            // 
            // labelCmpOutWidth
            // 
            this.labelCmpOutWidth.AutoSize = true;
            this.labelCmpOutWidth.Location = new System.Drawing.Point(31, 406);
            this.labelCmpOutWidth.Name = "labelCmpOutWidth";
            this.labelCmpOutWidth.Size = new System.Drawing.Size(97, 16);
            this.labelCmpOutWidth.TabIndex = 15;
            this.labelCmpOutWidth.Text = "CMP OUT Width";
            // 
            // labelStatus
            // 
            this.labelStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelStatus.Location = new System.Drawing.Point(34, 471);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Padding = new System.Windows.Forms.Padding(8, 5, 8, 5);
            this.labelStatus.Size = new System.Drawing.Size(368, 72);
            this.labelStatus.TabIndex = 7;
            // 
            // comboBoxMultipleRate
            // 
            this.comboBoxMultipleRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMultipleRate.FormattingEnabled = true;
            this.comboBoxMultipleRate.Location = new System.Drawing.Point(34, 298);
            this.comboBoxMultipleRate.Name = "comboBoxMultipleRate";
            this.comboBoxMultipleRate.Size = new System.Drawing.Size(180, 24);
            this.comboBoxMultipleRate.TabIndex = 10;
            // 
            // numericAutoIncrement
            // 
            this.numericAutoIncrement.Location = new System.Drawing.Point(34, 363);
            this.numericAutoIncrement.Maximum = new decimal(new int[] { 2147483647, 0, 0, 0 });
            this.numericAutoIncrement.Minimum = new decimal(new int[] { -2147483648, 0, 0, -2147483648 });
            this.numericAutoIncrement.Name = "numericAutoIncrement";
            this.numericAutoIncrement.Size = new System.Drawing.Size(180, 23);
            this.numericAutoIncrement.TabIndex = 13;
            // 
            // numericCmpOutWidth
            // 
            this.numericCmpOutWidth.Location = new System.Drawing.Point(34, 428);
            this.numericCmpOutWidth.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            this.numericCmpOutWidth.Name = "numericCmpOutWidth";
            this.numericCmpOutWidth.Size = new System.Drawing.Size(180, 23);
            this.numericCmpOutWidth.TabIndex = 16;
            // 
            // buttonApplySettings
            // 
            this.buttonApplySettings.Location = new System.Drawing.Point(229, 423);
            this.buttonApplySettings.Name = "buttonApplySettings";
            this.buttonApplySettings.Size = new System.Drawing.Size(173, 30);
            this.buttonApplySettings.TabIndex = 14;
            this.buttonApplySettings.Text = "Apply / Save Settings";
            this.buttonApplySettings.UseVisualStyleBackColor = true;
            this.buttonApplySettings.Click += new System.EventHandler(this.buttonApplySettings_Click);
            // 
            // timerCounterRefresh
            // 
            this.timerCounterRefresh.Interval = 100;
            this.timerCounterRefresh.Tick += new System.EventHandler(this.timerCounterRefresh_Tick);
            // 
            // MeterWheelSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 567);
            this.Controls.Add(this.buttonApplySettings);
            this.Controls.Add(this.numericCmpOutWidth);
            this.Controls.Add(this.labelCmpOutWidth);
            this.Controls.Add(this.numericAutoIncrement);
            this.Controls.Add(this.labelAutoIncrement);
            this.Controls.Add(this.comboBoxMultipleRate);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.labelMultipleRate);
            this.Controls.Add(this.textBoxCompareValue);
            this.Controls.Add(this.labelCompareValue);
            this.Controls.Add(this.labelCounter);
            this.Controls.Add(this.labelCardId);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonClearCompareValue);
            this.Controls.Add(this.buttonClearCounter);
            this.Controls.Add(this.buttonOpenScan);
            this.Controls.Add(this.textBoxCounter);
            this.Controls.Add(this.comboBoxCardId);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MeterWheelSettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "LSI-8181 Meter Wheel Control";
            ((System.ComponentModel.ISupportInitialize)(this.numericCmpOutWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericAutoIncrement)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}

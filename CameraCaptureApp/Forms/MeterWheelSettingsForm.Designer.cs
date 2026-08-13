namespace CameraCaptureApp.Forms
{
    partial class MeterWheelSettingsForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ComboBox comboBoxCardId;
        private System.Windows.Forms.TextBox textBoxCounter;
        private System.Windows.Forms.Button buttonOpenScan;
        private System.Windows.Forms.Button buttonReadCounter;
        private System.Windows.Forms.Button buttonClearCounter;
        private System.Windows.Forms.Button buttonCloseCard;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Label labelCardId;
        private System.Windows.Forms.Label labelCounter;
        private System.Windows.Forms.Label labelMultipleRate;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.ComboBox comboBoxMultipleRate;
        private System.Windows.Forms.Button buttonApplyMultipleRate;
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
            this.buttonOpenScan = new System.Windows.Forms.Button();
            this.buttonReadCounter = new System.Windows.Forms.Button();
            this.buttonClearCounter = new System.Windows.Forms.Button();
            this.buttonCloseCard = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.labelCardId = new System.Windows.Forms.Label();
            this.labelCounter = new System.Windows.Forms.Label();
            this.labelMultipleRate = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.comboBoxMultipleRate = new System.Windows.Forms.ComboBox();
            this.buttonApplyMultipleRate = new System.Windows.Forms.Button();
            this.timerCounterRefresh = new System.Windows.Forms.Timer(this.components);
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
            // buttonReadCounter
            // 
            this.buttonReadCounter.Location = new System.Drawing.Point(421, 142);
            this.buttonReadCounter.Name = "buttonReadCounter";
            this.buttonReadCounter.Size = new System.Drawing.Size(128, 30);
            this.buttonReadCounter.TabIndex = 4;
            this.buttonReadCounter.Text = "Read Counter";
            this.buttonReadCounter.UseVisualStyleBackColor = true;
            this.buttonReadCounter.Click += new System.EventHandler(this.buttonReadCounter_Click);
            // 
            // buttonClearCounter
            // 
            this.buttonClearCounter.Location = new System.Drawing.Point(421, 178);
            this.buttonClearCounter.Name = "buttonClearCounter";
            this.buttonClearCounter.Size = new System.Drawing.Size(128, 30);
            this.buttonClearCounter.TabIndex = 5;
            this.buttonClearCounter.Text = "Clear Counter";
            this.buttonClearCounter.UseVisualStyleBackColor = true;
            this.buttonClearCounter.Click += new System.EventHandler(this.buttonClearCounter_Click);
            // 
            // buttonCloseCard
            // 
            this.buttonCloseCard.Location = new System.Drawing.Point(421, 214);
            this.buttonCloseCard.Name = "buttonCloseCard";
            this.buttonCloseCard.Size = new System.Drawing.Size(128, 30);
            this.buttonCloseCard.TabIndex = 6;
            this.buttonCloseCard.Text = "Close Card";
            this.buttonCloseCard.UseVisualStyleBackColor = true;
            this.buttonCloseCard.Click += new System.EventHandler(this.buttonCloseCard_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(421, 303);
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
            // labelMultipleRate
            // 
            this.labelMultipleRate.AutoSize = true;
            this.labelMultipleRate.Location = new System.Drawing.Point(31, 200);
            this.labelMultipleRate.Name = "labelMultipleRate";
            this.labelMultipleRate.Size = new System.Drawing.Size(86, 16);
            this.labelMultipleRate.TabIndex = 9;
            this.labelMultipleRate.Text = "Multiple Rate";
            // 
            // labelStatus
            // 
            this.labelStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelStatus.Location = new System.Drawing.Point(34, 265);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Padding = new System.Windows.Forms.Padding(8, 5, 8, 5);
            this.labelStatus.Size = new System.Drawing.Size(368, 72);
            this.labelStatus.TabIndex = 7;
            // 
            // comboBoxMultipleRate
            // 
            this.comboBoxMultipleRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMultipleRate.FormattingEnabled = true;
            this.comboBoxMultipleRate.Location = new System.Drawing.Point(34, 222);
            this.comboBoxMultipleRate.Name = "comboBoxMultipleRate";
            this.comboBoxMultipleRate.Size = new System.Drawing.Size(180, 24);
            this.comboBoxMultipleRate.TabIndex = 10;
            // 
            // buttonApplyMultipleRate
            // 
            this.buttonApplyMultipleRate.Location = new System.Drawing.Point(229, 220);
            this.buttonApplyMultipleRate.Name = "buttonApplyMultipleRate";
            this.buttonApplyMultipleRate.Size = new System.Drawing.Size(173, 30);
            this.buttonApplyMultipleRate.TabIndex = 11;
            this.buttonApplyMultipleRate.Text = "Apply Multiple Rate";
            this.buttonApplyMultipleRate.UseVisualStyleBackColor = true;
            this.buttonApplyMultipleRate.Click += new System.EventHandler(this.buttonApplyMultipleRate_Click);
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
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.buttonApplyMultipleRate);
            this.Controls.Add(this.comboBoxMultipleRate);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.labelMultipleRate);
            this.Controls.Add(this.labelCounter);
            this.Controls.Add(this.labelCardId);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonCloseCard);
            this.Controls.Add(this.buttonClearCounter);
            this.Controls.Add(this.buttonReadCounter);
            this.Controls.Add(this.buttonOpenScan);
            this.Controls.Add(this.textBoxCounter);
            this.Controls.Add(this.comboBoxCardId);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MeterWheelSettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "LSI-8181 Meter Wheel Control";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}

using System;
using System.Windows.Forms;
using CameraCaptureApp.Models;

namespace CameraCaptureApp.Forms
{
    public partial class CameraSettingsForm : Form
    {
        public CameraSettingsForm(CameraSettings settings)
        {
            Settings = settings.Clone();
            InitializeComponent();
            BindSettings();
        }

        public CameraSettings Settings { get; private set; }

        private void BindSettings()
        {
            comboBoxCameraName.Items.Clear();
            comboBoxCameraName.Items.AddRange(new object[]
            {
                "Default Camera",
                "Line Scan Camera 01",
                "Area Camera 01"
            });

            comboBoxPixelFormat.Items.Clear();
            comboBoxPixelFormat.Items.AddRange(new object[]
            {
                "Mono8",
                "Mono16",
                "RGB24"
            });

            comboBoxTriggerMode.Items.Clear();
            comboBoxTriggerMode.Items.AddRange(new object[]
            {
                "連續",
                "單張",
                "軟體觸發",
                "外部觸發"
            });

            comboBoxCameraName.Text = Settings.CameraName;
            textBoxConfigFile.Text = Settings.ConfigFilePath;
            numericWidth.Value = Settings.Width;
            numericHeight.Value = Settings.Height;
            numericExposure.Value = Settings.ExposureTime;
            numericGain.Value = Settings.Gain;
            numericFrameRate.Value = Settings.FrameRate;
            comboBoxPixelFormat.Text = Settings.PixelFormat;
            comboBoxTriggerMode.SelectedIndex = (int)Settings.TriggerMode;
            checkBoxAutoConnect.Checked = Settings.AutoConnect;
            checkBoxAutoSave.Checked = Settings.AutoSave;
            textBoxSaveFolder.Text = Settings.SaveFolder;
            textBoxFileNamePattern.Text = Settings.FileNamePattern;
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            SaveSettings();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonTestConnection_Click(object sender, EventArgs e)
        {
            SaveSettings();
            labelDiagnosticConnectionValue.Text = "待實機測試";
            labelDiagnosticSignalValue.Text = "待實機測試";
            labelDiagnosticResolutionValue.Text = Settings.Width + " x " + Settings.Height;
            labelDiagnosticMessageValue.Text = "Sapera SDK 連線流程已整合到主畫面的連線按鈕，請由主畫面進行實機測試。";
        }

        private void SaveSettings()
        {
            Settings.CameraName = comboBoxCameraName.Text.Trim();
            Settings.ConfigFilePath = textBoxConfigFile.Text.Trim();
            Settings.Width = (int)numericWidth.Value;
            Settings.Height = (int)numericHeight.Value;
            Settings.ExposureTime = numericExposure.Value;
            Settings.Gain = numericGain.Value;
            Settings.FrameRate = numericFrameRate.Value;
            Settings.PixelFormat = comboBoxPixelFormat.Text.Trim();
            Settings.TriggerMode = (TriggerMode)comboBoxTriggerMode.SelectedIndex;
            Settings.AutoConnect = checkBoxAutoConnect.Checked;
            Settings.AutoSave = checkBoxAutoSave.Checked;
            Settings.SaveFolder = textBoxSaveFolder.Text.Trim();
            Settings.FileNamePattern = textBoxFileNamePattern.Text.Trim();
            labelDiagnosticResolutionValue.Text = Settings.Width + " x " + Settings.Height;
        }
    }
}

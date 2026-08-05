using System;
using System.Windows.Forms;
using CameraCaptureApp.Models;
using CameraCaptureApp.Services;

namespace CameraCaptureApp.Forms
{
    public partial class CameraSettingsForm : Form
    {
        private readonly ICameraService _cameraService;

        public CameraSettingsForm(CameraSettings settings, ICameraService cameraService)
        {
            _cameraService = cameraService;
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
                "Continuous",
                "Single Frame",
                "Software Trigger",
                "External Trigger"
            });

            comboBoxCameraName.Text = Settings.CameraName;
            textBoxConfigFile.Text = Settings.ConfigFilePath;
            textBoxServerName.Text = Settings.ServerName;
            textBoxServerIndex.Text = Settings.ServerIndex >= 0 ? Settings.ServerIndex.ToString() : string.Empty;
            textBoxResourceIndex.Text = Settings.ResourceIndex.ToString();
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
            labelDiagnosticResolutionValue.Text = Settings.Width + " x " + Settings.Height;
        }

        private void buttonBrowseSapera_Click(object sender, EventArgs e)
        {
            SaveSettings();
            _cameraService.ApplySettings(Settings);

            if (!_cameraService.SelectConnectionSettings(this))
            {
                return;
            }

            Settings = _cameraService.CurrentSettings;
            textBoxConfigFile.Text = Settings.ConfigFilePath;
            textBoxServerName.Text = Settings.ServerName;
            textBoxServerIndex.Text = Settings.ServerIndex >= 0 ? Settings.ServerIndex.ToString() : string.Empty;
            textBoxResourceIndex.Text = Settings.ResourceIndex.ToString();
            comboBoxCameraName.Text = Settings.CameraName;
            labelDiagnosticConnectionValue.Text = "Saved";
            labelDiagnosticMessageValue.Text = "Sapera acquisition settings were loaded from the official dialog.";
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
            labelDiagnosticConnectionValue.Text = string.IsNullOrWhiteSpace(Settings.ConfigFilePath) ? "Missing" : "Ready";
            labelDiagnosticSignalValue.Text = string.IsNullOrWhiteSpace(Settings.ServerName) && Settings.ServerIndex < 0 ? "Missing" : "Saved";
            labelDiagnosticResolutionValue.Text = Settings.Width + " x " + Settings.Height;
            labelDiagnosticMessageValue.Text = string.IsNullOrWhiteSpace(Settings.ConfigFilePath)
                ? "Please select the Sapera acquisition configuration first."
                : "Saved connection settings will be used first. If the next connect fails, the official acquisition dialog will appear again.";
        }

        private void SaveSettings()
        {
            Settings.CameraName = comboBoxCameraName.Text.Trim();
            Settings.ConfigFilePath = textBoxConfigFile.Text.Trim();
            Settings.ServerName = textBoxServerName.Text.Trim();
            Settings.ServerIndex = ParseInt(textBoxServerIndex.Text, Settings.ServerIndex);
            Settings.ResourceIndex = ParseInt(textBoxResourceIndex.Text, Settings.ResourceIndex);
            Settings.Width = (int)numericWidth.Value;
            Settings.Height = (int)numericHeight.Value;
            Settings.ExposureTime = numericExposure.Value;
            Settings.Gain = numericGain.Value;
            Settings.FrameRate = numericFrameRate.Value;
            Settings.PixelFormat = comboBoxPixelFormat.Text.Trim();
            Settings.TriggerMode = (TriggerMode)Math.Max(0, comboBoxTriggerMode.SelectedIndex);
            Settings.AutoConnect = checkBoxAutoConnect.Checked;
            Settings.AutoSave = checkBoxAutoSave.Checked;
            Settings.SaveFolder = textBoxSaveFolder.Text.Trim();
            Settings.FileNamePattern = textBoxFileNamePattern.Text.Trim();
            labelDiagnosticResolutionValue.Text = Settings.Width + " x " + Settings.Height;
        }

        private static int ParseInt(string text, int fallback)
        {
            int parsed;
            return int.TryParse(text, out parsed) ? parsed : fallback;
        }
    }
}

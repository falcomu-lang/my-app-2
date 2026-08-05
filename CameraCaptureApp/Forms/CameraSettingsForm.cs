using System;
using System.Windows.Forms;
using CameraCaptureApp.Models;
using CameraCaptureApp.Services;
using DALSA.SaperaLT.SapClassBasic;

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
            comboBoxTriggerMode.Items.Clear();
            comboBoxTriggerMode.Items.AddRange(new object[]
            {
                "Continuous",
                "Single Frame",
                "Software Trigger",
                "External Trigger"
            });

            textBoxCameraName.Text = Settings.CameraName;
            textBoxConfigFile.Text = Settings.ConfigFilePath;
            textBoxServerName.Text = Settings.ServerName;
            textBoxServerIndex.Text = Settings.ServerIndex >= 0 ? Settings.ServerIndex.ToString() : string.Empty;
            textBoxResourceIndex.Text = Settings.ResourceIndex.ToString();
            numericExposure.Value = Settings.ExposureTime;
            numericGain.Value = Settings.Gain;
            numericLength.Value = Settings.Length > 0 ? Settings.Length : 1;
            numericInternalLineRate.Value = Settings.InternalLineRate > 0 ? Settings.InternalLineRate : 1;
            comboBoxTriggerMode.SelectedIndex = (int)Settings.TriggerMode;
            checkBoxAutoConnect.Checked = Settings.AutoConnect;
            checkBoxAutoSave.Checked = Settings.AutoSave;
            textBoxSaveFolder.Text = Settings.SaveFolder;
            textBoxFileNamePattern.Text = Settings.FileNamePattern;
            labelReadResult.Text = "Load Sapera settings first, then read supported CCF values into the fields.";
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
            textBoxCameraName.Text = Settings.CameraName;
            labelReadResult.Text = "Sapera acquisition settings loaded.";
        }

        private void buttonReadCcfToFields_Click(object sender, EventArgs e)
        {
            SaveSettings();

            if (string.IsNullOrWhiteSpace(Settings.ConfigFilePath))
            {
                labelReadResult.Text = "Please choose a CCF file first.";
                return;
            }

            if (string.IsNullOrWhiteSpace(Settings.ServerName) && Settings.ServerIndex < 0)
            {
                labelReadResult.Text = "Please load Sapera acquisition settings first.";
                return;
            }

            try
            {
                using (var acquisition = CreatePreviewAcquisition())
                {
                    if (!acquisition.Create())
                    {
                        labelReadResult.Text = "Could not create a temporary Sapera acquisition object.";
                        return;
                    }

                    comboBoxTriggerMode.SelectedIndex = (int)ReadTriggerMode(acquisition);

                    acquisition.Destroy();
                    labelReadResult.Text = "Mapped available CCF trigger values to supported fields.";
                }
            }
            catch (Exception ex)
            {
                labelReadResult.Text = "Read CCF failed: " + ex.Message;
            }
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

        private void SaveSettings()
        {
            Settings.CameraName = textBoxCameraName.Text.Trim();
            Settings.ConfigFilePath = textBoxConfigFile.Text.Trim();
            Settings.ServerName = textBoxServerName.Text.Trim();
            Settings.ServerIndex = ParseInt(textBoxServerIndex.Text, Settings.ServerIndex);
            Settings.ResourceIndex = ParseInt(textBoxResourceIndex.Text, Settings.ResourceIndex);
            Settings.ExposureTime = numericExposure.Value;
            Settings.Gain = numericGain.Value;
            Settings.Length = decimal.ToInt32(numericLength.Value);
            Settings.InternalLineRate = numericInternalLineRate.Value;
            Settings.TriggerMode = (TriggerMode)Math.Max(0, comboBoxTriggerMode.SelectedIndex);
            Settings.AutoConnect = checkBoxAutoConnect.Checked;
            Settings.AutoSave = checkBoxAutoSave.Checked;
            Settings.SaveFolder = textBoxSaveFolder.Text.Trim();
            Settings.FileNamePattern = textBoxFileNamePattern.Text.Trim();
        }

        private SapAcquisition CreatePreviewAcquisition()
        {
            SapLocation location;
            if (!string.IsNullOrWhiteSpace(Settings.ServerName))
            {
                location = new SapLocation(Settings.ServerName, Settings.ResourceIndex);
            }
            else
            {
                location = new SapLocation(Settings.ServerIndex, Settings.ResourceIndex);
            }

            return new SapAcquisition(location, Settings.ConfigFilePath);
        }

        private static TriggerMode ReadTriggerMode(SapAcquisition acquisition)
        {
            int enabled;

            if (acquisition.GetParameter(SapAcquisition.Prm.EXT_LINE_TRIGGER_ENABLE, out enabled) && enabled != 0)
            {
                return TriggerMode.ExternalTrigger;
            }

            if (acquisition.GetParameter(SapAcquisition.Prm.EXT_FRAME_TRIGGER_ENABLE, out enabled) && enabled != 0)
            {
                return TriggerMode.ExternalTrigger;
            }

            if (acquisition.GetParameter(SapAcquisition.Prm.EXT_TRIGGER_ENABLE, out enabled) && enabled != 0)
            {
                return TriggerMode.ExternalTrigger;
            }

            if (acquisition.GetParameter(SapAcquisition.Prm.CAM_TRIGGER_ENABLE, out enabled) && enabled != 0)
            {
                return TriggerMode.SoftwareTrigger;
            }

            if (acquisition.GetParameter(SapAcquisition.Prm.LINE_TRIGGER_ENABLE, out enabled) && enabled != 0)
            {
                return TriggerMode.SoftwareTrigger;
            }

            return TriggerMode.Continuous;
        }

        private static int ParseInt(string text, int fallback)
        {
            int parsed;
            return int.TryParse(text, out parsed) ? parsed : fallback;
        }
    }
}

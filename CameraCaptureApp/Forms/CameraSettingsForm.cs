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
                "RGB24",
                "Unknown"
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
            numericExposure.Value = Settings.ExposureTime;
            numericGain.Value = Settings.Gain;
            numericFrameRate.Value = Settings.FrameRate;
            comboBoxPixelFormat.Text = string.IsNullOrWhiteSpace(Settings.PixelFormat) ? "Unknown" : Settings.PixelFormat;
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
            comboBoxCameraName.Text = Settings.CameraName;
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

                    comboBoxPixelFormat.Text = ReadPixelFormat(acquisition);
                    numericFrameRate.Value = ClampDecimal(ReadFrameRate(acquisition), numericFrameRate.Minimum, numericFrameRate.Maximum, numericFrameRate.Value);
                    comboBoxTriggerMode.SelectedIndex = (int)ReadTriggerMode(acquisition);

                    acquisition.Destroy();
                    labelReadResult.Text = "Mapped available CCF values to supported fields.";
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
            Settings.CameraName = comboBoxCameraName.Text.Trim();
            Settings.ConfigFilePath = textBoxConfigFile.Text.Trim();
            Settings.ServerName = textBoxServerName.Text.Trim();
            Settings.ServerIndex = ParseInt(textBoxServerIndex.Text, Settings.ServerIndex);
            Settings.ResourceIndex = ParseInt(textBoxResourceIndex.Text, Settings.ResourceIndex);
            Settings.ExposureTime = numericExposure.Value;
            Settings.Gain = numericGain.Value;
            Settings.FrameRate = numericFrameRate.Value;
            Settings.PixelFormat = comboBoxPixelFormat.Text.Trim();
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

        private static string ReadPixelFormat(SapAcquisition acquisition)
        {
            int pixelDepth;
            if (!acquisition.GetParameter(SapAcquisition.Prm.PIXEL_DEPTH, out pixelDepth))
            {
                return "Unknown";
            }

            if (pixelDepth <= 8)
            {
                return "Mono8";
            }

            if (pixelDepth <= 16)
            {
                return "Mono16";
            }

            if (pixelDepth <= 24)
            {
                return "RGB24";
            }

            return "Unknown";
        }

        private static decimal ReadFrameRate(SapAcquisition acquisition)
        {
            long frequency;
            if (acquisition.GetParameter(SapAcquisition.Prm.INT_LINE_TRIGGER_FREQ, out frequency))
            {
                return (decimal)frequency;
            }

            return 30m;
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

        private static decimal ClampDecimal(decimal value, decimal minimum, decimal maximum, decimal fallback)
        {
            if (value < minimum || value > maximum)
            {
                return fallback;
            }

            return value;
        }

        private static int ParseInt(string text, int fallback)
        {
            int parsed;
            return int.TryParse(text, out parsed) ? parsed : fallback;
        }
    }
}

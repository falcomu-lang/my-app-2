using System;
using System.Windows.Forms;
using CameraCaptureApp.Native;
using CameraCaptureApp.Services;

namespace CameraCaptureApp.Forms
{
    public partial class MeterWheelControlForm : Form
    {
        private readonly Lsi8181MeterWheelService _meterWheelService = new Lsi8181MeterWheelService();

        public MeterWheelControlForm()
        {
            InitializeComponent();
            comboCardId.SelectedIndex = 0;
            comboMultipleRate.SelectedIndex = 0;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            timerRefresh.Stop();
            timerRefresh.Dispose();
            _meterWheelService.Dispose();
            base.OnFormClosed(e);
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (_meterWheelService.IsInitialized)
                {
                    timerRefresh.Stop();
                    _meterWheelService.Close();
                    labelStatus.Text = "Offline";
                }
                else
                {
                    _meterWheelService.Open((byte)comboCardId.SelectedIndex);
                    timerRefresh.Start();
                    labelStatus.Text = "Connected";
                }

                UpdateControlStates();
                RefreshValues();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void buttonClearEncoder_Click(object sender, EventArgs e)
        {
            SetEncoderValue(0);
        }

        private void buttonSetEncoder_Click(object sender, EventArgs e)
        {
            SetEncoderValue((int)numericEncoder.Value);
        }

        private void buttonClearCompare_Click(object sender, EventArgs e)
        {
            SetCompareValue(0);
        }

        private void buttonSetCompare_Click(object sender, EventArgs e)
        {
            SetCompareValue((int)numericCompare.Value);
        }

        private void buttonApplyIncrement_Click(object sender, EventArgs e)
        {
            SetCompareIncrement((int)numericIncrement.Value);
        }

        private void buttonSetMultipleRate_Click(object sender, EventArgs e)
        {
            SetMultipleRate(GetSelectedMultipleRate());
        }

        private void buttonSetCmpOutWidth_Click(object sender, EventArgs e)
        {
            SetCmpOutWidth((ushort)numericCmpOutWidth.Value);
        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            RefreshValues();
        }

        private void RefreshValues()
        {
            if (!_meterWheelService.IsInitialized)
            {
                labelEncoderValue.Text = "0";
                labelCompareValue.Text = "0";
                return;
            }

            try
            {
                labelEncoderValue.Text = _meterWheelService.ReadEncoder().ToString();
                labelCompareValue.Text = _meterWheelService.ReadCompare().ToString();
            }
            catch (Exception ex)
            {
                timerRefresh.Stop();
                ShowError(ex);
            }
        }

        private void SetEncoderValue(int value)
        {
            try
            {
                _meterWheelService.SetEncoder(value);
                RefreshValues();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void SetCompareValue(int value)
        {
            try
            {
                _meterWheelService.SetCompare(value);
                RefreshValues();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void UpdateControlStates()
        {
            var isConnected = _meterWheelService.IsInitialized;
            comboCardId.Enabled = !isConnected;
            buttonConnect.Text = isConnected ? "Disconnect" : "Connect";
            buttonClearEncoder.Enabled = isConnected;
            buttonSetEncoder.Enabled = isConnected;
            buttonClearCompare.Enabled = isConnected;
            buttonSetCompare.Enabled = isConnected;
            buttonApplyIncrement.Enabled = isConnected;
            buttonSetMultipleRate.Enabled = isConnected;
            buttonSetCmpOutWidth.Enabled = isConnected;
        }

        private void SetCompareIncrement(int value)
        {
            try
            {
                _meterWheelService.SetCompareIncrement(value);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void SetMultipleRate(byte multipleRate)
        {
            try
            {
                _meterWheelService.SetMultipleRate(multipleRate);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void SetCmpOutWidth(ushort outWidth)
        {
            try
            {
                _meterWheelService.SetCmpOutWidth(outWidth);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private byte GetSelectedMultipleRate()
        {
            switch (comboMultipleRate.SelectedIndex)
            {
                case 1:
                    return Lsi8181Native.Multiple2;
                case 2:
                    return Lsi8181Native.Multiple1;
                default:
                    return Lsi8181Native.Multiple4;
            }
        }

        private void ShowError(Exception ex)
        {
            AppLogger.Log("Meter wheel control failed.", ex);
            labelStatus.Text = "Error";
            UpdateControlStates();
            MessageBox.Show(
                this,
                "Meter wheel operation failed.\r\n" + ex.Message + "\r\n\r\nMake sure LSI8181_64.dll and the driver are installed.",
                "Meter Wheel Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}

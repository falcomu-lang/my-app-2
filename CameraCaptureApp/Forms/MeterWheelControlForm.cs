using System;
using System.Drawing;
using System.Windows.Forms;
using CameraCaptureApp.Native;
using CameraCaptureApp.Services;

namespace CameraCaptureApp.Forms
{
    public sealed class MeterWheelControlForm : Form
    {
        private readonly Lsi8181MeterWheelService _meterWheelService = new Lsi8181MeterWheelService();
        private readonly Timer _refreshTimer = new Timer();
        private ComboBox comboCardId;
        private Label labelEncoderValue;
        private Label labelCompareValue;
        private NumericUpDown numericEncoder;
        private NumericUpDown numericCompare;
        private Button buttonConnect;
        private Button buttonClearEncoder;
        private Button buttonSetEncoder;
        private Button buttonClearCompare;
        private Button buttonSetCompare;
        private Label labelStatus;

        public MeterWheelControlForm()
        {
            InitializeComponent();

            _refreshTimer.Interval = 200;
            _refreshTimer.Tick += RefreshTimer_Tick;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _refreshTimer.Stop();
            _refreshTimer.Dispose();
            _meterWheelService.Dispose();
            base.OnFormClosed(e);
        }

        private void InitializeComponent()
        {
            Text = "Meter Wheel Control";
            StartPosition = FormStartPosition.CenterParent;
            MinimumSize = new Size(620, 390);
            Size = new Size(680, 430);
            BackColor = Color.FromArgb(18, 23, 34);
            Font = new Font("Microsoft JhengHei UI", 10F);

            var labelCard = CreateLabel("Meter wheel card", 24, 26, 150, 28, true);
            comboCardId = new ComboBox();
            comboCardId.DropDownStyle = ComboBoxStyle.DropDownList;
            comboCardId.Location = new Point(180, 24);
            comboCardId.Size = new Size(130, 28);
            for (var i = 0; i <= Lsi8181Native.CardIdMax; i++)
            {
                comboCardId.Items.Add(i.ToString());
            }

            comboCardId.SelectedIndex = 0;

            buttonConnect = CreateButton("Connect", 330, 20, 130, 36);
            buttonConnect.Click += ButtonConnect_Click;

            var encoderTitle = CreateLabel("Encoder", 24, 92, 150, 30, true);
            labelEncoderValue = CreateValueLabel("0", 24, 130, 270, 46);
            buttonClearEncoder = CreateButton("Clear", 320, 130, 110, 46);
            buttonClearEncoder.Click += ButtonClearEncoder_Click;
            numericEncoder = CreateNumeric(24, 194, 270, 32);
            buttonSetEncoder = CreateButton("Set Encoder", 320, 190, 150, 40);
            buttonSetEncoder.Click += ButtonSetEncoder_Click;

            var compareTitle = CreateLabel("Compare", 24, 252, 150, 30, true);
            labelCompareValue = CreateValueLabel("0", 24, 290, 270, 46);
            buttonClearCompare = CreateButton("Clear", 320, 290, 110, 46);
            buttonClearCompare.Click += ButtonClearCompare_Click;
            numericCompare = CreateNumeric(24, 354, 270, 32);
            buttonSetCompare = CreateButton("Set Compare", 320, 350, 150, 40);
            buttonSetCompare.Click += ButtonSetCompare_Click;

            labelStatus = CreateLabel("Offline", 480, 27, 160, 28, false);
            labelStatus.ForeColor = Color.FromArgb(210, 220, 240);

            Controls.Add(labelCard);
            Controls.Add(comboCardId);
            Controls.Add(buttonConnect);
            Controls.Add(labelStatus);
            Controls.Add(encoderTitle);
            Controls.Add(labelEncoderValue);
            Controls.Add(buttonClearEncoder);
            Controls.Add(numericEncoder);
            Controls.Add(buttonSetEncoder);
            Controls.Add(compareTitle);
            Controls.Add(labelCompareValue);
            Controls.Add(buttonClearCompare);
            Controls.Add(numericCompare);
            Controls.Add(buttonSetCompare);

            UpdateControlStates();
        }

        private void ButtonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (_meterWheelService.IsInitialized)
                {
                    _refreshTimer.Stop();
                    _meterWheelService.Close();
                    labelStatus.Text = "Offline";
                }
                else
                {
                    _meterWheelService.Open((byte)comboCardId.SelectedIndex);
                    _refreshTimer.Start();
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

        private void ButtonClearEncoder_Click(object sender, EventArgs e)
        {
            SetEncoderValue(0);
        }

        private void ButtonSetEncoder_Click(object sender, EventArgs e)
        {
            SetEncoderValue((int)numericEncoder.Value);
        }

        private void ButtonClearCompare_Click(object sender, EventArgs e)
        {
            SetCompareValue(0);
        }

        private void ButtonSetCompare_Click(object sender, EventArgs e)
        {
            SetCompareValue((int)numericCompare.Value);
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
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
                _refreshTimer.Stop();
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

        private static Label CreateLabel(string text, int x, int y, int width, int height, bool bold)
        {
            return new Label
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, height),
                ForeColor = Color.White,
                Font = new Font("Microsoft JhengHei UI", 10F, bold ? FontStyle.Bold : FontStyle.Regular)
            };
        }

        private static Label CreateValueLabel(string text, int x, int y, int width, int height)
        {
            return new Label
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, height),
                BackColor = Color.FromArgb(8, 12, 20),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Consolas", 18F, FontStyle.Bold)
            };
        }

        private static NumericUpDown CreateNumeric(int x, int y, int width, int height)
        {
            return new NumericUpDown
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                Minimum = int.MinValue,
                Maximum = int.MaxValue,
                DecimalPlaces = 0,
                ThousandsSeparator = true
            };
        }

        private static Button CreateButton(string text, int x, int y, int width, int height)
        {
            var button = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, height),
                BackColor = Color.FromArgb(84, 120, 196),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft JhengHei UI", 10F)
            };
            button.FlatAppearance.BorderColor = Color.FromArgb(126, 158, 226);
            return button;
        }
    }
}

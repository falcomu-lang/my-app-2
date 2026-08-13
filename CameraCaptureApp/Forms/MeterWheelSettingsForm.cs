using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CameraCaptureApp.Models;
using CameraCaptureApp.Services;

namespace CameraCaptureApp.Forms
{
    public partial class MeterWheelSettingsForm : Form
    {
        private readonly ILsi8181Service _lsi8181Service;
        private readonly ISettingsService _settingsService;
        private CameraSettings _settings;
        private IReadOnlyList<Lsi8181CardInfo> _cards;
        private bool _counterRefreshInProgress;

        public MeterWheelSettingsForm(ILsi8181Service lsi8181Service, ISettingsService settingsService)
        {
            _lsi8181Service = lsi8181Service;
            _settingsService = settingsService;
            _settings = _settingsService.Load() ?? CameraSettings.CreateDefault();
            InitializeComponent();
            BindMultipleRateOptions();
            SelectMultipleRate((byte)Clamp(_settings.Lsi8181MultipleRate, 0, 2));
            numericAutoIncrement.Value = ClampToNumericRange(numericAutoIncrement, _settings.Lsi8181AutoIncrement);
            labelStatus.Text = "Click Open / Scan to find LSI-8181 cards.";
        }

        private void buttonOpenScan_Click(object sender, EventArgs e)
        {
            try
            {
                comboBoxCardId.Items.Clear();
                _cards = _lsi8181Service.InitializeAndScanCards();
                foreach (var card in _cards)
                {
                    comboBoxCardId.Items.Add(card);
                }

                if (comboBoxCardId.Items.Count > 0)
                {
                    comboBoxCardId.SelectedIndex = FindCardIndex(_settings.Lsi8181CardId);
                    ReadCounterForSelectedCard();
                    ReadCompareValueForSelectedCard();
                    ReadMultipleRateForSelectedCard();
                    ReadAutoIncrementForSelectedCard();
                    timerCounterRefresh.Start();
                }
                else
                {
                    timerCounterRefresh.Stop();
                }

                labelStatus.Text = _lsi8181Service.LastMessage;
            }
            catch (Exception ex)
            {
                ShowError("Open / Scan failed", ex);
            }
        }

        private void buttonReadCounter_Click(object sender, EventArgs e)
        {
            ReadCounterForSelectedCard();
        }

        private void buttonClearCounter_Click(object sender, EventArgs e)
        {
            try
            {
                var card = GetSelectedCard();
                _lsi8181Service.ClearCounter(card.CardId);
                textBoxCounter.Text = _lsi8181Service.ReadCounter(card.CardId).ToString();
                labelStatus.Text = "Counter cleared.";
            }
            catch (Exception ex)
            {
                ShowError("Clear counter failed", ex);
            }
        }

        private void buttonCloseCard_Click(object sender, EventArgs e)
        {
            try
            {
                _lsi8181Service.Close();
                timerCounterRefresh.Stop();
                labelStatus.Text = _lsi8181Service.LastMessage;
            }
            catch (Exception ex)
            {
                ShowError("Close card failed", ex);
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonApplyMultipleRate_Click(object sender, EventArgs e)
        {
            try
            {
                var card = GetSelectedCard();
                var option = comboBoxMultipleRate.SelectedItem as MultipleRateOption;
                if (option == null)
                {
                    throw new InvalidOperationException("Please select a multiple rate first.");
                }

                _lsi8181Service.SetMultipleRate(card.CardId, option.Value);
                SaveMeterWheelSettings(card.CardId, option.Value, decimal.ToInt32(numericAutoIncrement.Value));
                labelStatus.Text = "Multiple rate set to " + option.Text + ".";
            }
            catch (Exception ex)
            {
                ShowError("Apply multiple rate failed", ex);
            }
        }

        private void buttonApplyAutoIncrement_Click(object sender, EventArgs e)
        {
            try
            {
                var card = GetSelectedCard();
                var incrementValue = decimal.ToInt32(numericAutoIncrement.Value);
                _lsi8181Service.ApplyAutoIncrementMode(card.CardId, incrementValue);
                var option = comboBoxMultipleRate.SelectedItem as MultipleRateOption;
                SaveMeterWheelSettings(card.CardId, option != null ? option.Value : _settings.Lsi8181MultipleRate, incrementValue);
                labelStatus.Text = "Auto increment compare mode enabled. Increment: " + incrementValue + ".";
            }
            catch (Exception ex)
            {
                ShowError("Apply auto increment failed", ex);
            }
        }

        private void timerCounterRefresh_Tick(object sender, EventArgs e)
        {
            if (_counterRefreshInProgress || comboBoxCardId.SelectedItem == null || !_lsi8181Service.IsInitialized)
            {
                return;
            }

            _counterRefreshInProgress = true;
            try
            {
                var card = GetSelectedCard();
                var counter = _lsi8181Service.ReadCounter(card.CardId).ToString();
                if (!string.Equals(textBoxCounter.Text, counter, StringComparison.Ordinal))
                {
                    textBoxCounter.Text = counter;
                }

                var compareValue = _lsi8181Service.ReadCompareValue(card.CardId).ToString();
                if (!string.Equals(textBoxCompareValue.Text, compareValue, StringComparison.Ordinal))
                {
                    textBoxCompareValue.Text = compareValue;
                }
            }
            catch (Exception ex)
            {
                timerCounterRefresh.Stop();
                ShowError("Auto counter refresh failed", ex);
            }
            finally
            {
                _counterRefreshInProgress = false;
            }
        }

        private void ReadCounterForSelectedCard()
        {
            try
            {
                var card = GetSelectedCard();
                textBoxCounter.Text = _lsi8181Service.ReadCounter(card.CardId).ToString();
                labelStatus.Text = _lsi8181Service.LastMessage;
            }
            catch (Exception ex)
            {
                ShowError("Read counter failed", ex);
            }
        }

        private void ReadCompareValueForSelectedCard()
        {
            try
            {
                var card = GetSelectedCard();
                textBoxCompareValue.Text = _lsi8181Service.ReadCompareValue(card.CardId).ToString();
            }
            catch (Exception ex)
            {
                ShowError("Read compare value failed", ex);
            }
        }

        private void ReadMultipleRateForSelectedCard()
        {
            try
            {
                var card = GetSelectedCard();
                var inputMode = _lsi8181Service.ReadCounterInputMode(card.CardId);
                SelectMultipleRate(inputMode.MultipleRate);
            }
            catch (Exception ex)
            {
                ShowError("Read multiple rate failed", ex);
            }
        }

        private void ReadAutoIncrementForSelectedCard()
        {
            try
            {
                var card = GetSelectedCard();
                var incrementValue = _lsi8181Service.ReadAutoIncrement(card.CardId);
                numericAutoIncrement.Value = ClampToNumericRange(numericAutoIncrement, incrementValue);
            }
            catch (Exception ex)
            {
                ShowError("Read auto increment failed", ex);
            }
        }

        private static decimal ClampToNumericRange(NumericUpDown control, decimal value)
        {
            if (value < control.Minimum)
            {
                return control.Minimum;
            }

            if (value > control.Maximum)
            {
                return control.Maximum;
            }

            return value;
        }

        private int FindCardIndex(int savedCardId)
        {
            for (var index = 0; index < comboBoxCardId.Items.Count; index++)
            {
                var card = comboBoxCardId.Items[index] as Lsi8181CardInfo;
                if (card != null && card.CardId == savedCardId)
                {
                    return index;
                }
            }

            return 0;
        }

        private void SaveMeterWheelSettings(byte cardId, int multipleRate, int autoIncrement)
        {
            _settings.Lsi8181CardId = cardId;
            _settings.Lsi8181MultipleRate = multipleRate;
            _settings.Lsi8181AutoIncrement = autoIncrement;
            _settingsService.Save(_settings);
        }

        private static int Clamp(int value, int minimum, int maximum)
        {
            if (value < minimum)
            {
                return minimum;
            }

            if (value > maximum)
            {
                return maximum;
            }

            return value;
        }

        private void BindMultipleRateOptions()
        {
            comboBoxMultipleRate.Items.Clear();
            comboBoxMultipleRate.Items.Add(new MultipleRateOption("x4", 0));
            comboBoxMultipleRate.Items.Add(new MultipleRateOption("x2", 1));
            comboBoxMultipleRate.Items.Add(new MultipleRateOption("x1", 2));
            comboBoxMultipleRate.SelectedIndex = 0;
        }

        private void SelectMultipleRate(byte multipleRate)
        {
            for (var index = 0; index < comboBoxMultipleRate.Items.Count; index++)
            {
                var option = comboBoxMultipleRate.Items[index] as MultipleRateOption;
                if (option != null && option.Value == multipleRate)
                {
                    comboBoxMultipleRate.SelectedIndex = index;
                    return;
                }
            }

            comboBoxMultipleRate.SelectedIndex = 0;
        }

        private Lsi8181CardInfo GetSelectedCard()
        {
            var card = comboBoxCardId.SelectedItem as Lsi8181CardInfo;
            if (card == null)
            {
                throw new InvalidOperationException("Please click Open / Scan and select a Card ID first.");
            }

            return card;
        }

        private void ShowError(string title, Exception ex)
        {
            AppLogger.Log(title, ex);
            labelStatus.Text = title + ": " + ex.Message;
            MessageBox.Show(this, ex.Message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private sealed class MultipleRateOption
        {
            public MultipleRateOption(string text, byte value)
            {
                Text = text;
                Value = value;
            }

            public string Text { get; private set; }

            public byte Value { get; private set; }

            public override string ToString()
            {
                return Text;
            }
        }
    }
}

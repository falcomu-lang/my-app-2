using System;
using System.Collections.Generic;
using System.Threading;
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
        private bool _resourceUnavailableWarningShown;

        public MeterWheelSettingsForm(ILsi8181Service lsi8181Service, ISettingsService settingsService)
        {
            _lsi8181Service = lsi8181Service;
            _settingsService = settingsService;
            _settings = _settingsService.Load() ?? CameraSettings.CreateDefault();
            InitializeComponent();
            BindMultipleRateOptions();
            SelectMultipleRate((byte)Clamp(_settings.Lsi8181MultipleRate, 0, 2));
            numericAutoIncrement.Value = ClampToNumericRange(numericAutoIncrement, _settings.Lsi8181AutoIncrement);
            numericCmpOutWidth.Value = ClampToNumericRange(numericCmpOutWidth, _settings.Lsi8181CmpOutWidth);
            labelStatus.Text = "Click Open / Scan to find LSI-8181 cards.";
            PopulateCards(_lsi8181Service.LastCards);
        }

        private void buttonOpenScan_Click(object sender, EventArgs e)
        {
            try
            {
                PopulateCards(_lsi8181Service.InitializeAndScanCards());
                labelStatus.Text = _lsi8181Service.LastMessage;
            }
            catch (Exception ex)
            {
                ShowError("Open / Scan failed", ex);
            }
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
                HandleLsi8181ResourceFailure("Clear counter failed", ex);
            }
        }

        private void buttonClearCompareValue_Click(object sender, EventArgs e)
        {
            try
            {
                var card = GetSelectedCard();
                _lsi8181Service.ClearCompareValue(card.CardId);
                textBoxCompareValue.Text = _lsi8181Service.ReadCompareValue(card.CardId).ToString();
                labelStatus.Text = "Compare value cleared.";
            }
            catch (Exception ex)
            {
                HandleLsi8181ResourceFailure("Clear compare value failed", ex);
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonApplySettings_Click(object sender, EventArgs e)
        {
            try
            {
                var card = GetSelectedCard();
                var option = comboBoxMultipleRate.SelectedItem as MultipleRateOption;
                if (option == null)
                {
                    throw new InvalidOperationException("Please select a multiple rate first.");
                }

                var incrementValue = decimal.ToInt32(numericAutoIncrement.Value);
                var cmpOutWidth = decimal.ToUInt16(numericCmpOutWidth.Value);
                _lsi8181Service.SetMultipleRate(card.CardId, option.Value);
                _lsi8181Service.ApplyAutoIncrementMode(card.CardId, incrementValue, cmpOutWidth);
                SaveMeterWheelSettings(
                    card.CardId,
                    option.Value,
                    incrementValue,
                    cmpOutWidth);
                labelStatus.Text = "Settings applied and saved. Multiple rate: " + option.Text + ", increment: " + incrementValue + ", CMP OUT width: " + cmpOutWidth + ".";
            }
            catch (Exception ex)
            {
                HandleLsi8181ResourceFailure("Apply settings failed", ex);
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
                HandleLsi8181ResourceFailure("Auto counter refresh failed", ex);
            }
            finally
            {
                _counterRefreshInProgress = false;
            }
        }

        private void PopulateCards(IReadOnlyList<Lsi8181CardInfo> cards)
        {
            comboBoxCardId.Items.Clear();
            _cards = cards ?? new List<Lsi8181CardInfo>();
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
                labelStatus.Text = _lsi8181Service.LastMessage;
            }
            else
            {
                timerCounterRefresh.Stop();
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
                HandleLsi8181ResourceFailure("Read counter failed", ex);
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
                HandleLsi8181ResourceFailure("Read compare value failed", ex);
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
                HandleLsi8181ResourceFailure("Read multiple rate failed", ex);
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
                HandleLsi8181ResourceFailure("Read auto increment failed", ex);
            }
        }

        private void HandleLsi8181ResourceFailure(string title, Exception originalException)
        {
            AppLogger.Log(title, originalException);
            labelStatus.Text = title + ". Reconnecting LSI-8181...";

            if (TryReconnectLsi8181())
            {
                _resourceUnavailableWarningShown = false;
                labelStatus.Text = "LSI-8181 reconnected.";
                return;
            }

            if (_resourceUnavailableWarningShown)
            {
                labelStatus.Text = "LSI-8181 resource unavailable.";
                return;
            }

            _resourceUnavailableWarningShown = true;
            ShowError("LSI-8181 Resource Unavailable", new InvalidOperationException("Could not access the LSI-8181 resource after 3 reconnect attempts.", originalException));
        }

        private bool TryReconnectLsi8181()
        {
            timerCounterRefresh.Stop();
            for (var attempt = 1; attempt <= 3; attempt++)
            {
                try
                {
                    _lsi8181Service.Close();
                    Thread.Sleep(150);
                    var cards = _lsi8181Service.InitializeAndScanCards();
                    if (cards.Count == 0)
                    {
                        continue;
                    }

                    PopulateCards(cards);
                    return true;
                }
                catch (Exception ex)
                {
                    AppLogger.Log("LSI-8181 reconnect attempt " + attempt + " failed.", ex);
                }
            }

            timerCounterRefresh.Stop();
            return false;
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

        private void SaveMeterWheelSettings(byte cardId, int multipleRate, int autoIncrement, int cmpOutWidth)
        {
            _settings.Lsi8181CardId = cardId;
            _settings.Lsi8181MultipleRate = multipleRate;
            _settings.Lsi8181AutoIncrement = autoIncrement;
            _settings.Lsi8181CmpOutWidth = cmpOutWidth;
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

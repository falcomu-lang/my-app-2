using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CameraCaptureApp.Services;

namespace CameraCaptureApp.Forms
{
    public partial class MeterWheelSettingsForm : Form
    {
        private readonly ILsi8181Service _lsi8181Service;
        private IReadOnlyList<Lsi8181CardInfo> _cards;

        public MeterWheelSettingsForm(ILsi8181Service lsi8181Service)
        {
            _lsi8181Service = lsi8181Service;
            InitializeComponent();
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
                    comboBoxCardId.SelectedIndex = 0;
                    ReadCounterForSelectedCard();
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
    }
}

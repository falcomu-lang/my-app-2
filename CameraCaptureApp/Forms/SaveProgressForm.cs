using System;
using System.Windows.Forms;

namespace CameraCaptureApp.Forms
{
    public sealed class SaveProgressForm : Form
    {
        private readonly ProgressBar progressBar;
        private readonly Label labelStatus;

        public SaveProgressForm()
        {
            Text = "Saving Image";
            Width = 420;
            Height = 130;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;

            labelStatus = new Label();
            labelStatus.AutoEllipsis = true;
            labelStatus.Left = 16;
            labelStatus.Top = 16;
            labelStatus.Width = 370;
            labelStatus.Height = 22;
            labelStatus.Text = "Preparing image...";

            progressBar = new ProgressBar();
            progressBar.Left = 16;
            progressBar.Top = 48;
            progressBar.Width = 370;
            progressBar.Height = 24;
            progressBar.Minimum = 0;
            progressBar.Maximum = 100;
            progressBar.Value = 0;

            Controls.Add(labelStatus);
            Controls.Add(progressBar);
        }

        public void Report(int percent, string statusText)
        {
            if (IsDisposed)
            {
                return;
            }

            if (InvokeRequired)
            {
                try
                {
                    BeginInvoke(new Action<int, string>(Report), percent, statusText);
                }
                catch (InvalidOperationException)
                {
                }

                return;
            }

            progressBar.Value = Math.Max(progressBar.Minimum, Math.Min(progressBar.Maximum, percent));
            labelStatus.Text = statusText ?? string.Empty;
        }
    }
}

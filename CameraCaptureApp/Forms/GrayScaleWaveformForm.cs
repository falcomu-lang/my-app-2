using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using CameraCaptureApp.Services;

namespace CameraCaptureApp.Forms
{
    public partial class GrayScaleWaveformForm : Form
    {
        private readonly IGrayPixelSource _pixelSource;
        private readonly Point[] _linePoints;
        private readonly int[] _grayValues;
        private readonly int _minGray;
        private readonly int _maxGray;

        public GrayScaleWaveformForm(IGrayPixelSource pixelSource, Point[] linePoints)
        {
            _pixelSource = pixelSource;
            _linePoints = linePoints ?? new Point[0];
            InitializeComponent();
            if (_pixelSource == null || _linePoints.Length == 0)
            {
                buttonClose.Text = "關閉";
                labelInfo.Text = "No waveform data.";
                return;
            }

            _grayValues = SampleGrayValues(_pixelSource, _linePoints);
            _minGray = _grayValues.Min();
            _maxGray = _grayValues.Max();
            labelInfo.Text = "Points: " + _grayValues.Length + " | Gray range: " + _minGray + " - " + _maxGray;
            panelChart.Invalidate();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _pixelSource?.Dispose();

            base.OnFormClosed(e);
        }

        private void panelChart_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.FromArgb(20, 24, 35));
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            if (_grayValues == null || _grayValues.Length == 0)
            {
                using (var brush = new SolidBrush(Color.Gainsboro))
                {
                    e.Graphics.DrawString("No waveform data.", Font, brush, 16, 16);
                }

                return;
            }

            var bounds = panelChart.ClientRectangle;
            bounds.Inflate(-28, -28);
            if (bounds.Width <= 0 || bounds.Height <= 0)
            {
                return;
            }

            DrawAxes(e.Graphics, bounds);
            DrawWaveform(e.Graphics, bounds);
        }

        private void DrawAxes(Graphics graphics, Rectangle bounds)
        {
            using (var pen = new Pen(Color.FromArgb(90, 100, 120), 1f))
            {
                graphics.DrawRectangle(pen, bounds);
                graphics.DrawLine(pen, bounds.Left, bounds.Bottom, bounds.Right, bounds.Bottom);
                graphics.DrawLine(pen, bounds.Left, bounds.Top, bounds.Left, bounds.Bottom);
            }
        }

        private void DrawWaveform(Graphics graphics, Rectangle bounds)
        {
            if (_grayValues.Length < 2)
            {
                return;
            }

            var points = new List<PointF>(_grayValues.Length);
            for (var i = 0; i < _grayValues.Length; i++)
            {
                var x = bounds.Left + (i * (bounds.Width - 1f) / Math.Max(1, _grayValues.Length - 1));
                var y = bounds.Bottom - ((_grayValues[i] / 255f) * bounds.Height);
                points.Add(new PointF(x, y));
            }

            using (var pen = new Pen(Color.FromArgb(90, 200, 255), 2f))
            {
                if (points.Count >= 2)
                {
                    graphics.DrawLines(pen, points.ToArray());
                }
            }

            using (var brush = new SolidBrush(Color.Gainsboro))
            {
                graphics.DrawString("X: line position", Font, brush, bounds.Left, bounds.Top - 22);
                graphics.DrawString("Y: gray value", Font, brush, bounds.Right - 120, bounds.Top - 22);
            }
        }

        private static int[] SampleGrayValues(IGrayPixelSource pixelSource, Point[] linePoints)
        {
            var result = new int[linePoints.Length];
            for (var i = 0; i < linePoints.Length; i++)
            {
                result[i] = pixelSource.GetGrayAt(linePoints[i].X, linePoints[i].Y);
            }

            return result;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

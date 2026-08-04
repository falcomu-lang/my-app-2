using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CameraCaptureApp.Controls
{
    public partial class CameraDisplayControl : UserControl
    {
        private readonly object _imageLock = new object();
        private Bitmap _sourceBitmap;
        private bool _isPanning;
        private Point _lastMousePoint;
        private int _imageVersion;
        private DateTime _lastDisplayUpdateUtc;
        private float _zoom = 1f;
        private PointF _imageOffset = PointF.Empty;

        public CameraDisplayControl()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            StatusText = "尚未載入圖片";
        }

        public string OverlayText
        {
            get { return overlayLabel.Text; }
            set { overlayLabel.Text = value; }
        }

        public string ResolutionText
        {
            get { return resolutionLabel.Text; }
            set { resolutionLabel.Text = value; }
        }

        public string StatusText
        {
            get { return statusLabel.Text; }
            set { statusLabel.Text = value; }
        }

        public async Task LoadImageFromFileAsync(string filePath, CancellationToken cancellationToken)
        {
            var version = Interlocked.Increment(ref _imageVersion);
            StatusText = "載入圖片中...";

            var bitmap = await Task.Run(
                () =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    using (var source = Image.FromFile(filePath))
                    {
                        return new Bitmap(source);
                    }
                },
                cancellationToken);

            if (cancellationToken.IsCancellationRequested || version != _imageVersion)
            {
                bitmap.Dispose();
                return;
            }

            await ApplyImageAsync(bitmap, version, cancellationToken);
        }

        public async Task ShowFrameAsync(Bitmap frame, CancellationToken cancellationToken)
        {
            var version = Interlocked.Increment(ref _imageVersion);
            await ApplyImageAsync(new Bitmap(frame), version, cancellationToken);
        }

        private async Task ApplyImageAsync(Bitmap bitmap, int version, CancellationToken cancellationToken)
        {
            var elapsed = DateTime.UtcNow - _lastDisplayUpdateUtc;
            if (elapsed.TotalMilliseconds < 200)
            {
                await Task.Delay(200 - (int)elapsed.TotalMilliseconds, cancellationToken);
            }

            if (cancellationToken.IsCancellationRequested || version != _imageVersion)
            {
                bitmap.Dispose();
                return;
            }

            lock (_imageLock)
            {
                if (_sourceBitmap != null)
                {
                    _sourceBitmap.Dispose();
                }

                _sourceBitmap = bitmap;
            }

            _lastDisplayUpdateUtc = DateTime.UtcNow;
            OverlayText = "拖曳平移，滾輪縮放";
            ResolutionText = bitmap.Width + " x " + bitmap.Height;
            FitImageToView();
            UpdateStatusLabel();
            viewerPanel.Invalidate();
            viewerPanel.Update();
        }

        private void viewerPanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Black);
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.SmoothingMode = SmoothingMode.HighSpeed;

            Bitmap bitmap;
            float zoom;
            PointF offset;
            lock (_imageLock)
            {
                bitmap = _sourceBitmap;
                zoom = _zoom;
                offset = _imageOffset;
            }

            if (bitmap == null || zoom <= 0f)
            {
                return;
            }

            var drawWidth = bitmap.Width * zoom;
            var drawHeight = bitmap.Height * zoom;
            e.Graphics.DrawImage(bitmap, offset.X, offset.Y, drawWidth, drawHeight);
        }

        private void viewerPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            Bitmap bitmap;
            float oldZoom;
            PointF oldOffset;
            lock (_imageLock)
            {
                bitmap = _sourceBitmap;
                oldZoom = _zoom;
                oldOffset = _imageOffset;
            }

            if (bitmap == null)
            {
                return;
            }

            var zoomFactor = e.Delta > 0 ? 1.25f : 0.8f;
            var newZoom = ClampZoom(oldZoom * zoomFactor);
            if (Math.Abs(newZoom - oldZoom) < 0.0001f)
            {
                return;
            }

            var imageX = (e.X - oldOffset.X) / oldZoom;
            var imageY = (e.Y - oldOffset.Y) / oldZoom;

            lock (_imageLock)
            {
                _zoom = newZoom;
                _imageOffset = new PointF(
                    e.X - (imageX * newZoom),
                    e.Y - (imageY * newZoom));
            }

            UpdateStatusLabel();
            viewerPanel.Invalidate();
        }

        private void viewerPanel_MouseDown(object sender, MouseEventArgs e)
        {
            lock (_imageLock)
            {
                if (e.Button != MouseButtons.Left || _sourceBitmap == null)
                {
                    return;
                }
            }

            _isPanning = true;
            _lastMousePoint = e.Location;
            viewerPanel.Cursor = Cursors.Hand;
        }

        private void viewerPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isPanning)
            {
                return;
            }

            var deltaX = e.X - _lastMousePoint.X;
            var deltaY = e.Y - _lastMousePoint.Y;
            _lastMousePoint = e.Location;

            lock (_imageLock)
            {
                _imageOffset = new PointF(_imageOffset.X + deltaX, _imageOffset.Y + deltaY);
            }

            UpdateStatusLabel();
            viewerPanel.Invalidate();
        }

        private void viewerPanel_MouseUp(object sender, MouseEventArgs e)
        {
            _isPanning = false;
            viewerPanel.Cursor = Cursors.Default;
        }

        private void viewerPanel_MouseEnter(object sender, EventArgs e)
        {
            viewerPanel.Focus();
        }

        private void buttonFitToWindow_Click(object sender, EventArgs e)
        {
            FitImageToView();
            UpdateStatusLabel();
            viewerPanel.Invalidate();
        }

        private void CameraDisplayControl_SizeChanged(object sender, EventArgs e)
        {
            if (!_isPanning)
            {
                UpdateStatusLabel();
            }

            viewerPanel.Invalidate();
        }

        private void FitImageToView()
        {
            lock (_imageLock)
            {
                if (_sourceBitmap == null)
                {
                    _zoom = 1f;
                    _imageOffset = PointF.Empty;
                    return;
                }

                var bounds = GetDestinationRectangle();
                if (bounds.Width <= 0 || bounds.Height <= 0)
                {
                    _zoom = 1f;
                    _imageOffset = PointF.Empty;
                    return;
                }

                var scaleX = bounds.Width / (float)_sourceBitmap.Width;
                var scaleY = bounds.Height / (float)_sourceBitmap.Height;
                _zoom = Math.Min(scaleX, scaleY);

                var drawWidth = _sourceBitmap.Width * _zoom;
                var drawHeight = _sourceBitmap.Height * _zoom;
                _imageOffset = new PointF(
                    bounds.X + ((bounds.Width - drawWidth) / 2f),
                    bounds.Y + ((bounds.Height - drawHeight) / 2f));
            }
        }

        private void UpdateStatusLabel()
        {
            lock (_imageLock)
            {
                if (_sourceBitmap == null)
                {
                    StatusText = "尚未載入圖片";
                    return;
                }

                var imageX = (-_imageOffset.X) / _zoom;
                var imageY = (-_imageOffset.Y) / _zoom;
                StatusText = string.Format(
                    "縮放 {0:0.00}x | 偏移 {1:0},{2:0} | 影像座標 {3:0},{4:0}",
                    _zoom,
                    _imageOffset.X,
                    _imageOffset.Y,
                    imageX,
                    imageY);
            }
        }

        private Rectangle GetDestinationRectangle()
        {
            var bounds = viewerPanel.ClientRectangle;
            bounds.Inflate(-8, -8);
            return bounds;
        }

        private static float ClampZoom(float zoom)
        {
            if (zoom < 0.02f)
            {
                return 0.02f;
            }

            if (zoom > 50f)
            {
                return 50f;
            }

            return zoom;
        }
    }
}

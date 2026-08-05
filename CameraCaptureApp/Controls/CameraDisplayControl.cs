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
        private const int TileSourceSize = 1024;
        private const float TileRenderZoomThreshold = 0.12f;
        private const float TilePreviewHandoffRatio = 1.02f;
        private const int TileRefreshIntervalMs = 33;

        private readonly object _imageLock = new object();
        private readonly System.Windows.Forms.Timer _tileRefreshTimer;
        private Bitmap _sourceBitmap;
        private LargeImageSource _largeImageSource;
        private bool _isPanning;
        private Point _lastMousePoint;
        private int _imageVersion;
        private DateTime _lastDisplayUpdateUtc;
        private bool _tileRefreshPending;
        private float _zoom = 1f;
        private PointF _imageOffset = PointF.Empty;

        public CameraDisplayControl()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            _tileRefreshTimer = new System.Windows.Forms.Timer();
            _tileRefreshTimer.Interval = TileRefreshIntervalMs;
            _tileRefreshTimer.Tick += TileRefreshTimer_Tick;
            StatusText = "No image loaded";
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
            StatusText = "Loading large image...";

            var largeImageSource = await Task.Run(
                () =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    return new LargeImageSource(filePath);
                },
                cancellationToken);

            if (cancellationToken.IsCancellationRequested || version != _imageVersion)
            {
                largeImageSource.Dispose();
                return;
            }

            await ApplyLargeImageAsync(largeImageSource, version, cancellationToken);
        }

        public async Task ShowFrameAsync(Bitmap frame, CancellationToken cancellationToken)
        {
            var version = Interlocked.Increment(ref _imageVersion);
            await ApplyBitmapAsync(new Bitmap(frame), version, cancellationToken);
        }

        private async Task ApplyLargeImageAsync(LargeImageSource source, int version, CancellationToken cancellationToken)
        {
            var elapsed = DateTime.UtcNow - _lastDisplayUpdateUtc;
            if (elapsed.TotalMilliseconds < 200)
            {
                await Task.Delay(200 - (int)elapsed.TotalMilliseconds, cancellationToken);
            }

            if (cancellationToken.IsCancellationRequested || version != _imageVersion)
            {
                source.Dispose();
                return;
            }

            lock (_imageLock)
            {
                DisposeCurrentImage();
                _largeImageSource = source;
            }

            _lastDisplayUpdateUtc = DateTime.UtcNow;
            OverlayText = "Large image view";
            ResolutionText = source.Width + " x " + source.Height;
            FitImageToView();
            UpdateStatusLabel();
            viewerPanel.Invalidate();
            viewerPanel.Update();
        }

        private async Task ApplyBitmapAsync(Bitmap bitmap, int version, CancellationToken cancellationToken)
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
                DisposeCurrentImage();
                _sourceBitmap = bitmap;
            }

            _lastDisplayUpdateUtc = DateTime.UtcNow;
            OverlayText = "Live frame view";
            ResolutionText = bitmap.Width + " x " + bitmap.Height;
            FitImageToView();
            UpdateStatusLabel();
            viewerPanel.Invalidate();
            viewerPanel.Update();
        }

        private void viewerPanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Black);
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.Default;
            e.Graphics.SmoothingMode = SmoothingMode.HighSpeed;

            Bitmap bitmap;
            LargeImageSource largeImageSource;
            float zoom;
            PointF offset;
            lock (_imageLock)
            {
                bitmap = _sourceBitmap;
                largeImageSource = _largeImageSource;
                zoom = _zoom;
                offset = _imageOffset;
            }

            if (zoom <= 0f)
            {
                return;
            }

            if (bitmap != null)
            {
                var drawWidth = bitmap.Width * zoom;
                var drawHeight = bitmap.Height * zoom;
                e.Graphics.DrawImage(bitmap, offset.X, offset.Y, drawWidth, drawHeight);
                return;
            }

            if (largeImageSource == null)
            {
                return;
            }

            DrawLargeImage(e.Graphics, largeImageSource, zoom, offset);
        }

        private void DrawLargeImage(Graphics graphics, LargeImageSource source, float zoom, PointF offset)
        {
            using (var preview = source.GetBestPreview(zoom))
            {
                var drawWidth = source.Width * zoom;
                var drawHeight = source.Height * zoom;
                var previousInterpolation = graphics.InterpolationMode;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(preview.Bitmap, offset.X, offset.Y, drawWidth, drawHeight);
                graphics.InterpolationMode = previousInterpolation;
                if (!ShouldRenderTiles(zoom, preview.Scale))
                {
                    return;
                }
            }

            var viewBounds = viewerPanel.ClientRectangle;
            var visibleSourceRect = GetVisibleSourceRectangle(source.Width, source.Height, viewBounds, zoom, offset);
            if (visibleSourceRect.Width <= 0 || visibleSourceRect.Height <= 0)
            {
                return;
            }

            var startTileX = (visibleSourceRect.Left / TileSourceSize) * TileSourceSize;
            var endTileX = ((visibleSourceRect.Right + TileSourceSize - 1) / TileSourceSize) * TileSourceSize;
            var startTileY = (visibleSourceRect.Top / TileSourceSize) * TileSourceSize;
            var endTileY = ((visibleSourceRect.Bottom + TileSourceSize - 1) / TileSourceSize) * TileSourceSize;

            for (var tileY = startTileY; tileY < endTileY; tileY += TileSourceSize)
            {
                for (var tileX = startTileX; tileX < endTileX; tileX += TileSourceSize)
                {
                    var tileRect = source.GetVisibleTileBounds(new Rectangle(tileX, tileY, TileSourceSize, TileSourceSize));
                    Bitmap tile;
                    if (source.TryGetTile(tileRect, out tile))
                    {
                        using (tile)
                        {
                            DrawTile(graphics, tile, tileRect, zoom, offset);
                        }
                    }
                    else
                    {
                        RequestTile(source, tileRect);
                    }
                }
            }
        }

        private void RequestTile(LargeImageSource source, Rectangle tileRect)
        {
            source.QueueTile(
                tileRect,
                ScheduleTileRefresh);
            source.PrefetchNeighborhood(
                tileRect,
                ScheduleTileRefresh);
        }

        private static void DrawTile(Graphics graphics, Bitmap tile, Rectangle tileRect, float zoom, PointF offset)
        {
            var left = offset.X + (tileRect.Left * zoom);
            var top = offset.Y + (tileRect.Top * zoom);
            var right = offset.X + (tileRect.Right * zoom);
            var bottom = offset.Y + (tileRect.Bottom * zoom);

            var drawLeft = (float)Math.Round(left);
            var drawTop = (float)Math.Round(top);
            var drawRight = (float)Math.Round(right);
            var drawBottom = (float)Math.Round(bottom);
            if (drawRight <= drawLeft)
            {
                drawRight = drawLeft + 1f;
            }

            if (drawBottom <= drawTop)
            {
                drawBottom = drawTop + 1f;
            }

            var drawRect = RectangleF.FromLTRB(drawLeft, drawTop, drawRight, drawBottom);
            var previousInterpolation = graphics.InterpolationMode;
            graphics.InterpolationMode = zoom >= 1f ? InterpolationMode.NearestNeighbor : InterpolationMode.HighQualityBilinear;
            graphics.DrawImage(tile, drawRect);
            graphics.InterpolationMode = previousInterpolation;
        }

        private static bool ShouldRenderTiles(float zoom, float previewScale)
        {
            if (zoom < TileRenderZoomThreshold)
            {
                return false;
            }

            return zoom > (previewScale * TilePreviewHandoffRatio);
        }

        private void ScheduleTileRefresh()
        {
            if (IsDisposed || !IsHandleCreated)
            {
                return;
            }

            try
            {
                BeginInvoke(
                    new Action(
                        () =>
                        {
                            if (IsDisposed)
                            {
                                return;
                            }

                            _tileRefreshPending = true;
                            if (!_tileRefreshTimer.Enabled)
                            {
                                _tileRefreshTimer.Start();
                            }
                        }));
            }
            catch (ObjectDisposedException)
            {
            }
            catch (InvalidOperationException)
            {
            }
        }

        private void TileRefreshTimer_Tick(object sender, EventArgs e)
        {
            _tileRefreshTimer.Stop();
            if (!_tileRefreshPending || IsDisposed)
            {
                return;
            }

            _tileRefreshPending = false;
            viewerPanel.Invalidate();
        }

        private void viewerPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            int width;
            int height;
            float oldZoom;
            PointF oldOffset;
            if (!TryGetSourceMetrics(out width, out height, out oldZoom, out oldOffset))
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
                if (e.Button != MouseButtons.Left || (_sourceBitmap == null && _largeImageSource == null))
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
                var sourceWidth = GetSourceWidthUnsafe();
                var sourceHeight = GetSourceHeightUnsafe();
                if (sourceWidth <= 0 || sourceHeight <= 0)
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

                var scaleX = bounds.Width / (float)sourceWidth;
                var scaleY = bounds.Height / (float)sourceHeight;
                _zoom = Math.Min(scaleX, scaleY);

                var drawWidth = sourceWidth * _zoom;
                var drawHeight = sourceHeight * _zoom;
                _imageOffset = new PointF(
                    bounds.X + ((bounds.Width - drawWidth) / 2f),
                    bounds.Y + ((bounds.Height - drawHeight) / 2f));
            }
        }

        private void UpdateStatusLabel()
        {
            lock (_imageLock)
            {
                var sourceWidth = GetSourceWidthUnsafe();
                var sourceHeight = GetSourceHeightUnsafe();
                if (sourceWidth <= 0 || sourceHeight <= 0)
                {
                    StatusText = "No image loaded";
                    return;
                }

                var imageX = (-_imageOffset.X) / _zoom;
                var imageY = (-_imageOffset.Y) / _zoom;
                StatusText = string.Format(
                    "Zoom {0:0.00}x | Offset {1:0},{2:0} | Image {3:0},{4:0}",
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

        private bool TryGetSourceMetrics(out int width, out int height, out float zoom, out PointF offset)
        {
            lock (_imageLock)
            {
                width = GetSourceWidthUnsafe();
                height = GetSourceHeightUnsafe();
                zoom = _zoom;
                offset = _imageOffset;
                return width > 0 && height > 0;
            }
        }

        private int GetSourceWidthUnsafe()
        {
            if (_sourceBitmap != null)
            {
                return _sourceBitmap.Width;
            }

            return _largeImageSource != null ? _largeImageSource.Width : 0;
        }

        private int GetSourceHeightUnsafe()
        {
            if (_sourceBitmap != null)
            {
                return _sourceBitmap.Height;
            }

            return _largeImageSource != null ? _largeImageSource.Height : 0;
        }

        private void DisposeCurrentImage()
        {
            if (_sourceBitmap != null)
            {
                _sourceBitmap.Dispose();
                _sourceBitmap = null;
            }

            if (_largeImageSource != null)
            {
                _largeImageSource.Dispose();
                _largeImageSource = null;
            }
        }

        private static Rectangle GetVisibleSourceRectangle(int sourceWidth, int sourceHeight, Rectangle viewBounds, float zoom, PointF offset)
        {
            var left = Math.Max(0, (int)Math.Floor((viewBounds.Left - offset.X) / zoom));
            var top = Math.Max(0, (int)Math.Floor((viewBounds.Top - offset.Y) / zoom));
            var right = Math.Min(sourceWidth, (int)Math.Ceiling((viewBounds.Right - offset.X) / zoom));
            var bottom = Math.Min(sourceHeight, (int)Math.Ceiling((viewBounds.Bottom - offset.Y) / zoom));
            return Rectangle.FromLTRB(left, top, Math.Max(left + 1, right), Math.Max(top + 1, bottom));
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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CameraCaptureApp.Models;
using CameraCaptureApp.Services;

namespace CameraCaptureApp.Controls
{
    public partial class CameraDisplayControl : UserControl
    {
        private const int TileSourceSize = 1024;
        private const float TileRenderZoomThreshold = 0.12f;
        private const float TilePreviewHandoffRatio = 1.02f;
        private const int TileRefreshIntervalMs = 33;
        private const int MaxLivePreviewDimension = 1600;

        private readonly object _imageLock = new object();
        private readonly System.Windows.Forms.Timer _tileRefreshTimer;
        private readonly List<Bitmap> _rollingPreviewFrames = new List<Bitmap>();
        private Bitmap _sourceBitmap;
        private LargeImageSource _largeImageSource;
        private int _rollingPreviewFrameWidth;
        private int _rollingPreviewFrameHeight;
        private int _rollingPreviewFrameCount;
        private RollingCaptureDirection _rollingPreviewDirection;
        private bool _isPanning;
        private Point _lastMousePoint;
        private int _imageVersion;
        private DateTime _lastDisplayUpdateUtc;
        private bool _tileRefreshPending;
        private float _zoom = 1f;
        private PointF _imageOffset = PointF.Empty;
        private bool _grayWaveformSelectionActive;
        private IGrayPixelSource _grayWaveformSelectionSource;
        private Point? _grayWaveformSelectionStart;
        private Point? _grayWaveformSelectionEnd;
        private bool _grayWaveformDragging;

        public event EventHandler SaveSnapshotRequested;

        public event EventHandler GrayWaveformRequested;

        public event EventHandler<GrayWaveformSelectionEventArgs> GrayWaveformSelectionCompleted;

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

        public bool SaveSnapshotButtonEnabled
        {
            get { return buttonSaveSnapshot.Enabled; }
            set { buttonSaveSnapshot.Enabled = value; }
        }

        public bool GrayWaveformButtonEnabled
        {
            get { return buttonGrayWaveform.Enabled; }
            set { buttonGrayWaveform.Enabled = value; }
        }

        public bool BeginGrayWaveformSelection()
        {
            return BeginGrayWaveformSelection(null);
        }

        public bool BeginGrayWaveformSelection(Bitmap cameraPixelSource)
        {
            if (_grayWaveformSelectionActive)
            {
                if (cameraPixelSource != null)
                {
                    cameraPixelSource.Dispose();
                }

                return false;
            }

            var snapshot = cameraPixelSource == null
                ? CreateCurrentGrayPixelSource()
                : new BitmapGrayPixelSource(cameraPixelSource);
            if (snapshot == null)
            {
                StatusText = "No image is available for waveform selection.";
                return false;
            }

            lock (_imageLock)
            {
                ClearGrayWaveformSelectionUnsafe();
                _grayWaveformSelectionSource = snapshot;
                _grayWaveformSelectionActive = true;
                _grayWaveformSelectionStart = null;
                _grayWaveformSelectionEnd = null;
                _grayWaveformDragging = false;
            }

            OverlayText = "Waveform selection mode";
            StatusText = "Drag a line on the image.";
            viewerPanel.Cursor = Cursors.Cross;
            viewerPanel.Invalidate();
            return true;
        }

        public bool IsDisplayingPreviewBitmap
        {
            get
            {
                lock (_imageLock)
                {
                    return _sourceBitmap != null;
                }
            }
        }

        private IGrayPixelSource CreateCurrentGrayPixelSource()
        {
            lock (_imageLock)
            {
                if (_sourceBitmap != null)
                {
                    return new BitmapGrayPixelSource((Bitmap)_sourceBitmap.Clone());
                }

                if (_largeImageSource != null)
                {
                    return new LargeImageGrayPixelSource(_largeImageSource);
                }
            }

            return null;
        }

        public void CancelGrayWaveformSelection()
        {
            lock (_imageLock)
            {
                ClearGrayWaveformSelectionUnsafe();
            }

            viewerPanel.Cursor = Cursors.Default;
            viewerPanel.Invalidate();
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

        public Task ShowFrameAsync(Bitmap frame, CancellationToken cancellationToken)
        {
            return ShowFrameAsync(frame, cancellationToken, false, 1, RollingCaptureDirection.TopToBottom);
        }

        public async Task ShowFrameAsync(Bitmap frame, CancellationToken cancellationToken, bool rollingPreviewEnabled, int rollingFrameCount, RollingCaptureDirection rollingDirection)
        {
            var version = Interlocked.Increment(ref _imageVersion);
            var sourceWidth = frame.Width;
            var sourceHeight = frame.Height;
            var previewFrame = await Task.Run(
                () =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    return CreateLivePreviewBitmap(frame);
                },
                cancellationToken);
            frame.Dispose();
            if (rollingPreviewEnabled)
            {
                if (rollingDirection == RollingCaptureDirection.TopToBottom)
                {
                    previewFrame.RotateFlip(RotateFlipType.RotateNoneFlipY);
                }

                await ApplyRollingBitmapAsync(previewFrame, version, sourceWidth, sourceHeight, rollingFrameCount, rollingDirection, cancellationToken);
                return;
            }

            await ApplyBitmapAsync(previewFrame, version, sourceWidth, sourceHeight, cancellationToken);
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
                if (_grayWaveformSelectionActive)
                {
                    source.Dispose();
                    return;
                }

                DisposeCurrentImage();
                DisposeRollingPreviewFrames();
                _largeImageSource = source;
            }

            _lastDisplayUpdateUtc = DateTime.UtcNow;
            OverlayText = "Large image view";
            ResolutionText = source.Width + " x " + source.Height;
            FitImageToView();
            UpdateStatusLabel();
            viewerPanel.Invalidate();
        }

        private async Task ApplyBitmapAsync(Bitmap bitmap, int version, int sourceWidth, int sourceHeight, CancellationToken cancellationToken)
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
                if (_grayWaveformSelectionActive)
                {
                    bitmap.Dispose();
                    return;
                }

                DisposeCurrentImage();
                DisposeRollingPreviewFrames();
                _sourceBitmap = bitmap;
            }

            _lastDisplayUpdateUtc = DateTime.UtcNow;
            OverlayText = "Live frame view";
            ResolutionText = sourceWidth + " x " + sourceHeight;
            FitImageToView();
            UpdateStatusLabel();
            viewerPanel.Invalidate();
        }

        private async Task ApplyRollingBitmapAsync(Bitmap bitmap, int version, int sourceWidth, int sourceHeight, int frameCount, RollingCaptureDirection direction, CancellationToken cancellationToken)
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

            frameCount = Math.Max(1, frameCount);
            bool shouldFitToView;
            lock (_imageLock)
            {
                if (_grayWaveformSelectionActive)
                {
                    bitmap.Dispose();
                    return;
                }

                shouldFitToView = _rollingPreviewFrames.Count == 0 ||
                    _rollingPreviewFrameWidth != bitmap.Width ||
                    _rollingPreviewFrameHeight != bitmap.Height ||
                    _rollingPreviewFrameCount != frameCount ||
                    _rollingPreviewDirection != direction;

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

                _rollingPreviewFrameWidth = bitmap.Width;
                _rollingPreviewFrameHeight = bitmap.Height;
                _rollingPreviewFrameCount = frameCount;
                _rollingPreviewDirection = direction;
                _rollingPreviewFrames.Insert(0, bitmap);
                while (_rollingPreviewFrames.Count > frameCount)
                {
                    var lastIndex = _rollingPreviewFrames.Count - 1;
                    _rollingPreviewFrames[lastIndex].Dispose();
                    _rollingPreviewFrames.RemoveAt(lastIndex);
                }
            }

            _lastDisplayUpdateUtc = DateTime.UtcNow;
            OverlayText = "Rolling live frame view";
            ResolutionText = sourceWidth + " x " + (sourceHeight * frameCount);
            if (shouldFitToView)
            {
                FitImageToView();
            }

            UpdateStatusLabel();
            viewerPanel.Invalidate();
        }

        private void viewerPanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Black);
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.Default;
            e.Graphics.SmoothingMode = SmoothingMode.HighSpeed;

            Bitmap bitmap;
            LargeImageSource largeImageSource;
            IGrayPixelSource waveformSource;
            float zoom;
            PointF offset;
            Point? selectionStart;
            Point? selectionEnd;
            bool selectionActive;
            lock (_imageLock)
            {
                bitmap = _sourceBitmap;
                largeImageSource = _largeImageSource;
                waveformSource = _grayWaveformSelectionSource;
                zoom = _zoom;
                offset = _imageOffset;
                selectionStart = _grayWaveformSelectionStart;
                selectionEnd = _grayWaveformSelectionEnd;
                selectionActive = _grayWaveformSelectionActive;
            }

            if (zoom <= 0f)
            {
                return;
            }

            if (bitmap != null)
            {
                var drawWidth = bitmap.Width * zoom;
                var drawHeight = bitmap.Height * zoom;
                e.Graphics.InterpolationMode = InterpolationMode.Low;
                e.Graphics.DrawImage(bitmap, offset.X, offset.Y, drawWidth, drawHeight);
            }

            if (_rollingPreviewFrames.Count > 0)
            {
                e.Graphics.InterpolationMode = InterpolationMode.Low;
                for (var index = 0; index < _rollingPreviewFrames.Count; index++)
                {
                    var rollingFrame = _rollingPreviewFrames[index];
                    var frameSlot = _rollingPreviewDirection == RollingCaptureDirection.BottomToTop
                        ? _rollingPreviewFrameCount - index - 1
                        : index;
                    var drawLeft = (float)Math.Round(offset.X);
                    var drawTop = (float)Math.Round(offset.Y + (_rollingPreviewFrameHeight * frameSlot * zoom));
                    var drawRight = (float)Math.Round(offset.X + (_rollingPreviewFrameWidth * zoom));
                    var drawBottom = (float)Math.Round(offset.Y + (_rollingPreviewFrameHeight * (frameSlot + 1) * zoom));
                    if (drawRight <= drawLeft)
                    {
                        drawRight = drawLeft + 1f;
                    }

                    if (drawBottom <= drawTop)
                    {
                        drawBottom = drawTop + 1f;
                    }

                    e.Graphics.DrawImage(rollingFrame, RectangleF.FromLTRB(drawLeft, drawTop, drawRight, drawBottom));
                }
            }

            if (largeImageSource != null)
            {
                DrawLargeImage(e.Graphics, largeImageSource, zoom, offset);
            }

            if (selectionActive && waveformSource != null)
            {
                DrawGrayWaveformSelectionOverlay(e.Graphics, selectionStart, selectionEnd);
            }
        }

        private void DrawGrayWaveformSelectionOverlay(Graphics graphics, Point? selectionStart, Point? selectionEnd)
        {
            if (!selectionStart.HasValue || !selectionEnd.HasValue)
            {
                return;
            }

            var imageBounds = GetCurrentDisplayImageBoundsUnsafe();
            if (imageBounds.Width <= 0 || imageBounds.Height <= 0)
            {
                return;
            }

            var p1 = ImageToClient(selectionStart.Value, imageBounds, GetCurrentImageSizeUnsafe());
            var p2 = ImageToClient(selectionEnd.Value, imageBounds, GetCurrentImageSizeUnsafe());
            using (var pen = new Pen(Color.Yellow, 2f))
            {
                graphics.DrawLine(pen, p1, p2);
            }
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
            if (TryStartGrayWaveformSelectionDrag(e))
            {
                return;
            }

            lock (_imageLock)
            {
                if (e.Button != MouseButtons.Left ||
                    (_sourceBitmap == null && _largeImageSource == null && _rollingPreviewFrames.Count == 0))
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
            if (TryUpdateGrayWaveformSelectionDrag(e))
            {
                return;
            }

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
            if (TryFinishGrayWaveformSelectionDrag(e))
            {
                return;
            }

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

        private void buttonSaveSnapshot_Click(object sender, EventArgs e)
        {
            var handler = SaveSnapshotRequested;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void buttonGrayWaveform_Click(object sender, EventArgs e)
        {
            var handler = GrayWaveformRequested;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
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

            if (_rollingPreviewFrames.Count > 0)
            {
                return _rollingPreviewFrameWidth;
            }

            return _largeImageSource != null ? _largeImageSource.Width : 0;
        }

        private int GetSourceHeightUnsafe()
        {
            if (_sourceBitmap != null)
            {
                return _sourceBitmap.Height;
            }

            if (_rollingPreviewFrames.Count > 0)
            {
                return _rollingPreviewFrameHeight * _rollingPreviewFrameCount;
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

            ClearGrayWaveformSelectionUnsafe();
        }

        private void DisposeRollingPreviewFrames()
        {
            foreach (var frame in _rollingPreviewFrames)
            {
                frame.Dispose();
            }

            _rollingPreviewFrames.Clear();
            _rollingPreviewFrameWidth = 0;
            _rollingPreviewFrameHeight = 0;
            _rollingPreviewFrameCount = 0;
            _rollingPreviewDirection = RollingCaptureDirection.TopToBottom;
        }

        private static Bitmap CreateLivePreviewBitmap(Bitmap source)
        {
            if (source == null)
            {
                return null;
            }

            var maxDimension = Math.Max(source.Width, source.Height);
            if (maxDimension <= MaxLivePreviewDimension)
            {
                return (Bitmap)source.Clone();
            }

            var scale = MaxLivePreviewDimension / (float)maxDimension;
            var previewWidth = Math.Max(1, (int)Math.Round(source.Width * scale));
            var previewHeight = Math.Max(1, (int)Math.Round(source.Height * scale));
            var preview = new Bitmap(previewWidth, previewHeight, PixelFormat.Format24bppRgb);
            using (var graphics = Graphics.FromImage(preview))
            {
                graphics.InterpolationMode = InterpolationMode.Low;
                graphics.PixelOffsetMode = PixelOffsetMode.Half;
                graphics.SmoothingMode = SmoothingMode.None;
                graphics.DrawImage(source, 0, 0, previewWidth, previewHeight);
            }

            return preview;
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

        public Bitmap CaptureCurrentImageSnapshot()
        {
            return null;
        }

        private bool TryStartGrayWaveformSelectionDrag(MouseEventArgs e)
        {
            lock (_imageLock)
            {
                if (!_grayWaveformSelectionActive || e.Button != MouseButtons.Left || _grayWaveformSelectionSource == null)
                {
                    return false;
                }

                var imageBounds = GetCurrentDisplayImageBoundsUnsafe();
                if (!imageBounds.Contains(e.Location))
                {
                    return false;
                }

                _grayWaveformDragging = true;
                _grayWaveformSelectionStart = ClientToImage(e.Location, imageBounds, GetCurrentImageSizeUnsafe());
                _grayWaveformSelectionEnd = _grayWaveformSelectionStart;
                viewerPanel.Invalidate();
                return true;
            }
        }

        private bool TryUpdateGrayWaveformSelectionDrag(MouseEventArgs e)
        {
            lock (_imageLock)
            {
                if (!_grayWaveformSelectionActive || !_grayWaveformDragging || _grayWaveformSelectionSource == null)
                {
                    return false;
                }

                var imageBounds = GetCurrentDisplayImageBoundsUnsafe();
                _grayWaveformSelectionEnd = ClampToImage(ClientToImage(e.Location, imageBounds, GetCurrentImageSizeUnsafe()), GetCurrentImageSizeUnsafe());
                viewerPanel.Invalidate();
                return true;
            }
        }

        private bool TryFinishGrayWaveformSelectionDrag(MouseEventArgs e)
        {
            GrayWaveformSelectionEventArgs args = null;
            lock (_imageLock)
            {
                if (!_grayWaveformSelectionActive || !_grayWaveformDragging || _grayWaveformSelectionSource == null)
                {
                    return false;
                }

                var imageBounds = GetCurrentDisplayImageBoundsUnsafe();
                _grayWaveformSelectionEnd = ClampToImage(ClientToImage(e.Location, imageBounds, GetCurrentImageSizeUnsafe()), GetCurrentImageSizeUnsafe());
                _grayWaveformDragging = false;
                var start = _grayWaveformSelectionStart ?? Point.Empty;
                var end = _grayWaveformSelectionEnd ?? start;
                var linePoints = BuildLinePoints(start, end);
                if (linePoints.Length < 2)
                {
                    return true;
                }

                args = new GrayWaveformSelectionEventArgs(_grayWaveformSelectionSource, start, end, linePoints);
            }

            if (args != null)
            {
                var handler = GrayWaveformSelectionCompleted;
                if (handler != null)
                {
                    handler(this, args);
                }
            }

            return true;
        }

        private void ClearGrayWaveformSelectionUnsafe()
        {
            if (_grayWaveformSelectionSource != null)
            {
                _grayWaveformSelectionSource.Dispose();
                _grayWaveformSelectionSource = null;
            }

            _grayWaveformSelectionActive = false;
            _grayWaveformSelectionStart = null;
            _grayWaveformSelectionEnd = null;
            _grayWaveformDragging = false;
        }

        private Rectangle GetCurrentImageBoundsUnsafe()
        {
            return GetImageBoundsUnsafe(GetCurrentImageSizeUnsafe());
        }

        private Rectangle GetCurrentDisplayImageBoundsUnsafe()
        {
            return GetImageBoundsUnsafe(GetCurrentDisplayImageSizeUnsafe());
        }

        private Rectangle GetImageBoundsUnsafe(Size size)
        {
            var bounds = GetDestinationRectangle();
            if (size.Width <= 0 || size.Height <= 0 || bounds.Width <= 0 || bounds.Height <= 0)
            {
                return Rectangle.Empty;
            }

            var scaleX = bounds.Width / (float)size.Width;
            var scaleY = bounds.Height / (float)size.Height;
            var zoom = _zoom > 0f ? _zoom : Math.Min(scaleX, scaleY);
            var drawWidth = size.Width * zoom;
            var drawHeight = size.Height * zoom;
            return new Rectangle(
                (int)Math.Round(_imageOffset.X),
                (int)Math.Round(_imageOffset.Y),
                Math.Max(1, (int)Math.Round(drawWidth)),
                Math.Max(1, (int)Math.Round(drawHeight)));
        }

        private Size GetCurrentImageSizeUnsafe()
        {
            if (_grayWaveformSelectionActive && _grayWaveformSelectionSource != null)
            {
                return new Size(_grayWaveformSelectionSource.Width, _grayWaveformSelectionSource.Height);
            }

            if (_sourceBitmap != null)
            {
                return _sourceBitmap.Size;
            }

            if (_rollingPreviewFrames.Count > 0)
            {
                return new Size(_rollingPreviewFrameWidth, _rollingPreviewFrameHeight * _rollingPreviewFrameCount);
            }

            if (_largeImageSource != null)
            {
                return new Size(_largeImageSource.Width, _largeImageSource.Height);
            }

            return Size.Empty;
        }

        private Size GetCurrentDisplayImageSizeUnsafe()
        {
            if (_sourceBitmap != null)
            {
                return _sourceBitmap.Size;
            }

            if (_rollingPreviewFrames.Count > 0)
            {
                return new Size(_rollingPreviewFrameWidth, _rollingPreviewFrameHeight * _rollingPreviewFrameCount);
            }

            if (_largeImageSource != null)
            {
                return new Size(_largeImageSource.Width, _largeImageSource.Height);
            }

            return Size.Empty;
        }

        private static Point[] BuildLinePoints(Point start, Point end)
        {
            var points = new List<Point>();
            var x0 = start.X;
            var y0 = start.Y;
            var x1 = end.X;
            var y1 = end.Y;
            var dx = Math.Abs(x1 - x0);
            var sx = x0 < x1 ? 1 : -1;
            var dy = -Math.Abs(y1 - y0);
            var sy = y0 < y1 ? 1 : -1;
            var err = dx + dy;

            while (true)
            {
                points.Add(new Point(x0, y0));
                if (x0 == x1 && y0 == y1)
                {
                    break;
                }

                var e2 = 2 * err;
                if (e2 >= dy)
                {
                    err += dy;
                    x0 += sx;
                }

                if (e2 <= dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }

            return points.ToArray();
        }

        private static Point ClientToImage(Point point, Rectangle imageBounds, Size imageSize)
        {
            if (imageBounds.Width <= 0 || imageBounds.Height <= 0 || imageSize.Width <= 0 || imageSize.Height <= 0)
            {
                return Point.Empty;
            }

            var x = (point.X - imageBounds.Left) * imageSize.Width / (float)imageBounds.Width;
            var y = (point.Y - imageBounds.Top) * imageSize.Height / (float)imageBounds.Height;
            return new Point((int)Math.Round(x), (int)Math.Round(y));
        }

        private static Point ImageToClient(Point point, Rectangle imageBounds, Size imageSize)
        {
            if (imageBounds.Width <= 0 || imageBounds.Height <= 0 || imageSize.Width <= 0 || imageSize.Height <= 0)
            {
                return Point.Empty;
            }

            var x = imageBounds.Left + (point.X * imageBounds.Width / (float)imageSize.Width);
            var y = imageBounds.Top + (point.Y * imageBounds.Height / (float)imageSize.Height);
            return new Point((int)Math.Round(x), (int)Math.Round(y));
        }

        private static Point ClampToImage(Point point, Size imageSize)
        {
            if (imageSize.Width <= 0 || imageSize.Height <= 0)
            {
                return Point.Empty;
            }

            return new Point(
                Math.Max(0, Math.Min(imageSize.Width - 1, point.X)),
                Math.Max(0, Math.Min(imageSize.Height - 1, point.Y)));
        }

        private sealed class BitmapGrayPixelSource : IGrayPixelSource
        {
            private readonly Bitmap _bitmap;

            public BitmapGrayPixelSource(Bitmap bitmap)
            {
                _bitmap = bitmap;
            }

            public int Width { get { return _bitmap.Width; } }

            public int Height { get { return _bitmap.Height; } }

            public int GetGrayAt(int x, int y)
            {
                x = Math.Max(0, Math.Min(_bitmap.Width - 1, x));
                y = Math.Max(0, Math.Min(_bitmap.Height - 1, y));
                var pixel = _bitmap.GetPixel(x, y);
                return (pixel.R + pixel.G + pixel.B) / 3;
            }

            public void Dispose()
            {
                _bitmap.Dispose();
            }
        }

        private sealed class LargeImageGrayPixelSource : IGrayPixelSource
        {
            private readonly LargeImageSource _source;

            public LargeImageGrayPixelSource(LargeImageSource source)
            {
                _source = source;
            }

            public int Width { get { return _source.Width; } }

            public int Height { get { return _source.Height; } }

            public int GetGrayAt(int x, int y)
            {
                return _source.GetGrayAt(x, y);
            }

            public void Dispose()
            {
            }
        }
    }
}

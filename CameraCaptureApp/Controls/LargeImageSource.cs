using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SW = System.Windows;
using SWM = System.Windows.Media;
using SWMI = System.Windows.Media.Imaging;

namespace CameraCaptureApp.Controls
{
    internal sealed class LargeImageSource : IDisposable
    {
        private const int TileSourceSize = 1024;
        private const int MaxTileCacheCount = 96;

        private readonly object _sync = new object();
        private readonly FileStream _stream;
        private readonly SWMI.BitmapFrame _frame;
        private readonly Dictionary<string, Bitmap> _tileCache;
        private readonly LinkedList<string> _tileOrder;
        private readonly HashSet<string> _pendingTiles;
        private readonly List<PreviewLevel> _previewLevels;
        private bool _disposed;

        public LargeImageSource(string filePath)
        {
            _stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var decoder = SWMI.BitmapDecoder.Create(_stream, SWMI.BitmapCreateOptions.PreservePixelFormat, SWMI.BitmapCacheOption.OnLoad);
            _frame = decoder.Frames[0];
            Width = _frame.PixelWidth;
            Height = _frame.PixelHeight;
            _tileCache = new Dictionary<string, Bitmap>(StringComparer.Ordinal);
            _tileOrder = new LinkedList<string>();
            _pendingTiles = new HashSet<string>(StringComparer.Ordinal);
            _previewLevels = new List<PreviewLevel>();
            InitializePreviewLevels();
        }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public Rectangle GetVisibleTileBounds(Rectangle sourceRect)
        {
            var normalized = NormalizeRect(sourceRect);
            var tileX = (normalized.X / TileSourceSize) * TileSourceSize;
            var tileY = (normalized.Y / TileSourceSize) * TileSourceSize;
            var width = Math.Min(TileSourceSize, Width - tileX);
            var height = Math.Min(TileSourceSize, Height - tileY);
            return new Rectangle(tileX, tileY, width, height);
        }

        public PreviewBitmap GetBestPreview(float zoom)
        {
            lock (_sync)
            {
                ThrowIfDisposed();
                PreviewLevel selected = _previewLevels[_previewLevels.Count - 1];
                for (var i = 0; i < _previewLevels.Count; i++)
                {
                    if (_previewLevels[i].Scale >= zoom)
                    {
                        selected = _previewLevels[i];
                        break;
                    }
                }

                if (selected.Bitmap == null)
                {
                    selected.Bitmap = CreateScaledBitmap(selected.DecodeWidth, selected.DecodeHeight);
                }

                return new PreviewBitmap((Bitmap)selected.Bitmap.Clone(), selected.Scale);
            }
        }

        public bool TryGetTile(Rectangle sourceRect, out Bitmap tile)
        {
            var normalized = NormalizeRect(sourceRect);
            var key = CreateTileKey(normalized);

            lock (_sync)
            {
                ThrowIfDisposed();
                Bitmap cached;
                if (_tileCache.TryGetValue(key, out cached))
                {
                    TouchKey(key);
                    tile = (Bitmap)cached.Clone();
                    return true;
                }
            }

            tile = null;
            return false;
        }

        public void QueueTile(Rectangle sourceRect, Action onReady)
        {
            var normalized = NormalizeRect(sourceRect);
            var key = CreateTileKey(normalized);

            lock (_sync)
            {
                if (_disposed || _tileCache.ContainsKey(key) || _pendingTiles.Contains(key))
                {
                    return;
                }

                _pendingTiles.Add(key);
            }

            Task.Run(
                () =>
                {
                    Bitmap tile = null;
                    try
                    {
                        lock (_sync)
                        {
                            if (_disposed)
                            {
                                return;
                            }

                            tile = CreateTileBitmap(normalized);
                            _tileCache[key] = tile;
                            _tileOrder.AddFirst(key);
                            TrimCache();
                            tile = null;
                        }

                        if (onReady != null)
                        {
                            onReady();
                        }
                    }
                    finally
                    {
                        if (tile != null)
                        {
                            tile.Dispose();
                        }

                        lock (_sync)
                        {
                            _pendingTiles.Remove(key);
                        }
                    }
                });
        }

        public void PrefetchNeighborhood(Rectangle centerRect, Action onReady)
        {
            var center = GetVisibleTileBounds(centerRect);
            for (var offsetY = -1; offsetY <= 1; offsetY++)
            {
                for (var offsetX = -1; offsetX <= 1; offsetX++)
                {
                    if (offsetX == 0 && offsetY == 0)
                    {
                        continue;
                    }

                    var neighbor = new Rectangle(
                        center.X + (offsetX * TileSourceSize),
                        center.Y + (offsetY * TileSourceSize),
                        TileSourceSize,
                        TileSourceSize);

                    if (neighbor.Right <= 0 || neighbor.Bottom <= 0 || neighbor.X >= Width || neighbor.Y >= Height)
                    {
                        continue;
                    }

                    QueueTile(neighbor, onReady);
                }
            }
        }

        public void Dispose()
        {
            lock (_sync)
            {
                if (_disposed)
                {
                    return;
                }

                _disposed = true;
                foreach (var level in _previewLevels)
                {
                    if (level.Bitmap != null)
                    {
                        level.Bitmap.Dispose();
                        level.Bitmap = null;
                    }
                }

                foreach (var pair in _tileCache)
                {
                    pair.Value.Dispose();
                }

                _tileCache.Clear();
                _tileOrder.Clear();
                _pendingTiles.Clear();
                _stream.Dispose();
            }
        }

        private void InitializePreviewLevels()
        {
            AddPreviewLevel(512);
            AddPreviewLevel(1024);
            AddPreviewLevel(2048);
            _previewLevels.Sort((a, b) => b.Scale.CompareTo(a.Scale));
        }

        private void AddPreviewLevel(int maxDimension)
        {
            var scale = Math.Min((double)maxDimension / Width, (double)maxDimension / Height);
            scale = Math.Min(scale, 1d);
            if (scale <= 0d)
            {
                scale = 1d;
            }

            var decodeWidth = Math.Max(1, (int)Math.Round(Width * scale));
            var decodeHeight = Math.Max(1, (int)Math.Round(Height * scale));
            _previewLevels.Add(new PreviewLevel((float)scale, decodeWidth, decodeHeight));
        }

        private Rectangle NormalizeRect(Rectangle sourceRect)
        {
            var x = Math.Max(0, Math.Min(sourceRect.X, Width - 1));
            var y = Math.Max(0, Math.Min(sourceRect.Y, Height - 1));
            var width = Math.Max(1, Math.Min(sourceRect.Width, Width - x));
            var height = Math.Max(1, Math.Min(sourceRect.Height, Height - y));
            return new Rectangle(x, y, width, height);
        }

        private void TouchKey(string key)
        {
            var node = _tileOrder.Find(key);
            if (node == null)
            {
                return;
            }

            _tileOrder.Remove(node);
            _tileOrder.AddFirst(node);
        }

        private void TrimCache()
        {
            while (_tileOrder.Count > MaxTileCacheCount)
            {
                var last = _tileOrder.Last;
                if (last == null)
                {
                    break;
                }

                Bitmap bitmap;
                if (_tileCache.TryGetValue(last.Value, out bitmap))
                {
                    bitmap.Dispose();
                    _tileCache.Remove(last.Value);
                }

                _tileOrder.RemoveLast();
            }
        }

        private Bitmap CreateTileBitmap(Rectangle sourceRect)
        {
            var cropped = new SWMI.CroppedBitmap(
                _frame,
                new SW.Int32Rect(sourceRect.X, sourceRect.Y, sourceRect.Width, sourceRect.Height));
            return ConvertToBitmap(cropped);
        }

        private Bitmap CreateScaledBitmap(int decodeWidth, int decodeHeight)
        {
            var preview = new SWMI.TransformedBitmap(
                _frame,
                new SWM.ScaleTransform((double)decodeWidth / Width, (double)decodeHeight / Height));
            return ConvertToBitmap(preview);
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("LargeImageSource");
            }
        }

        private static string CreateTileKey(Rectangle rect)
        {
            return rect.X + ":" + rect.Y + ":" + rect.Width + ":" + rect.Height;
        }

        private static Bitmap ConvertToBitmap(SWMI.BitmapSource source)
        {
            var formatted = new SWMI.FormatConvertedBitmap(source, SWM.PixelFormats.Bgr32, null, 0);
            var stride = formatted.PixelWidth * 4;
            var pixels = new byte[stride * formatted.PixelHeight];
            formatted.CopyPixels(pixels, stride, 0);

            var bitmap = new Bitmap(formatted.PixelWidth, formatted.PixelHeight, PixelFormat.Format32bppArgb);
            var data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
            try
            {
                Marshal.Copy(pixels, 0, data.Scan0, pixels.Length);
            }
            finally
            {
                bitmap.UnlockBits(data);
            }

            return bitmap;
        }

        internal sealed class PreviewBitmap : IDisposable
        {
            public PreviewBitmap(Bitmap bitmap, float scale)
            {
                Bitmap = bitmap;
                Scale = scale;
            }

            public Bitmap Bitmap { get; private set; }

            public float Scale { get; private set; }

            public void Dispose()
            {
                if (Bitmap != null)
                {
                    Bitmap.Dispose();
                    Bitmap = null;
                }
            }
        }

        private sealed class PreviewLevel
        {
            public PreviewLevel(float scale, int decodeWidth, int decodeHeight)
            {
                Scale = scale;
                DecodeWidth = decodeWidth;
                DecodeHeight = decodeHeight;
            }

            public float Scale { get; private set; }

            public int DecodeWidth { get; private set; }

            public int DecodeHeight { get; private set; }

            public Bitmap Bitmap { get; set; }
        }
    }
}

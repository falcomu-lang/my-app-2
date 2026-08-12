using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using SWM = System.Windows.Media;
using SWMI = System.Windows.Media.Imaging;

namespace CameraCaptureApp.Services
{
    public sealed class FrameRecorder : IDisposable
    {
        private readonly object _sync = new object();
        private readonly List<Bitmap> _rollingFrames = new List<Bitmap>();
        private Bitmap _latestFrame;

        public void StoreLatest(Bitmap frame)
        {
            if (frame == null)
            {
                return;
            }

            var copy = (Bitmap)frame.Clone();
            lock (_sync)
            {
                if (_latestFrame != null)
                {
                    _latestFrame.Dispose();
                }

                _latestFrame = copy;
            }
        }

        public void StoreRolling(Bitmap frame, int frameCount)
        {
            if (frame == null)
            {
                return;
            }

            frameCount = Math.Max(1, frameCount);
            var copy = (Bitmap)frame.Clone();
            lock (_sync)
            {
                _rollingFrames.Insert(0, copy);
                while (_rollingFrames.Count > frameCount)
                {
                    var lastIndex = _rollingFrames.Count - 1;
                    _rollingFrames[lastIndex].Dispose();
                    _rollingFrames.RemoveAt(lastIndex);
                }
            }
        }

        public void ClearRolling()
        {
            lock (_sync)
            {
                DisposeRollingFramesUnsafe();
            }
        }

        public Bitmap SnapshotLatest()
        {
            lock (_sync)
            {
                return _latestFrame == null ? null : (Bitmap)_latestFrame.Clone();
            }
        }

        public Bitmap SnapshotRolling()
        {
            List<Bitmap> frames;
            lock (_sync)
            {
                if (_rollingFrames.Count == 0)
                {
                    return null;
                }

                frames = new List<Bitmap>(_rollingFrames.Count);
                foreach (var frame in _rollingFrames)
                {
                    frames.Add((Bitmap)frame.Clone());
                }
            }

            try
            {
                return CombineVertical(frames);
            }
            finally
            {
                foreach (var frame in frames)
                {
                    frame.Dispose();
                }
            }
        }

        public bool SaveRollingPng(string filePath)
        {
            List<Bitmap> frames;
            lock (_sync)
            {
                if (_rollingFrames.Count == 0)
                {
                    return false;
                }

                frames = new List<Bitmap>(_rollingFrames.Count);
                foreach (var frame in _rollingFrames)
                {
                    frames.Add((Bitmap)frame.Clone());
                }
            }

            try
            {
                SaveVerticalPng(frames, filePath);
                return true;
            }
            finally
            {
                foreach (var frame in frames)
                {
                    frame.Dispose();
                }
            }
        }

        public void Dispose()
        {
            lock (_sync)
            {
                if (_latestFrame != null)
                {
                    _latestFrame.Dispose();
                    _latestFrame = null;
                }

                DisposeRollingFramesUnsafe();
            }
        }

        private void DisposeRollingFramesUnsafe()
        {
            foreach (var frame in _rollingFrames)
            {
                frame.Dispose();
            }

            _rollingFrames.Clear();
        }

        private static Bitmap CombineVertical(IList<Bitmap> frames)
        {
            var width = 0;
            var height = 0;
            foreach (var frame in frames)
            {
                width = Math.Max(width, frame.Width);
                height += frame.Height;
            }

            if (width <= 0 || height <= 0)
            {
                return null;
            }

            var combined = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            using (var graphics = Graphics.FromImage(combined))
            {
                graphics.Clear(Color.Black);
                var y = 0;
                foreach (var frame in frames)
                {
                    var destination = new Rectangle(0, y + frame.Height, frame.Width, -frame.Height);
                    graphics.DrawImage(frame, destination);
                    y += frame.Height;
                }
            }

            return combined;
        }

        private static void SaveVerticalPng(IList<Bitmap> frames, string filePath)
        {
            var width = 0;
            var height = 0;
            foreach (var frame in frames)
            {
                width = Math.Max(width, frame.Width);
                checked
                {
                    height += frame.Height;
                }
            }

            if (width <= 0 || height <= 0)
            {
                return;
            }

            var stride = checked(width * 4);
            var pixels = new byte[checked(stride * height)];
            var destinationY = 0;
            foreach (var frame in frames)
            {
                CopyFrameFlippedIntoBuffer(frame, pixels, stride, destinationY);
                destinationY += frame.Height;
            }

            var bitmapSource = SWMI.BitmapSource.Create(
                width,
                height,
                96,
                96,
                SWM.PixelFormats.Bgr32,
                null,
                pixels,
                stride);
            var encoder = new SWMI.PngBitmapEncoder();
            encoder.Frames.Add(SWMI.BitmapFrame.Create(bitmapSource));

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                encoder.Save(stream);
            }
        }

        private static void CopyFrameFlippedIntoBuffer(Bitmap frame, byte[] destinationPixels, int destinationStride, int destinationY)
        {
            using (var converted = new Bitmap(frame.Width, frame.Height, PixelFormat.Format32bppArgb))
            {
                using (var graphics = Graphics.FromImage(converted))
                {
                    graphics.DrawImageUnscaled(frame, 0, 0);
                }

                var sourceRect = new Rectangle(0, 0, converted.Width, converted.Height);
                var data = converted.LockBits(sourceRect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                try
                {
                    var sourceStride = Math.Abs(data.Stride);
                    var sourceRow = new byte[sourceStride];
                    for (var y = 0; y < converted.Height; y++)
                    {
                        Marshal.Copy(data.Scan0 + ((converted.Height - 1 - y) * data.Stride), sourceRow, 0, sourceStride);
                        Buffer.BlockCopy(sourceRow, 0, destinationPixels, ((destinationY + y) * destinationStride), Math.Min(sourceStride, destinationStride));
                    }
                }
                finally
                {
                    converted.UnlockBits(data);
                }
            }
        }
    }
}

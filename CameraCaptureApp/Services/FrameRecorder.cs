using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

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
    }
}

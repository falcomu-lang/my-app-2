using System;
using System.Drawing;

namespace CameraCaptureApp.Services
{
    public sealed class GrayWaveformSelectionEventArgs : EventArgs
    {
        public GrayWaveformSelectionEventArgs(Bitmap snapshot, Point startPoint, Point endPoint, Point[] linePoints)
        {
            Snapshot = snapshot;
            StartPoint = startPoint;
            EndPoint = endPoint;
            LinePoints = linePoints ?? new Point[0];
        }

        public Bitmap Snapshot { get; private set; }

        public Point StartPoint { get; private set; }

        public Point EndPoint { get; private set; }

        public Point[] LinePoints { get; private set; }
    }
}

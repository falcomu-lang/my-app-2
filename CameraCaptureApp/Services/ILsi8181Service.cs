using System;
using System.Collections.Generic;

namespace CameraCaptureApp.Services
{
    public interface ILsi8181Service : IDisposable
    {
        bool IsInitialized { get; }

        IReadOnlyList<Lsi8181CardInfo> LastCards { get; }

        int LastStatusCode { get; }

        string LastMessage { get; }

        IReadOnlyList<Lsi8181CardInfo> InitializeAndScanCards();

        int ReadCounter(byte cardId);

        int ReadCompareValue(byte cardId);

        void ClearCompareValue(byte cardId);

        void ClearCounter(byte cardId);

        Lsi8181CounterInputMode ReadCounterInputMode(byte cardId);

        void SetMultipleRate(byte cardId, byte multipleRate);

        int ReadAutoIncrement(byte cardId);

        void ApplyAutoIncrementMode(byte cardId, int incrementValue, ushort cmpOutWidth);

        void Close();
    }
}

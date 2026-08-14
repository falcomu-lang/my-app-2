using System;
using CameraCaptureApp.Native;

namespace CameraCaptureApp.Services
{
    internal sealed class Lsi8181MeterWheelService : IDisposable
    {
        private bool _initialized;
        private byte _cardId;

        public byte CardId
        {
            get { return _cardId; }
        }

        public bool IsInitialized
        {
            get { return _initialized; }
        }

        public void Open(byte cardId)
        {
            var status = Lsi8181Native.LSI8181_initial();
            EnsureSuccess(status, "Initialize LSI8181");

            _initialized = true;
            _cardId = cardId;

            ulong ioAddress = 0;
            ulong tcAddress = 0;
            status = Lsi8181Native.LSI8181_info(cardId, ref ioAddress, ref tcAddress);
            EnsureSuccess(status, "Read LSI8181 card info");

            status = Lsi8181Native.LSI8181_CI_mode_set(
                cardId,
                Lsi8181Native.QuadratureMode,
                Lsi8181Native.DebounceTime1Us,
                Lsi8181Native.Multiple4);
            EnsureSuccess(status, "Set encoder input mode");

            status = Lsi8181Native.LSI8181_compare_mode_set(cardId, Lsi8181Native.CompareAutoIncrement);
            EnsureSuccess(status, "Set compare mode to auto increment");

            status = Lsi8181Native.LSI8181_counter_start(cardId, Lsi8181Native.CounterRun);
            EnsureSuccess(status, "Start encoder counter");
        }

        public int ReadEncoder()
        {
            EnsureOpen();
            var value = 0;
            var status = Lsi8181Native.LSI8181_counter_read(_cardId, ref value);
            EnsureSuccess(status, "Read encoder counter");
            return value;
        }

        public void SetEncoder(int value)
        {
            EnsureOpen();
            var status = Lsi8181Native.LSI8181_counter_set(_cardId, value);
            EnsureSuccess(status, "Set encoder counter");
        }

        public int ReadCompare()
        {
            EnsureOpen();
            var value = 0;
            var status = Lsi8181Native.LSI8181_compare_value_read(_cardId, ref value);
            EnsureSuccess(status, "Read compare value");
            return value;
        }

        public void SetCompare(int value)
        {
            EnsureOpen();
            var status = Lsi8181Native.LSI8181_compare_value_set(_cardId, value);
            EnsureSuccess(status, "Set compare value");
        }

        public void SetCompareIncrement(int value)
        {
            EnsureOpen();
            var status = Lsi8181Native.LSI8181_compare_increment_set(_cardId, value);
            EnsureSuccess(status, "Set compare auto increment value");
        }

        public void Close()
        {
            if (!_initialized)
            {
                return;
            }

            try
            {
                Lsi8181Native.LSI8181_counter_stop(_cardId);
            }
            finally
            {
                Lsi8181Native.LSI8181_close();
                _initialized = false;
            }
        }

        public void Dispose()
        {
            Close();
        }

        private void EnsureOpen()
        {
            if (!_initialized)
            {
                throw new InvalidOperationException("LSI8181 meter wheel card is not connected.");
            }
        }

        private static void EnsureSuccess(uint status, string action)
        {
            if (status != Lsi8181Native.Success)
            {
                throw new InvalidOperationException(action + " failed. Status: " + status);
            }
        }
    }
}

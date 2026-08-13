using System;
using System.Runtime.InteropServices;

namespace CameraCaptureApp.Services
{
    internal static class Lsi8181Native
    {
        public const int NoError = 0;
        public const int CardIdMax = 15;

        [DllImport("LSI8181_64.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int LSI8181_initial();

        [DllImport("LSI8181_64.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int LSI8181_close();

        [DllImport("LSI8181_64.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int LSI8181_info(byte cardId, ref ulong ioAddress, ref ulong timerCounterAddress);

        [DllImport("LSI8181_64.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int LSI8181_counter_read(byte cardId, ref int counterValue);

        [DllImport("LSI8181_64.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int LSI8181_counter_set(byte cardId, int counterValue);

        [DllImport("LSI8181_64.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int LSI8181_CI_mode_read(byte cardId, ref byte inputMode, ref byte debounceTime, ref byte multipleRate);

        [DllImport("LSI8181_64.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int LSI8181_CI_mode_set(byte cardId, byte inputMode, byte debounceTime, byte multipleRate);

        [DllImport("LSI8181_64.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int LSI8181_compare_increment_read(byte cardId, ref int incrementValue);

        [DllImport("LSI8181_64.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int LSI8181_compare_increment_set(byte cardId, int incrementValue);

        [DllImport("LSI8181_64.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int LSI8181_compare_mode_set(byte cardId, byte compareMode);

        [DllImport("LSI8181_64.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int LSI8181_counter_start(byte cardId, byte mode);
    }
}

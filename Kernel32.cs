using System;
using System.Runtime.InteropServices;
using System.Text;

namespace WinHook
{
    public static class Kernel32
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int GetModuleFileName([In] IntPtr hModule, [Out] StringBuilder lpFilename, int nSize);

        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateEvent([In] IntPtr lpEventAttributes,
            [In] [MarshalAs(UnmanagedType.Bool)] bool bManualReset,
            [In] [MarshalAs(UnmanagedType.Bool)] bool bInitialState,
            [In] [MarshalAs(UnmanagedType.LPWStr)] string lpName);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle([In] IntPtr hObject);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetEvent([In] IntPtr hEvent);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ResetEvent([In] IntPtr hEvent);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PulseEvent([In] IntPtr hEvent);

        [DllImport("kernel32", ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.I4)]
        public static extern int WaitForSingleObject([In] IntPtr handle,
            [In] [MarshalAs(UnmanagedType.I4)] int milliseconds);
    }
}
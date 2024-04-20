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
        public static extern int GetModuleFileName(
            IntPtr hModule, 
            [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpFilename, 
            int nSize);

        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateEvent(
            IntPtr lpEventAttributes,
            bool bManualReset,
            bool bInitialState,
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpName);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetEvent(IntPtr hEvent);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ResetEvent(IntPtr hEvent);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PulseEvent(IntPtr hEvent);

        [DllImport("kernel32.dll", SetLastError = true,
            ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.I4)]
        public static extern int WaitForSingleObject(
            [In,] IntPtr handle,
            [In, MarshalAs(UnmanagedType.I4)] int milliseconds);
    }
}

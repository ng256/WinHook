using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WinHook
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct KeyboardLowLevelHookData
    {
        public Keys vkCode;
        public char scanCode;
        public LowLevelKeyboardHookFlag flags;
        public int time;
        public IntPtr dwExtraInfo;
    }
}
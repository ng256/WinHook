using System;

namespace WinHook
{
    [Flags]
    public enum KeyEventType : int
    {
        WM_KEYDOWN = 0x100,
        WM_KEYUP = 0x101,
        WM_SYSKEYDOWN = 0x104,
        WM_SYSKEYUP = 0x105
    }
}
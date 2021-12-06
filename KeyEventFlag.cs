using System;

namespace WinHook
{
    [Flags]
    public enum KeyEventFlag : uint
    {
        KEYEVENTF_EXTENDEDKEY = 0x01,
        KEYEVENTF_KEYUP = 0x02,
        KEYEVENTF_UNICODE = 0x04,
        KEYEVENTF_SCANCODE = 0x08

    }
}
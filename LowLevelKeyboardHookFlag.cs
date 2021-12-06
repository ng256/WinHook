using System;

namespace WinHook
{
    [Flags]
    public enum LowLevelKeyboardHookFlag
    {
        LLKHF_EXTENDED = 0x1,
        LLKHF_LOWER_IL_INJECTED = 0x2,
        LLKHF_INJECTED = 0x10,
        LLKHF_ALTDOWN = 0x20,
        LLKHF_UP = 0x80
    }
}
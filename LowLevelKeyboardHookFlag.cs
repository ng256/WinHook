using System;

namespace WinHook
{
    [Flags]
    public enum LowLevelKeyboardHookFlag
    {
        LLKHF_EXTENDED = KeyFlag.KF_EXTENDED >> 8,
        LLKHF_LOWER_IL_INJECTED = 0x00000002,
        LLKHF_INJECTED = 0x00000010,
        LLKHF_ALTDOWN = KeyFlag.KF_ALTDOWN >> 8,
        LLKHF_UP = KeyFlag.KF_UP >> 8
    }
}
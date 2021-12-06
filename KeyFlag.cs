using System;

namespace WinHook
{
    [Flags]
    public enum KeyFlag
    {
        KF_EXTENDED = 0x100,
        KF_DLGMODE = 0x800,
        KF_MENUMODE = 0x1000,
        KF_ALTDOWN = 0x2000,
        KF_REPEAT = 0x4000,
        KF_UP = 0x8000,
    }
}

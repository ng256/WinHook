using System;

namespace WinHook
{
    [Flags]
    public enum KeyboardLayoutFlags : uint
    {
        KLF_ACTIVATE = 0b00000001,
        KLF_NOTELLSHELL = 0b10000000,
        KLF_REORDER = 0b00001000,
        KLF_REPLACELANG = 0b00010000,
        KLF_SUBSTITUTE_OK = 0b000000010,
        KLF_SETFORPROCESS = 0b100000000
    }
}
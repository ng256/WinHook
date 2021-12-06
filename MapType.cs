using System;

namespace WinHook
{
    [Flags]
    public enum MapType : uint
    {
        MAPVK_VK_TO_VSC = 0x0u,
        MAPVK_VSC_TO_VK = 0x1u,
        MAPVK_VK_TO_CHAR = 0x2u,
        MAPVK_VSC_TO_VK_EX = 0x3u
    }
}
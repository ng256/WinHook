using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using static WinHook.User32;
using static WinHook.Kernel32;

namespace WinHook
{
    public class KeyboardHook : IDisposable
    {
        private readonly LowLevelKeyboardProc _callback;
        private bool _disposed;
        private IntPtr _hookPtr = IntPtr.Zero;

        public KeyboardHook()
        {
            _callback = Callback;
            using (var process = Process.GetCurrentProcess())
            {
                using (var processModule = process.MainModule)
                {
                    var moduleName = processModule?.ModuleName ?? throw new NullReferenceException();
                    _hookPtr = SetWindowsHookEx(HookType.WH_KEYBOARD_LL, _callback,
                        Kernel32.GetModuleHandle(moduleName), 0u);
                }
            }

            if (_hookPtr == IntPtr.Zero) throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public event LowLevelKeyEventHandler KeyDown;
        public event LowLevelKeyEventHandler KeyUp;

        private IntPtr Callback(HookCode nCode, KeyEventType wParam, KeyboardLowLevelHookData lParam)
        {
            var shift = GetKeyState(16).HasFlag(KeyState.KeyPressed);
            var control = GetKeyState(17).HasFlag(KeyState.KeyPressed);
            var alt = GetKeyState(18).HasFlag(KeyState.KeyPressed);
            var vk = lParam.vkCode;
            var scan = lParam.scanCode;
            var hkl = GetKeyboardLayout();
            var layout = GetKeyboardLayoutId(hkl);
            var text = GetText(vk, shift, control, alt, hkl, scan);
            var flag = lParam.flags;
            var eventArgs = new LowLevelKeyboardEventArgs(vk)
            {
                Shift = shift,
                Control = control,
                Alt = alt,
                ScanCode = scan,
                Layout = layout,
                Text = text,
                Flag = flag
            };
            switch (wParam)
            {
                case KeyEventType.WM_KEYDOWN:
                    KeyDown?.Invoke(this, eventArgs);
                    if (eventArgs.Handled) return new IntPtr(1);
                    break;
                case KeyEventType.WM_KEYUP:
                    KeyUp?.Invoke(this, eventArgs);
                    if (eventArgs.Handled) return new IntPtr(1);
                    break;
            }

            return CallNextHookEx(_hookPtr, nCode, wParam, lParam);
        }

        internal static IntPtr GetKeyboardLayout()
        {
            int lpdwProcessId;
            return User32.GetKeyboardLayout(GetWindowThreadProcessId(GetForegroundWindow(), out lpdwProcessId));
        }

        internal static int GetKeyboardLayoutId(IntPtr keyboardLayout)
        {
            var cultureInfo = CultureInfo.CurrentCulture;
            var installedInputLanguages = InputLanguage.InstalledInputLanguages;
            for (var i = 0; i < installedInputLanguages.Count; i++)
                if (keyboardLayout == installedInputLanguages[i].Handle)
                    cultureInfo = installedInputLanguages[i].Culture;
            return cultureInfo.KeyboardLayoutId;
        }

        internal static string ToUnicode(Keys virtualKey, IntPtr keybLayout)
        {
            var lpKeyState = new byte[256];
            GetKeyboardState(lpKeyState);
            var vScanCode = (char)MapVirtualKeyEx(virtualKey, MapType.MAPVK_VK_TO_CHAR, keybLayout);
            var stringBuilder = new StringBuilder(5);
            ToUnicodeEx(virtualKey, vScanCode, lpKeyState, stringBuilder, 5, 0u, keybLayout);
            return stringBuilder.ToString();
        }

        internal static string ToUnicode(Keys virtualKey, IntPtr keybLayout, out char scanCode)
        {
            var lpKeyState = new byte[256];
            GetKeyboardState(lpKeyState);
            scanCode = (char)MapVirtualKeyEx(virtualKey, MapType.MAPVK_VK_TO_CHAR, keybLayout);
            var stringBuilder = new StringBuilder(5);
            ToUnicodeEx(virtualKey, scanCode, lpKeyState, stringBuilder, 5, 0u, keybLayout);
            return stringBuilder.ToString();
        }

        private static string GetText(Keys vk, bool shift, bool control, bool alt, IntPtr hkl, char scan)
        {
            if (control || alt)
            {
                var str = "";
                if (control) str += "{Control}+";
                if (alt) str += "{Alt}+";
                return $"{str}{{{vk}}}\r\n";
            }

            switch (vk)
            {
                case Keys.Return:
                    return Environment.NewLine;
                case Keys.Capital:
                    return "{CapsLock}";
                case Keys.Escape:
                    return "{Escape}";
                case Keys.Space:
                    return " ";
                case Keys.Left:
                    return "{←}";
                case Keys.Up:
                    return "{↑}";
                case Keys.Right:
                    return "{→}";
                case Keys.Down:
                    return "{↓}";
                case Keys.KeyCode:
                case Keys.Modifiers:
                case Keys.None:
                case Keys.Cancel:
                case Keys.MButton:
                case Keys.XButton1:
                case Keys.XButton2:
                case Keys.Back:
                case Keys.Tab:
                case Keys.LineFeed:
                case Keys.Clear:
                case Keys.ShiftKey:
                case Keys.ControlKey:
                case Keys.Menu:
                case Keys.Pause:
                case Keys.KanaMode:
                case Keys.JunjaMode:
                case Keys.FinalMode:
                case Keys.HanjaMode:
                case Keys.IMEConvert:
                case Keys.IMENonconvert:
                case Keys.IMEAccept:
                case Keys.IMEModeChange:
                case Keys.Prior:
                case Keys.Next:
                case Keys.End:
                case Keys.Home:
                case Keys.Select:
                case Keys.Print:
                case Keys.Execute:
                case Keys.Snapshot:
                case Keys.Insert:
                case Keys.Delete:
                case Keys.Help:
                case Keys.LWin:
                case Keys.RWin:
                case Keys.Apps:
                case Keys.Sleep:
                case Keys.F1:
                case Keys.F2:
                case Keys.F3:
                case Keys.F4:
                case Keys.F5:
                case Keys.F6:
                case Keys.F7:
                case Keys.F8:
                case Keys.F9:
                case Keys.F10:
                case Keys.F11:
                case Keys.F12:
                case Keys.F13:
                case Keys.F14:
                case Keys.F15:
                case Keys.F16:
                case Keys.F17:
                case Keys.F18:
                case Keys.F19:
                case Keys.F20:
                case Keys.F21:
                case Keys.F22:
                case Keys.F23:
                case Keys.F24:
                case Keys.NumLock:
                case Keys.Scroll:
                case Keys.LShiftKey:
                case Keys.RShiftKey:
                case Keys.LControlKey:
                case Keys.RControlKey:
                case Keys.LMenu:
                case Keys.RMenu:
                case Keys.BrowserBack:
                case Keys.BrowserForward:
                case Keys.BrowserRefresh:
                case Keys.BrowserStop:
                case Keys.BrowserSearch:
                case Keys.BrowserFavorites:
                case Keys.BrowserHome:
                case Keys.VolumeMute:
                case Keys.VolumeDown:
                case Keys.VolumeUp:
                case Keys.MediaNextTrack:
                case Keys.MediaPreviousTrack:
                case Keys.MediaStop:
                case Keys.MediaPlayPause:
                case Keys.LaunchMail:
                case Keys.SelectMedia:
                case Keys.LaunchApplication1:
                case Keys.LaunchApplication2:
                case Keys.ProcessKey:
                case Keys.Packet:
                case Keys.Attn:
                case Keys.Crsel:
                case Keys.Exsel:
                case Keys.EraseEof:
                case Keys.Play:
                case Keys.Zoom:
                case Keys.NoName:
                case Keys.Pa1:
                case Keys.OemClear:
                case Keys.Shift:
                case Keys.Control:
                case Keys.Alt:
                    return $"{{{vk}}}";
                default:
                    return ToUnicode(vk, hkl, out scan);
            }
        }

        private void ReleaseUnmanagedResources()
        {
            if (_hookPtr != IntPtr.Zero) UnhookWindowsHookEx(_hookPtr);
        }

        protected virtual void Dispose(bool disposing)
        {
            ReleaseUnmanagedResources();
            _hookPtr = IntPtr.Zero;
            _disposed = true;
        }

        ~KeyboardHook()
        {
            Dispose(false);
        }
    }
}

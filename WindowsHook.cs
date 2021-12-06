using System;
using System.Diagnostics;
using System.Text;
using System.Timers;
using static WinHook.User32;

namespace WinHook
{
    public delegate void WindowTextChangeEventHandler(object sender, WindowTextChangeArgs eventArgs);

    public class WindowsHook : IDisposable
    {
        private Timer _timer = new Timer(100.0);

        public WindowsHook()
        {
            _timer.Elapsed += TimerOnElapsed;
            _timer.Start();
            ForegroundWindow = GetWindowInfo(GetForegroundWindow());
        }

        public WindowInfo ForegroundWindow { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public event WindowTextChangeEventHandler WindowTextChanged;

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            var windowInfo = GetWindowInfo(GetForegroundWindow());
            if (ForegroundWindow != windowInfo)
            {
                var eventArgs = new WindowTextChangeArgs(windowInfo.Text)
                {
                    TimeStamp = e.SignalTime
                };
                WindowTextChanged?.Invoke(this, eventArgs);
            }

            ForegroundWindow = windowInfo;
        }

        internal static WindowInfo GetWindowInfo(IntPtr handle)
        {
            var length = GetWindowTextLength(handle) + 1;
            var stringBuilder = new StringBuilder(length);
            length = GetWindowText(handle, stringBuilder, length);
            var wndText = stringBuilder.ToString(0, length);
            var procWnd = GetProcessInfo(handle);
            var result = default(WindowInfo);
            result.Handle = handle;
            result.Text = wndText;
            result.ProcessInfo = procWnd;
            return result;
        }

        internal static ProcessInfo GetProcessInfo(IntPtr hWnd)
        {
            int lpdwProcessId;
            var windowThreadProcessId = GetWindowThreadProcessId(hWnd, out lpdwProcessId);
            var processName = Process.GetProcessById(lpdwProcessId).ProcessName;
            var result = default(ProcessInfo);
            result.processId = lpdwProcessId;
            result.threadId = windowThreadProcessId;
            result.processName = processName;
            return result;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing) _timer.Close();
            _timer = null;
        }
    }
}
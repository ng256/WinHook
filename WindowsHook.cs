using System;
using System.Diagnostics;
using System.Text;
using System.Timers;
using static WinHook.User32;

namespace WinHook
{
	public class WindowsHook : IDisposable
	{
        private WindowInfo _windowInfo;
        private Timer _timer = new Timer(100);
        
        public WindowInfo ForegroundWindow
		{
			get
			{
				return _windowInfo;
			}
			set
			{
                _windowInfo = value;
			}
        }

		public event WindowTextChangeEventHandler WindowTextChanged;

        public WindowsHook()
		{
            _timer.Elapsed += TimerOnElapsed;
            _timer.Start();
            ForegroundWindow = GetWindowInfo(GetForegroundWindow());
		}

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            
            var hwnd = GetForegroundWindow();
            var windowInfo = GetWindowInfo(hwnd);
            if (ForegroundWindow != windowInfo)
            {
                var eventArgs = new WindowTextChangeArgs(windowInfo.Text) {TimeStamp = e.SignalTime};
                WindowTextChanged?.Invoke(this, eventArgs);
            }

            ForegroundWindow = windowInfo;
        }

        internal static WindowInfo GetWindowInfo(IntPtr handle)
		{
			int length = GetWindowTextLength(handle) + 1;
			StringBuilder stringBuilder = new StringBuilder(length);
			length = GetWindowText(handle, stringBuilder, length);
			string wndText = stringBuilder.ToString(0, length);
			ProcessInfo procWnd = GetProcessInfo(handle);
			WindowInfo result = default(WindowInfo);
			result.Handle = handle;
			result.Text = wndText;
			result.ProcessInfo = procWnd;
			return result;
		}

        internal static ProcessInfo GetProcessInfo(IntPtr hWnd)
		{
			int lpdwProcessId;
			int windowThreadProcessId = GetWindowThreadProcessId(hWnd, out lpdwProcessId);
			string processName = Process.GetProcessById(lpdwProcessId).ProcessName;
			ProcessInfo result = default(ProcessInfo);
			result.processId = lpdwProcessId;
			result.threadId = windowThreadProcessId;
			result.processName = processName;
			return result;
		}

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _timer.Close();
            }

            _timer = null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

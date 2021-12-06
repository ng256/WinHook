using System;

namespace WinHook
{
    public delegate void WindowTextChangeEventHandler(object sender, WindowTextChangeArgs eventArgs);

    public class WindowTextChangeArgs : EventArgs
	{
        public string Text { get; set; }
        public DateTime TimeStamp { get; set; }

        public WindowTextChangeArgs(string text)
        {
            Text = text;
        }
    }
}

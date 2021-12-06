using System;

namespace WinHook
{
    public class WindowTextChangeArgs : EventArgs
    {
        public WindowTextChangeArgs(string text)
        {
            Text = text;
        }

        public string Text { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
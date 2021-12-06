using System;
using System.Windows.Forms;

namespace WinHook
{
    public delegate void ClipBoardEventHandler(ClipboardChangedEventArgs eventArgs);

    public class ClipboardChangedEventArgs : EventArgs
    {
        public IDataObject ClipboardData { get; set; }
        public string ClipboardDataFormat { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Text { get; set; }
    }
}

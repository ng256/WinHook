using System.Windows.Forms;

namespace WinHook
{
    public delegate void LowLevelKeyEventHandler(object sender, LowLevelKeyboardEventArgs eventArgs);

    public class LowLevelKeyboardEventArgs
    {
        private bool _handled;
        private bool _suppressKeyPress;

        public LowLevelKeyboardEventArgs(Keys virtualKey)
        {
            VirtualKey = virtualKey;
        }

        public Keys VirtualKey { get; }
        public char ScanCode { get; set; }
        public LowLevelKeyboardHookFlag Flag { get; set; }
        public int Layout { get; set; }
        public string Text { get; set; }
        public virtual bool Alt { get; set; }
        public bool Control { get; set; }
        public virtual bool Shift { get; set; }
        public bool Handled { get => _handled; set => _handled = value; }
        public bool SuppressKeyPress
        {
            get
            {
                return this._suppressKeyPress;
            }
            set
            {
                this._suppressKeyPress = value;
                this._handled = value;
            }
        }
    }
}

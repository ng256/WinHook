using System.Windows.Forms;

namespace WinHook
{
    public class LowLevelKeyboardEventArgs
    {
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

        public bool Handled { get; set; }

        public bool SuppressKeyPress
        {
            get => _suppressKeyPress;
            set
            {
                _suppressKeyPress = value;
                Handled = value;
            }
        }
    }
}
using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using static WinHook.User32;

namespace WinHook
{
    public delegate void ClipBoardEventHandler(ClipboardChangedEventArgs eventArgs);

    public class ClipboardHook : IDisposable
    {
        private ClipboardProc _clipboardProc;

        private bool _disposed;

        private Form _window = new Form
        {
            Text = string.Empty,
            Visible = false
        };

        public ClipboardHook()
        {
            if (_window != null)
            {
                var nextWind = SetClipboardViewer(_window.Handle);
                _clipboardProc = new ClipboardProc(nextWind);
                _clipboardProc.AssignHandle(_window.Handle);
                _clipboardProc.ClipBoardChanged += OnClipBoardChanged;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public event ClipBoardEventHandler ClipBoardChanged;

        private void OnClipBoardChanged(ClipboardChangedEventArgs e)
        {
            ClipBoardChanged?.Invoke(e);
        }

        private static string GetClipboardData(IDataObject data, string dataFormat)
        {
            var text = data.GetData(dataFormat) as string ?? string.Empty;
            if (dataFormat == DataFormats.Rtf) return Rtf2String(text);
            if (dataFormat == DataFormats.Text || dataFormat == DataFormats.OemText ||
                dataFormat == DataFormats.UnicodeText) return text;
            data.GetFormats();
            return string.Empty;
        }

        private static string GetDataFormat(IDataObject data)
        {
            foreach (var item in from f in typeof(DataFormats).GetFields(BindingFlags.Static | BindingFlags.Public)
                where f.FieldType == typeof(string)
                select (string) f.GetValue(null))
            {
                if (data.GetDataPresent(DataFormats.Rtf, true)) return item;
                if (data.GetDataPresent(DataFormats.Text, true)) return item;
                if (data.GetDataPresent(DataFormats.UnicodeText, true)) return item;
                if (data.GetDataPresent(DataFormats.OemText, true)) return item;
            }

            return null;
        }

        private static string Rtf2String(string rtfText)
        {
            return new RichTextBox
            {
                Rtf = rtfText
            }.Text;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _clipboardProc?.ReleaseHandle();
                _window.Dispose();
            }

            _window = null;
            _clipboardProc = null;
            _disposed = true;
        }

        ~ClipboardHook()
        {
            Dispose(false);
        }

        private class ClipboardProc : NativeWindow
        {
            private const int WM_DESTROY = 2;
            private const int WM_DRAWCLIPBOARD = 776;
            private const int WM_CHANGECBCHAIN = 781;

            public ClipboardProc(IntPtr nextWind)
            {
                NextWindow = nextWind;
            }

            public IntPtr NextWindow { get; set; }

            public event EventHandler HandleChanged;
            public event EventHandler WindowClosing;
            public event ClipBoardEventHandler ClipBoardChanged;

            protected override void OnHandleChange()
            {
                base.OnHandleChange();
                if (Handle == IntPtr.Zero) NextWindow = IntPtr.Zero;
                HandleChanged?.Invoke(this, new EventArgs());
            }

            protected override void WndProc(ref Message m)
            {
                switch (m.Msg)
                {
                    case WM_DRAWCLIPBOARD:
                    {
                        var dataObject = Clipboard.GetDataObject();
                        var format = GetDataFormat(dataObject);
                        var text = GetClipboardData(dataObject, format);
                        _ = DateTime.Now;
                        var eventArgs = new ClipboardChangedEventArgs
                        {
                            ClipboardData = dataObject,
                            Text = text,
                            TimeStamp = DateTime.Now,
                            ClipboardDataFormat = format
                        };
                        ClipBoardChanged?.Invoke(eventArgs);
                        SendMessage(NextWindow, (uint) m.Msg, m.WParam, m.LParam);
                        break;
                    }
                    case WM_CHANGECBCHAIN:
                        if (m.WParam == NextWindow)
                            NextWindow = m.LParam;
                        else
                            SendMessage(NextWindow, (uint) m.Msg, m.WParam, m.LParam);
                        break;
                    default:
                        if (m.Msg == WM_DESTROY && WindowClosing != null)
                        {
                            ClipBoardChanged = null;
                            WindowClosing(this, new EventArgs());
                        }

                        base.WndProc(ref m);
                        break;
                }
            }
        }
    }
}
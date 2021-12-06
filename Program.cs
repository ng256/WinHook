using System;
using System.Windows.Forms;

namespace WinHook
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            if (DuplicateLaunch.Default.Detect())
                NamedEvent.Deafult.Set();
            else
                Application.Run(new WinHookContext());
        }
    }
}
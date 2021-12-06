using System;
using System.Threading;

namespace WinHook
{
    public class DuplicateLaunch : IDisposable
    {
        private const string NAME = "FEF01A7A-E82F-45DD-9A92-3EB59C456D2E";
        public static readonly DuplicateLaunch Default = new DuplicateLaunch(NAME);
        private Mutex _mutex;
        private readonly string _name;

        public DuplicateLaunch(string name)
        {
            _name = name;
        }

        public void Dispose()
        {
            _mutex?.Dispose();
        }

        public bool Detect()
        {
            try
            {
                _mutex = Mutex.OpenExisting(_name);
                return true;
            }
            catch
            {
                _mutex = new Mutex(true, _name);
                return false;
            }
        }
    }
}
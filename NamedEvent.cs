using System;
using System.Runtime.InteropServices;
using System.Security;
using static WinHook.Kernel32;

namespace WinHook
{
    [SecurityCritical]
    public class NamedEvent
    {
        private const string NAME = "47622A58-54A1-4842-8AD9-80F475524B55";
        private readonly string _eventName;
        private IntPtr _handle;
        private readonly IntPtr _attributes = IntPtr.Zero;
        private readonly bool _manualReset;
        private readonly bool _initialState;

        public const int WaitForInfinite = -1;  

        public static readonly NamedEvent Deafult = new NamedEvent(NAME);

        public NamedEvent(string eventName) : this(eventName, false) { }

        public NamedEvent(string eventName, bool manualReset)
        {
            _eventName = eventName;
            _manualReset = manualReset;
            _initialState = false;
        }

        public bool Wait(int timeoutInSecs = WaitForInfinite)
        {
            try
            {
                _handle = CreateEvent(_attributes, _manualReset, _initialState, _eventName);
                int rc = WaitForSingleObject(_handle, timeoutInSecs * 1000);
                return rc == 0;
            }
            finally
            {
                if (_handle != IntPtr.Zero) CloseHandle(_handle);
            }
        }

        public bool Pulse()
        {
            try
            {
                _handle = CreateEvent(_attributes, _manualReset, _initialState, _eventName);
                PulseEvent(_handle);
                return _handle != IntPtr.Zero;
            }
            finally
            {
                if (_handle != IntPtr.Zero) CloseHandle(_handle);
            }
        }

        public void Set()
        {
            try
            {
                _handle = CreateEvent(_attributes, _manualReset, _initialState, _eventName);
                SetEvent(_handle);
            }
            finally
            {
                if (_handle != IntPtr.Zero) CloseHandle(_handle);
            }
        }

        public void Reset()
        {
            try
            {
                _handle = CreateEvent(_attributes, _manualReset, _initialState, _eventName);
                ResetEvent(_handle);
            }
            finally
            {
                if (_handle != IntPtr.Zero) CloseHandle(_handle);
            }
        }

        public static bool Wait(int timeoutInSecs, string name) 
            => new NamedEvent(name).Wait(timeoutInSecs);

        public static bool Pulse(string name) => new NamedEvent(name).Pulse();

        public static void Set(string name) => new NamedEvent(name).Set();

        public static void Reset(string name) => new NamedEvent(name).Reset();
    }
}
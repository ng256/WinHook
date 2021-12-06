using System;

namespace WinHook
{
	public struct WindowInfo
	{
		public IntPtr Handle { get; set; }
        public string Text { get; set; }
        public ProcessInfo ProcessInfo { get; set; }

        public static bool operator ==(WindowInfo w1, WindowInfo w2)
		{
			return w1.Equals(w2);
		}

		public static bool operator !=(WindowInfo w1, WindowInfo w2)
		{
			return !w1.Equals(w2);
		}

		public override bool Equals(object obj)
		{
			if (obj is WindowInfo)
			{
				WindowInfo windowInfo = (WindowInfo)obj;
				if (Handle == windowInfo.Handle)
				{
					return Text == windowInfo.Text;
				}
				return false;
			}
			return base.Equals(obj);
		}
	}
}

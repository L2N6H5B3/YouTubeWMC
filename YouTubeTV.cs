using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YouTubeWMC {
    public partial class YouTubeTV : Form {

        // Create variable to hold YouTube TV URL
        private static string youtubeTvUrl = "https://youtube.com/tv";
        // Create variable to hold User Agent
        private static string userAgent = "Mozilla/5.0 (SMART-TV; Linux; Tizen 4.0.0.2) AppleWebkit/605.1.15 (KHTML, like Gecko) SamsungBrowser/9.2 TV Safari/605.1.15";

        public YouTubeTV() {

            // Initialise
            InitializeComponent();

            // Configure Form Sizing
            this.Bounds = Screen.PrimaryScreen.Bounds;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.chromiumWebBrowser1.Size = new System.Drawing.Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

            // Create CefSharp Settings
            var settings = new CefSettings();
            // Set Background Colour
            settings.BackgroundColor = 0xff000000;
            // Set User Agent
            settings.UserAgent = userAgent;
            // Set Browser Cache Path
            settings.CachePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\YouTubeWMC\\WebCache";
            // Initialise CefSharp
            Cef.Initialize(settings);
            // Set KeyboardHandler
            chromiumWebBrowser1.KeyboardHandler = new KeyboardHandler();
            // Set YouTube TV Url
            chromiumWebBrowser1.Load(youtubeTvUrl);
        }
    }

    public class KeyboardHandler : IKeyboardHandler {

        protected Timer wmcTimer = new Timer() {
            Interval = 100
        };
        
        public KeyboardHandler() {
            // Add Timer Event Handler
            wmcTimer.Tick += WmcTimer_Tick;
        }

        private void WmcTimer_Tick(object sender, EventArgs e) {
            // Disable Timer
            wmcTimer.Enabled = false;
        }

        public bool OnKeyEvent(IWebBrowser browserControl, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey) {
            return false;
        }

        public bool OnPreKeyEvent(IWebBrowser browserControl, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey, ref bool isKeyboardShortcut) {

            const int WM_SYSKEYDOWN = 0x104;
            const int WM_KEYDOWN = 0x100;
            const int WM_KEYUP = 0x101;
            const int WM_SYSKEYUP = 0x105;
            const int WM_CHAR = 0x102;
            const int WM_SYSCHAR = 0x106;
            const int VK_TAB = 0x9;

            const int RC6_BACK_WMC = 0xA6;

            bool result = false;

            isKeyboardShortcut = false;

            // Don't deal with TABs by default:
            // TODO: Are there any additional ones we need to be careful of?
            // i.e. Escape, Return, etc...?
            if (windowsKeyCode == VK_TAB) {
                return result;
            }

            if (windowsKeyCode == RC6_BACK_WMC) {
                // If the timer is enabled, don't process a second keystroke
                if (!wmcTimer.Enabled) {
                    // Create KeyDownEvent
                    KeyEvent keyDownEvent = new KeyEvent();
                    keyDownEvent.WindowsKeyCode = (int)Keys.Back;
                    keyDownEvent.FocusOnEditableField = true;
                    keyDownEvent.IsSystemKey = false;
                    keyDownEvent.Type = KeyEventType.RawKeyDown;
                    browser.GetHost().SendKeyEvent(keyDownEvent);
                    // Delay for 50ms
                    Task.Delay(50).Wait();
                    // Create KeyCharEvent
                    KeyEvent keyCharEvent = new KeyEvent();
                    keyCharEvent.WindowsKeyCode = (int)Keys.Back;
                    keyCharEvent.FocusOnEditableField = true;
                    keyCharEvent.IsSystemKey = false;
                    keyCharEvent.Type = KeyEventType.Char;
                    browser.GetHost().SendKeyEvent(keyCharEvent);
                    // Delay for 50ms
                    Task.Delay(50).Wait();
                    // Create KeyUpEvent
                    KeyEvent keyUpEvent = new KeyEvent();
                    keyUpEvent.WindowsKeyCode = (int)Keys.Back;
                    keyUpEvent.FocusOnEditableField = true;
                    keyUpEvent.IsSystemKey = false;
                    keyUpEvent.Type = KeyEventType.KeyUp;
                    browser.GetHost().SendKeyEvent(keyUpEvent);
                    // Enable the Timer
                    wmcTimer.Enabled = true;
                }
                // Return true as we have effectively handled the keystroke
                return true;
            }

            Control control = browserControl as Control;
            int msgType = 0;
            switch (type) {
                case KeyType.RawKeyDown:
                    if (isSystemKey) {
                        msgType = WM_SYSKEYDOWN;
                    } else {
                        msgType = WM_KEYDOWN;
                    }
                    break;
                case KeyType.KeyUp:
                    if (isSystemKey) {
                        msgType = WM_SYSKEYUP;
                    } else {
                        msgType = WM_KEYUP;
                    }
                    break;
                case KeyType.Char:
                    if (isSystemKey) {
                        msgType = WM_SYSCHAR;
                    } else {
                        msgType = WM_CHAR;
                    }
                    break;
                default:
                    Trace.Assert(false);
                    break;
            }

            // We have to adapt from CEF's UI thread message loop to our fronting WinForm control here.
            // So, we have to make some calls that Application.Run usually ends up handling for us:
            PreProcessControlState state = PreProcessControlState.MessageNotNeeded;
            // We can't use BeginInvoke here, because we need the results for the return value
            // and isKeyboardShortcut. In theory this shouldn't deadlock, because
            // atm this is the only synchronous operation between the two threads.
            control.Invoke(new Action(() => {
                Message msg = new Message() { HWnd = control.Handle, Msg = msgType, WParam = new IntPtr(windowsKeyCode), LParam = new IntPtr(nativeKeyCode) };

                // First comes Application.AddMessageFilter related processing:
                // 99.9% of the time in WinForms this doesn't do anything interesting.
                bool processed = Application.FilterMessage(ref msg);
                if (processed) {
                    state = PreProcessControlState.MessageProcessed;
                } else {
                    // Next we see if our control (or one of its parents)
                    // wants first crack at the message via several possible Control methods.
                    // This includes things like Mnemonics/Accelerators/Menu Shortcuts/etc...
                    state = control.PreProcessControlMessage(ref msg);
                }
            }));
            if (state == PreProcessControlState.MessageNeeded) {
                // TODO: Determine how to track MessageNeeded for OnKeyEvent.
                isKeyboardShortcut = true;
            } else if (state == PreProcessControlState.MessageProcessed) {
                // Most of the interesting cases get processed by PreProcessControlMessage.
                result = true;
            }
            Debug.WriteLine(String.Format("OnPreKeyEvent: KeyType: {0} 0x{1:X} Modifiers: {2}", type, windowsKeyCode, modifiers));
            Debug.WriteLine(String.Format("OnPreKeyEvent PreProcessControlState: {0}", state));
            return result;
        }
    }
}


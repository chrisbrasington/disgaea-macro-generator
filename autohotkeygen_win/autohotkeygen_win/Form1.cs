using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace autohotkeygen_win
{
    public partial class Form1 : Form
    {
        // https://stackoverflow.com/questions/15413172/capture-a-keyboard-keypress-in-the-background
        // DLL libraries used to manage hotkeys
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        [DllImportAttribute("User32.dll")]
        private static extern IntPtr SetForegroundWindow(int hWnd);
        

        //const int MYACTION_HOTKEY_ID = 1;

        /// <summary>
        /// find window by name (starts with)
        /// </summary>
        /// <param name="windowName"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        private static bool FindWindow(string windowName, out IntPtr handle)
        {
            foreach (Process pList in Process.GetProcesses())
            {
                if (pList.MainWindowTitle.ToUpper().StartsWith(windowName))
                {
                    // ignore this come on..
                    if (pList.MainWindowTitle.ToLower().Contains("discord"))
                    {
                        continue;
                    }

                    handle = pList.MainWindowHandle;

                    Console.WriteLine($"Found: {pList.MainWindowTitle}");
                    Console.WriteLine($"Handle: {handle}");

                    return true;
                }
            }
            handle = IntPtr.Zero;
            return false;
        }

        //public static Dictionary<int, Keys> Mappings = new Dictionary<int, Keys>();
        public static HashSet<Keys> Mappings = new HashSet<Keys>();

        public Form1()
        {
            InitializeComponent();
            
            AllocConsole();

            IntPtr gameHandle = IntPtr.Zero;
            FindWindow("ELDEN RING", out gameHandle);
            //FindWindow("DISGAEA", out gameHandle);

            Mappings.Add(Keys.W);
            Mappings.Add(Keys.A);
            Mappings.Add(Keys.S);
            Mappings.Add(Keys.D);
            Mappings.Add(Keys.Escape);
            Mappings.Add(Keys.Enter);

            foreach(Keys key in Mappings)
            {
                //RegisterHotKey(this.Handle, (int)key, 0, (int)key);
                RegisterHotKey(this.Handle, (int)key, 0, (int)key);
                //RegisterHotKey(gameHandle, (int)key, 0, (int)key);
            }

            if(gameHandle != IntPtr.Zero)
            {
                SetForegroundWindow((int)gameHandle);

                //System.Threading.Thread.Sleep(1000);
                //Send("ESC");
            }

            Console.WriteLine("GO!");
        }

        private void Send(string key)
        {
            Console.WriteLine($"Sendkey: {key}");
                
            SendKeys.SendWait($"{{{key}}}");
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            //if (m.Msg == 0x0312 && m.WParam.ToInt32() == MYACTION_HOTKEY_ID)
            if (m.Msg == 0x0312 && Mappings.Contains((Keys)m.WParam.ToInt32()))
            {
                Keys data = (Keys)(m.WParam.ToInt32());
                    // My hotkey has been typed

                // Do what you want here
                // ...
                Console.WriteLine($"key press: {(Keys)m.WParam.ToInt32()}");

                //SendKeys.Send(data.ToString());
                //Send("ESC");

            }
            base.WndProc(ref m);
        }
    }
}
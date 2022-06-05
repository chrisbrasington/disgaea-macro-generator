using System.Diagnostics;
using System.Runtime.InteropServices; //required for dll import
using System.Windows.Forms; // https://stackoverflow.com/a/70466224

namespace Generator
{
    internal class Program
    {
        // https://stackoverflow.com/questions/15413172/capture-a-keyboard-keypress-in-the-background
        // DLL libraries used to manage hotkeys
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        const int MYACTION_HOTKEY_ID = 1;

        public static int wait = 100;
        public static string sleepCommand = $"sleep, {wait}";

        public static Dictionary<string, List<string>> DefineMappings()
        {
            Dictionary<string, List<string>> mappings = new Dictionary<string, List<string>>();
            mappings.Add("UP", new List<string>() { "W" });
            mappings.Add("DOWN", new List<string>() { "S" });
            mappings.Add("LEFT", new List<string>() { "A" });
            mappings.Add("RIGHT", new List<string>() { "D" });
            mappings.Add("ATTACK", new List<string>() { "ENTER", "S", "ENTER", "ENTER" });
            mappings.Add("ATTACK2", new List<string>() { "ENTER", "S", "ENTER", "S", "ENTER" });
            mappings.Add("ENDTURN", new List<string>() { "I", "S", "ENTER", "ENTER" });

            return mappings;
        }

        public static void AddKeyPress(ref string output, string value)
        {
            output += $"Send, {{{value} down}}\n";
            output += $"{sleepCommand}\n";
            output += $"Send, {{{value} up}}\n";
            output += $"{sleepCommand}\n";
        }

        public static void Generate()
        {
            string output = "";
            Dictionary<string, List<string>> mappings = DefineMappings();

            foreach (string line in File.ReadLines("..\\..\\..\\input.txt"))
            {
                // comment
                if (line.StartsWith(";"))
                {
                    Console.WriteLine(line);
                    continue;
                }
                if (line.StartsWith("STARTCOMMAND"))
                {
                    Console.WriteLine("START COMMAND ~~~");
                    output += line.Split("STARTCOMMAND ")[1];
                    output += "\n";
                    continue;
                }
                if (line.StartsWith("ENDCOMMAND"))
                {
                    Console.WriteLine("END COMMAND ~~~");
                    output += "return\n\n";
                    continue;
                }

                if (mappings.ContainsKey(line))
                {
                    Console.WriteLine($"{line} ~~~");
                    foreach (string value in mappings[line])
                    {
                        Console.WriteLine($"\t{value}");
                        AddKeyPress(ref output, value);
                    }
                }
                else
                {
                    Console.WriteLine(line);
                    AddKeyPress(ref output, line);
                }
            }

            //System.Console.WriteLine(output);

            using StreamWriter file = new StreamWriter("output.ahk");
            {
                file.WriteLine(output);
            }

            string outputFile = Path.Combine(Environment.CurrentDirectory, "output.ahk");
            Console.WriteLine(outputFile);

            string autoHotKey = "C:\\Program Files\\AutoHotkey\\AutoHotkey.exe";

            string runCommand = $"\"{autoHotKey}\" \"{outputFile}\"";

            System.Diagnostics.Process.Start(runCommand);


        }

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



        static void Main(string[] args)
        {
            IntPtr gameHandle = IntPtr.Zero;
            FindWindow("DISGAEA", out gameHandle);

            RegisterHotKey(gameHandle, MYACTION_HOTKEY_ID, 6, (int)Keys.W);


        }


        //protected override void WndProc(ref Message m)
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == 0x0312 && m.WParam.ToInt32() == MYACTION_HOTKEY_ID)
            {
                // My hotkey has been typed

                // Do what you want here
                // ...
            }
            base.WndProc(ref m);
        }
    }
}

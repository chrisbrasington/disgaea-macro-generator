namespace Generator
{
    internal class Program
    {
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
                if(line.StartsWith(";"))
                {
                    Console.WriteLine(line);
                    continue;
                }
                if(line.StartsWith("STARTCOMMAND"))
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

        static void Main(string[] args)
        {
            string samplePath = "..\\..\\..\\sample\\elden1.jpg";

            Recognition.RecognitionEngine engine = new Recognition.RecognitionEngine();

            engine.Run(samplePath);
        }
    }
}

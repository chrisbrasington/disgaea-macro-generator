using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tesseract;

namespace Recognition
{
    internal class RecognitionEngine
    {
        public RecognitionEngine()
        { }

        public void Run(string path)
        {
            string tessData = "..\\..\\..\\tessdata";
            
            using (var engine = new TesseractEngine(tessData, "eng", EngineMode.Default))
            {
                using(var image = Pix.LoadFromFile(path))
                {
                    Console.WriteLine($"Image loaded: {path}");
                    using(var page = engine.Process(image))
                    {
                        var text = page.GetText();
                        Console.WriteLine(text);
                    }
                }
            }
        }
    }
}

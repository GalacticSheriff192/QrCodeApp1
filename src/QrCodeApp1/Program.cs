using SkiaSharp;
using Net.Codecrete.QrCodeGenerator;
using System;

namespace QrCodeApp1
{
    class Program
    {
        static void Main()
        {
            // asks for text
            Console.WriteLine("Text: ");

            // gets input for text
            var text = Console.ReadLine();

            // asks for name
            Console.WriteLine("Name: ");

            // gets input for name
            var name = Console.ReadLine() + ".png";

            // converts text into symbol
            var code = QrCode.EncodeText(text, QrCode.Ecc.Medium);

            // saves as png
            code.SaveAsPng(name, scale: 10, border: 4);

            // tells the result 
            Console.WriteLine("Saved.");
        }
    }
}

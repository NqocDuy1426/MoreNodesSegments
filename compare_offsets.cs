using System;
using System.IO;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";
        using (var fs = File.OpenRead(path))
        {
            Console.WriteLine("At 50308638:");
            fs.Seek(50308638, SeekOrigin.Begin);
            for (int i = 0; i < 16; i++) Console.Write(string.Format("{0:X2} ", fs.ReadByte()));
            Console.WriteLine();

            Console.WriteLine("At 54370268:");
            fs.Seek(54370268, SeekOrigin.Begin);
            for (int i = 0; i < 16; i++) Console.Write(string.Format("{0:X2} ", fs.ReadByte()));
            Console.WriteLine();
        }
    }
}

using System;
using System.IO;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";
        using (var fs = File.OpenRead(path))
        {
            fs.Seek(45760900, SeekOrigin.Begin);
            for (int i = 0; i < 30; i++)
            {
                Console.Write(string.Format("{0:X2} ", fs.ReadByte()));
            }
            Console.WriteLine();
        }
    }
}

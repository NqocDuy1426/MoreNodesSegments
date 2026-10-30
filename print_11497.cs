using System;
using System.IO;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";
        using (var fs = File.OpenRead(path))
        {
            fs.Seek(11497700, SeekOrigin.Begin);
            Console.WriteLine("At 11497700:");
            byte[] buf = new byte[200];
            fs.Read(buf, 0, buf.Length);
            for (int i = 0; i < buf.Length; i++)
            {
                if (i % 16 == 0) Console.Write(string.Format("\n{0:D8}: ", 11497700 + i));
                Console.Write(string.Format("{0:X2} ", buf[i]));
            }
            Console.WriteLine();
        }
    }
}

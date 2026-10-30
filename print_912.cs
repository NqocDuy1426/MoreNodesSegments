using System;
using System.IO;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";
        using (var fs = File.OpenRead(path))
        {
            fs.Seek(900, SeekOrigin.Begin);
            byte[] buf = new byte[64];
            fs.Read(buf, 0, buf.Length);
            for (int i = 0; i < buf.Length; i++)
            {
                if (i % 16 == 0) Console.Write(string.Format("\n{0:D4}: ", 900 + i));
                Console.Write(string.Format("{0:X2} ", buf[i]));
            }
            Console.WriteLine();
        }
    }
}

using System;
using System.IO;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";
        using (var fs = File.OpenRead(path))
        {
            fs.Seek(11497830, SeekOrigin.Begin);
            Console.WriteLine("Tag 1: " + fs.ReadByte());
            Console.WriteLine("Tag 2: " + fs.ReadByte());
            int b0 = fs.ReadByte();
            int b1 = fs.ReadByte();
            int b2 = fs.ReadByte();
            Console.WriteLine(string.Format("Len bytes: {0:X2} {1:X2} {2:X2}", b0, b1, b2));
            int len = ((b0 & 0x7F) << 16) | (b1 << 8) | b2;
            Console.WriteLine("Len: " + len);
            int running = 128;
            for (int i = 0; i < len; i++)
            {
                int diff = fs.ReadByte();
                running = (running + diff) & 0xFF;
                Console.Write((char)running);
            }
            Console.WriteLine();
        }
    }
}

using System;
using System.IO;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";
        using (var fs = File.OpenRead(path))
        {
            fs.Seek(54370268, SeekOrigin.Begin);
            Console.WriteLine("At 54370268:");
            int b0 = fs.ReadByte();
            int b1 = fs.ReadByte();
            Console.WriteLine(string.Format("EndTag bytes: {0:X2} {1:X2} (index={2})", b0, b1, (b0 << 8) | b1));
            
            Console.WriteLine("At 54370270, next 64 bytes:");
            byte[] buf = new byte[64];
            fs.Read(buf, 0, buf.Length);
            for (int i = 0; i < buf.Length; i++)
            {
                if (i % 16 == 0) Console.Write(string.Format("\n{0:D8}: ", 54370270 + i));
                Console.Write(string.Format("{0:X2} ", buf[i]));
            }
            Console.WriteLine();
        }
    }
}

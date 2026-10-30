using System;
using System.IO;

public class DecodeStrAt760
{
    public static void Main()
    {
        string p = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup .crp";
        using (FileStream fs = new FileStream(p, FileMode.Open, FileAccess.Read))
        {
            fs.Seek(760, SeekOrigin.Begin);
            int tag = fs.ReadByte();
            int b0 = fs.ReadByte();
            int b1 = fs.ReadByte();
            int b2 = fs.ReadByte();
            int len = (b0 << 16) | (b1 << 8) | b2;
            Console.WriteLine(string.Format("tag={0:X2}, len={1}", tag, len & ~0x800000));
            int running = 128;
            char[] ch = new char[len & ~0x800000];
            for (int i = 0; i < ch.Length; i++)
            {
                int b = fs.ReadByte();
                running = (running + b) & 0xFF;
                ch[i] = (char)running;
            }
            Console.WriteLine("String at 760: " + new string(ch));
        }
    }
}

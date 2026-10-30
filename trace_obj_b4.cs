using System;
using System.IO;
using System.Collections.Generic;

class TraceB4
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/backup4.crp";
        using (var fs = File.OpenRead(path))
        {
            // Search for containers count in first 2000 bytes
            byte[] buf = new byte[4096];
            fs.Read(buf, 0, buf.Length);
            for (int i = 0; i < buf.Length - 10; i++)
            {
                if (buf[i] == 0 && buf[i+1] == 0 && buf[i+2] == 29 && buf[i+3] == 254 && buf[i+4] == 254)
                {
                    Console.WriteLine("Found containers start at offset " + i);
                    fs.Seek(i, SeekOrigin.Begin);
                    Run(fs);
                    return;
                }
            }
        }
    }

    static void Run(Stream fs)
    {
        int b0 = fs.ReadByte(); int b1 = fs.ReadByte(); int b2 = fs.ReadByte();
        int count = (b0 << 16) | (b1 << 8) | b2;
        Console.WriteLine("Containers count: " + count);

        List<string> typeList = new List<string>();
        List<string> objList = new List<string>();
        objList.Add("SimulationManager+Data"); // index 0

        for (int i = 0; i < count; i++)
        {
            long pos = fs.Position;
            int tag = fs.ReadByte();
            if (tag == 254)
            {
                string t = ReadSharedType(fs, typeList);
                objList.Add(t);
                Console.WriteLine(string.Format("  [{0}] at {1}: {2}", i + 1, pos, t));
            }
            else if (tag != 255)
            {
                int idx = (tag << 16) | (fs.ReadByte() << 8) | fs.ReadByte();
                objList.Add("Ref to " + idx);
                Console.WriteLine(string.Format("  [{0}] at {1}: Ref to {2}", i + 1, pos, idx));
            }
        }
        Console.WriteLine("Containers array finished at offset " + fs.Position);
    }

    static string ReadUniqueString(Stream s)
    {
        int b0 = s.ReadByte();
        if (b0 == 255) return null;
        int b1 = s.ReadByte();
        int b2 = s.ReadByte();
        int len = (b0 << 16) | (b1 << 8) | b2;
        int running = 128;
        if ((len & 0x800000) != 0)
        {
            len &= ~0x800000;
            char[] c = new char[len];
            for (int i = 0; i < len; i++) { int b = s.ReadByte(); running = (running + b) & 0xFF; c[i] = (char)running; }
            return new string(c);
        }
        else
        {
            char[] c = new char[len];
            for (int i = 0; i < len; i++) { int b = (s.ReadByte() << 8) | s.ReadByte(); running = (running + b) & 0xFFFF; c[i] = (char)running; }
            return new string(c);
        }
    }

    static string ReadSharedType(Stream s, List<string> typeList)
    {
        int tag = s.ReadByte();
        if (tag == 255) return null;
        if (tag == 254)
        {
            string name = ReadUniqueString(s);
            typeList.Add(name);
            return name;
        }
        int idx = (tag << 8) | s.ReadByte();
        return typeList[idx];
    }
}

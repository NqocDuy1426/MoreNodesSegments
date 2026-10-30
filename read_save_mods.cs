using System;
using System.IO;
using System.Collections.Generic;

class ReadSaveMods
{
    static void Main()
    {
        string[] files = { "backup4.crp", "backup5.crp" };
        foreach (var f in files)
        {
            string p = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/" + f;
            if (!File.Exists(p)) continue;
            Console.WriteLine("================ " + f + " ================");
            using (var fs = File.OpenRead(p))
            {
                fs.Seek(370, SeekOrigin.Begin);
                ReadUInt16(fs); ReadUInt16(fs);
                List<string> typeList = new List<string>();
                List<string> objList = new List<string>();
                ReadObject(fs, typeList, objList);
                Console.WriteLine("City: " + ReadUniqueString(fs));
                Console.WriteLine("Save: " + ReadUniqueString(fs));
                ReadUniqueString(fs); ReadUniqueString(fs);
                fs.ReadByte(); fs.ReadByte();
                ReadLong64(fs);
                Console.WriteLine("Date: " + ReadUniqueString(fs));
                int modCount = ReadInt32(fs);
                Console.WriteLine("Mod count: " + modCount);
                for (int i = 0; i < modCount; i++)
                {
                    string m = ReadUniqueString(fs);
                    int en = fs.ReadByte();
                    if (m.Contains("Network") || m.Contains("Nodes") || m.Contains("Renew") || m.Contains("MNC") || m.Contains("More"))
                    {
                        Console.WriteLine("  " + m + " (enabled=" + en + ")");
                    }
                }
            }
        }
    }

    static uint ReadUInt16(Stream s) { return (uint)((s.ReadByte() << 8) | s.ReadByte()); }
    static uint ReadUInt32(Stream s) { return (uint)((s.ReadByte() << 24) | (s.ReadByte() << 16) | (s.ReadByte() << 8) | s.ReadByte()); }
    static int ReadInt32(Stream s) { return (int)ReadUInt32(s); }
    static long ReadLong64(Stream s) { long hi = ReadUInt32(s); long lo = ReadUInt32(s); return (hi << 32) | lo; }

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

    static string ReadObject(Stream s, List<string> typeList, List<string> objList)
    {
        int tag = s.ReadByte();
        if (tag == 255) return null;
        if (tag == 254)
        {
            string type = ReadSharedType(s, typeList);
            objList.Add(type);
            return type;
        }
        int idx = (tag << 16) | (s.ReadByte() << 8) | s.ReadByte();
        if (idx < objList.Count) return objList[idx];
        return "Ref #" + idx;
    }
}

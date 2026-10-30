using System;
using System.IO;
using System.Collections.Generic;

public class WalkSaveObjects
{
    public static void Main()
    {
        string savesDir = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves";
        string save1 = Path.Combine(savesDir, "backup4.crp");
        string save2 = Path.Combine(savesDir, "QuangNinh backup2.crp");

        Walk(save1);
        Console.WriteLine("\n--------------------------------------------------\n");
        Walk(save2);
    }

    static void Walk(string path)
    {
        Console.WriteLine("Walking: " + Path.GetFileName(path));
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            fs.Seek(370, SeekOrigin.Begin); // Data asset offset

            // 1. Read simulation metadata version
            uint v0 = ReadUInt16(fs);
            uint v1 = ReadUInt16(fs);
            Console.WriteLine(string.Format("SimMetaData version: {0}.{1}", v0, v1));

            // Read root object (SimulationMetaData)
            List<string> typeList = new List<string>();
            List<string> objectList = new List<string>();

            string rootType = ReadObject(fs, typeList, objectList);
            Console.WriteLine("Root 1: " + rootType);

            // SimulationMetaData deserializes inline:
            // Let's see what SimulationMetaData.Deserialize reads, or where SimulationManager.Data starts
        }
    }

    static uint ReadUInt16(Stream stream)
    {
        int b0 = stream.ReadByte();
        int b1 = stream.ReadByte();
        return (uint)((b0 << 8) | b1);
    }

    static string ReadObject(Stream stream, List<string> typeList, List<string> objectList)
    {
        int tag = stream.ReadByte();
        if (tag == 255) return null;
        if (tag == 254)
        {
            string type = ReadSharedType(stream, typeList);
            objectList.Add(type);
            return type;
        }
        else
        {
            int idx = (tag << 16) | (stream.ReadByte() << 8) | stream.ReadByte();
            if (idx < objectList.Count) return objectList[idx];
            return "Ref #" + idx;
        }
    }

    static string ReadSharedType(Stream stream, List<string> typeList)
    {
        int tag = stream.ReadByte();
        if (tag == 255) return null;
        if (tag == 254)
        {
            string name = ReadUniqueString(stream);
            typeList.Add(name);
            return name;
        }
        else
        {
            int idx = (tag << 8) | stream.ReadByte();
            if (idx < typeList.Count) return typeList[idx];
            return "TypeRef #" + idx;
        }
    }

    static string ReadUniqueString(Stream stream)
    {
        int b0 = stream.ReadByte();
        if (b0 == 255) return null;
        int b1 = stream.ReadByte();
        int b2 = stream.ReadByte();
        int len = (b0 << 16) | (b1 << 8) | b2;
        int running = 128;
        if ((len & 0x800000) != 0)
        {
            len &= ~0x800000;
            char[] chars = new char[len];
            for (int i = 0; i < len; i++)
            {
                int b = stream.ReadByte();
                running = (running + b) & 0xFF;
                chars[i] = (char)running;
            }
            return new string(chars);
        }
        else
        {
            char[] chars = new char[len];
            for (int i = 0; i < len; i++)
            {
                int u0 = stream.ReadByte();
                int u1 = stream.ReadByte();
                int val = (u0 << 8) | u1;
                running = (running + val) & 0xFFFF;
                chars[i] = (char)running;
            }
            return new string(chars);
        }
    }
}

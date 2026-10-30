using System;
using System.IO;
using System.Collections.Generic;

public class ParseSimMeta
{
    public static void Main()
    {
        string savesDir = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves";
        string save1 = Path.Combine(savesDir, "QuangNinh backup .crp");
        string save2 = Path.Combine(savesDir, "QuangNinh backup2.crp");

        InspectSave(save1);
        Console.WriteLine("\n==================================================\n");
        InspectSave(save2);
    }

    static void InspectSave(string path)
    {
        Console.WriteLine("Inspecting Save: " + Path.GetFileName(path));
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            fs.Seek(370, SeekOrigin.Begin); // Data asset offset

            uint v0 = ReadUInt16(fs);
            uint v1 = ReadUInt16(fs);
            uint version = (v0 << 16) | v1;
            if (v1 <= 322 && v0 <= 6) version = v1;

            List<string> typeList = new List<string>();
            List<string> objectList = new List<string>();

            string rootType = ReadObject(fs, typeList, objectList);
            string cityName = ReadUniqueString(fs);
            string saveName = ReadUniqueString(fs);
            string mapTheme = ReadUniqueString(fs);
            string env = ReadUniqueString(fs);
            bool invertTraffic = fs.ReadByte() != 0;
            bool disableAch = fs.ReadByte() != 0;
            long currentTick = ReadLong64(fs);
            string currentDateTime = ReadUniqueString(fs);
            int modCount = ReadInt32(fs);
            for (int i = 0; i < modCount; i++)
            {
                ReadUniqueString(fs);
                fs.ReadByte();
            }
            byte startingTiles = (byte)fs.ReadByte();
            byte milestones = (byte)fs.ReadByte();
            long timestamp = ReadLong64(fs);
            float camHeight = ReadFloat(fs);
            uint tempDay = ReadUInt32(fs);
            uint tempNight = ReadUInt32(fs);
            uint rainDay = ReadUInt32(fs);
            uint rainNight = ReadUInt32(fs);

            // SimulationManager.Data DataSerializer
            uint sm_v0 = ReadUInt16(fs);
            uint sm_v1 = ReadUInt16(fs);
            uint sm_version = (sm_v0 << 16) | sm_v1;
            if (sm_v1 <= 322 && sm_v0 <= 6) sm_version = sm_v1;

            List<string> smTypeList = new List<string>();
            List<string> smObjList = new List<string>();
            string smRoot = ReadObject(fs, smTypeList, smObjList);
            Console.WriteLine("SimulationManager.Data root: " + smRoot);

            // 1. tickIndex (ver >= 84)
            uint tickIndex = ReadUInt32(fs);
            // 2. frameIndex (ver >= 5)
            uint frameIndex = ReadUInt32(fs);
            // 3. buildIndex (ver >= 13)
            uint buildIndex = ReadUInt32(fs);
            // 4. randomizer seed (ver >= 10)
            ulong seed = (ulong)ReadLong64(fs);
            // 5. dayTimeOffsetFrames (ver >= 212)
            uint dayTimeOffset = ReadUInt32(fs);
            // 6. buildIndexHistory (ver >= 164)
            for (int i = 0; i < 32; i++)
            {
                ReadUInt32(fs);
            }

            Console.WriteLine(string.Format("Reached ReadObjectArray at position: {0}", fs.Position));

            // Now ReadObjectArray<IDataContainer>():
            int bArr0 = fs.ReadByte();
            if (bArr0 == 255)
            {
                Console.WriteLine("Object array is null");
                return;
            }
            int bArr1 = fs.ReadByte();
            int bArr2 = fs.ReadByte();
            int arrCount = (bArr0 << 16) | (bArr1 << 8) | bArr2;
            Console.WriteLine("Array count of IDataContainers: " + arrCount);

            for (int i = 0; i < arrCount; i++)
            {
                string objType = ReadObject(fs, smTypeList, smObjList);
                Console.WriteLine(string.Format("  Container [{0}]: {1}", i, objType));
            }

            Console.WriteLine("\nAll registered objects in save:");
            for (int i = 0; i < smObjList.Count; i++)
            {
                Console.WriteLine(string.Format("  [{0}] {1}", i, smObjList[i]));
            }
        }
    }

    static uint ReadUInt16(Stream stream)
    {
        int b0 = stream.ReadByte();
        int b1 = stream.ReadByte();
        return (uint)((b0 << 8) | b1);
    }

    static uint ReadUInt32(Stream stream)
    {
        int b0 = stream.ReadByte();
        int b1 = stream.ReadByte();
        int b2 = stream.ReadByte();
        int b3 = stream.ReadByte();
        return (uint)((b0 << 24) | (b1 << 16) | (b2 << 8) | b3);
    }

    static int ReadInt32(Stream stream)
    {
        return (int)ReadUInt32(stream);
    }

    static long ReadLong64(Stream stream)
    {
        long hi = (long)ReadUInt32(stream);
        long lo = (long)ReadUInt32(stream);
        return (hi << 32) | lo;
    }

    static float ReadFloat(Stream stream)
    {
        byte[] b = new byte[4];
        b[3] = (byte)stream.ReadByte();
        b[2] = (byte)stream.ReadByte();
        b[1] = (byte)stream.ReadByte();
        b[0] = (byte)stream.ReadByte();
        return BitConverter.ToSingle(b, 0);
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

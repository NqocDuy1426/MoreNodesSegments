using System;
using System.IO;
using System.Collections.Generic;

public class TracePMInSave
{
    public static void Main()
    {
        string savePath = Environment.GetEnvironmentVariable("HOME") + "/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";
        using (FileStream fs = new FileStream(savePath, FileMode.Open, FileAccess.Read))
        {
            fs.Seek(370, SeekOrigin.Begin);
            
            // SimulationMetaData
            uint v0 = ReadUInt16(fs);
            uint v1 = ReadUInt16(fs);
            Console.WriteLine(string.Format("SimMetaData version: {0}.{1}", v0, v1));

            List<string> typeList = new List<string>();
            List<string> objList = new List<string>();
            ReadObject(fs, typeList, objList); // SimulationMetaData

            ReadUniqueString(fs); // CityName
            ReadUniqueString(fs); // SaveName
            ReadUniqueString(fs); // MapTheme
            ReadUniqueString(fs); // Environment
            fs.ReadByte(); // InvertTraffic
            fs.ReadByte(); // DisableAchievements
            ReadLong64(fs); // CurrentTick
            ReadUniqueString(fs); // CurrentDateTime
            int modCount = ReadInt32(fs);
            for (int i = 0; i < modCount; i++)
            {
                ReadUniqueString(fs);
                fs.ReadByte();
            }
            fs.ReadByte(); // StartingTiles
            fs.ReadByte(); // Milestones
            ReadLong64(fs); // Timestamp
            ReadFloat(fs); // CamHeight
            ReadUInt32(fs); // TempDay
            ReadUInt32(fs); // TempNight
            ReadUInt32(fs); // RainDay
            ReadUInt32(fs); // RainNight

            Console.WriteLine("Reached SimulationManager.Data at pos: " + fs.Position);

            // SimulationManager.Data
            uint sm_v0 = ReadUInt16(fs);
            uint sm_v1 = ReadUInt16(fs);
            uint sm_ver = (sm_v0 << 16) | sm_v1;
            if (sm_v1 <= 322 && sm_v0 <= 6) sm_ver = sm_v1;
            Console.WriteLine("SimulationManager.Data version: " + sm_ver);

            List<string> smTypeList = new List<string>();
            List<string> smObjList = new List<string>();
            List<string> smStrList = new List<string>();

            ReadObject(fs, smTypeList, smObjList, smStrList); // SimulationManager.Data

            ReadUInt32(fs); // tickIndex
            ReadUInt32(fs); // frameIndex
            ReadUInt32(fs); // buildIndex
            ReadLong64(fs); // randomizer seed
            ReadUInt32(fs); // dayTimeOffsetFrames
            for (int i = 0; i < 32; i++) ReadUInt32(fs); // buildIndexHistory

            Console.WriteLine("Reached dataContainers array at pos: " + fs.Position);
            int b0 = fs.ReadByte();
            int b1 = fs.ReadByte();
            int b2 = fs.ReadByte();
            int count = (b0 << 16) | (b1 << 8) | b2;
            Console.WriteLine("Containers count: " + count);
        }
    }

    static uint ReadUInt16(Stream s) { return (uint)((s.ReadByte() << 8) | s.ReadByte()); }
    static uint ReadUInt32(Stream s) { return (uint)((s.ReadByte() << 24) | (s.ReadByte() << 16) | (s.ReadByte() << 8) | s.ReadByte()); }
    static int ReadInt32(Stream s) { return (int)ReadUInt32(s); }
    static long ReadLong64(Stream s) { long hi = ReadUInt32(s); long lo = ReadUInt32(s); return (hi << 32) | lo; }
    static float ReadFloat(Stream s) { byte[] b = new byte[4]; b[3]=(byte)s.ReadByte(); b[2]=(byte)s.ReadByte(); b[1]=(byte)s.ReadByte(); b[0]=(byte)s.ReadByte(); return BitConverter.ToSingle(b,0); }
    
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

    static string ReadObject(Stream s, List<string> typeList, List<string> objList, List<string> strList = null)
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
        return objList[idx];
    }
}

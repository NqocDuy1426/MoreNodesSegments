using System;
using System.IO;
using System.Collections.Generic;

public class ListAllObjectsInSave3
{
    public static void Main()
    {
        string savePath = Environment.GetEnvironmentVariable("HOME") + "/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup3.crp";
        using (FileStream fs = new FileStream(savePath, FileMode.Open, FileAccess.Read))
        {
            fs.Seek(370, SeekOrigin.Begin);
            
            ReadUInt16(fs); ReadUInt16(fs); // ver
            List<string> typeList = new List<string>();
            List<string> objList = new List<string>();
            ReadObject(fs, typeList, objList); // SimulationMetaData

            for (int i = 0; i < 4; i++) ReadUniqueString(fs);
            fs.ReadByte(); fs.ReadByte(); ReadLong64(fs); ReadUniqueString(fs);
            int modCount = ReadInt32(fs);
            for (int i = 0; i < modCount; i++) { ReadUniqueString(fs); fs.ReadByte(); }
            fs.ReadByte(); fs.ReadByte(); ReadLong64(fs); ReadFloat(fs);
            for (int i = 0; i < 4; i++) ReadUInt32(fs);

            // SimulationManager.Data
            ReadUInt16(fs); ReadUInt16(fs);
            List<string> smTypeList = new List<string>();
            List<string> smObjList = new List<string>();
            ReadObject(fs, smTypeList, smObjList);

            for (int i = 0; i < 5; i++) ReadUInt32(fs);
            for (int i = 0; i < 32; i++) ReadUInt32(fs);

            // ReadObjectArray<IDataContainer>()
            int b0 = fs.ReadByte();
            int b1 = fs.ReadByte();
            int b2 = fs.ReadByte();
            int count = (b0 << 16) | (b1 << 8) | b2;
            Console.WriteLine("Containers count in SimulationManager.Data: " + count);

            for (int i = 0; i < count; i++)
            {
                long pos = fs.Position;
                string objType = ReadObject(fs, smTypeList, smObjList);
                Console.WriteLine(string.Format("  [{0}] {1} (offset {2})", i, objType, pos));
            }
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
        int len = (b0 << 8) | b1;
        if (len == 0) return "";
        byte[] bytes = new byte[len];
        s.Read(bytes, 0, len);
        return System.Text.Encoding.UTF8.GetString(bytes);
    }

    static string ReadObject(Stream s, List<string> typeList, List<string> objList)
    {
        int typeIdx = s.ReadByte();
        if (typeIdx == 254)
        {
            string tName = ReadUniqueString(s);
            typeList.Add(tName);
            return tName;
        }
        else if (typeIdx < typeList.Count)
        {
            return typeList[typeIdx];
        }
        return "UnknownType_" + typeIdx;
    }
}

using System;
using System.IO;
using System.Reflection;
using ColossalFramework.IO;

class Program
{
    static void Main()
    {
        Read("QuangNinh backup .crp", 45530463);
        Read("QuangNinh backup2.crp", 45953494);
    }

    static void Read(string name, int offset)
    {
        string path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/" + name;
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            fs.Seek(offset + 16, SeekOrigin.Begin);
            ConstructorInfo ctor = typeof(DataSerializer).GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)[0];
            DataSerializer ds = (DataSerializer)ctor.Invoke(new object[] { fs, (DataSerializer.Mode)1, false });
            FieldInfo vFld = typeof(DataSerializer).GetField("m_version", BindingFlags.NonPublic | BindingFlags.Instance);
            if (vFld != null) vFld.SetValue(ds, 121034u);

            uint[] flags = new uint[256];
            EncodedArray.UInt rFlags = EncodedArray.UInt.BeginRead(ds);
            for (int i = 1; i < 256; i++) flags[i] = rFlags.Read();
            rFlags.EndRead();
            Console.WriteLine(name + " after flags: " + (fs.Position - offset));

            ushort milestoneCount = (ushort)ds.ReadUInt16();
            Console.WriteLine(name + " after milestoneCount (" + milestoneCount + "): " + (fs.Position - offset));

            EncodedArray.UShort rPrefabs = EncodedArray.UShort.BeginRead(ds);
            for (int i = 1; i < 256; i++) { if (flags[i] != 0) rPrefabs.Read(); }
            for (int i = 0; i < milestoneCount; i++) rPrefabs.Read();
            rPrefabs.EndRead();
            Console.WriteLine(name + " after prefabs: " + (fs.Position - offset));

            for (int i = 0; i < milestoneCount; i++) { ds.ReadInt32(); ds.ReadLong64(); }
            Console.WriteLine(name + " after milestones: " + (fs.Position - offset));

            for (int i = 0; i < 17; i++) ds.ReadUInt16();
            Console.WriteLine(name + " after 17 uint16: " + (fs.Position - offset));

            ushort num = (ushort)ds.ReadUInt16();
            ushort stops = (ushort)ds.ReadUInt16();
            Console.WriteLine(string.Format("{0}: Pos={1}, Line 1 Num={2}, Stops={3}", name, (fs.Position - offset), num, stops));
        }
    }
}

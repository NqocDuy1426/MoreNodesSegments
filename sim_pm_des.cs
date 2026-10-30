using System;
using System.IO;
using System.Reflection;
using ColossalFramework.IO;

public class SimPMDes
{
    public static void Main()
    {
        string savePath = Environment.GetEnvironmentVariable("HOME") + "/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";
        using (FileStream fs = new FileStream(savePath, FileMode.Open, FileAccess.Read))
        {
            // GuideManager end tag is at 45760709. gm_enc len is 16.
            // So PathManager starts at 45760725!
            long startPos = 45760725;
            fs.Seek(startPos, SeekOrigin.Begin);

            var ctor = typeof(DataSerializer).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(Stream), typeof(DataSerializer.Mode), typeof(bool) }, null);
            DataSerializer ds = (DataSerializer)ctor.Invoke(new object[] { fs, DataSerializer.Mode.Memory, false });
            
            var fVer = typeof(DataSerializer).GetField("m_version", BindingFlags.NonPublic | BindingFlags.Instance);
            fVer.SetValue(ds, 121034u);

            Console.WriteLine("Starting PathManager deserialization simulation at pos: " + fs.Position);

            // Test 1: Try length = 262144
            // Test 2: Try length = 524288
            // Let us see what happens with UShort.BeginRead(ds)
            var uShort = EncodedArray.UShort.BeginRead(ds);
            int length = 524288;
            ushort[] simFlags = new ushort[length];
            int active = 0;
            for (int i = 1; i < length; i++)
            {
                simFlags[i] = uShort.Read();
                if (simFlags[i] != 0) active++;
            }
            uShort.EndRead();

            Console.WriteLine(string.Format("After UShort.EndRead(524288): stream pos = {0}, activeCount = {1}", fs.Position, active));

            // Now read active units
            for (int j = 1; j < length; j++)
            {
                if (simFlags[j] != 0)
                {
                    byte pathFindFlags = (byte)ds.ReadUInt8();
                    byte laneTypes = (byte)ds.ReadUInt8();
                    uint vehicleTypes = ds.ReadUInt32();
                    byte positionCount = (byte)ds.ReadUInt8();
                    uint nextPathUnit = ds.ReadUInt24();
                    uint buildIndex = ds.ReadUInt32();
                    float len = ds.ReadFloat();
                    byte speed = (byte)ds.ReadUInt8();
                    for (int k = 0; k < positionCount; k++)
                    {
                        ushort seg = (ushort)ds.ReadUInt16();
                        byte off = (byte)ds.ReadUInt8();
                        byte lane = (byte)ds.ReadUInt8();
                    }
                }
            }

            Console.WriteLine(string.Format("After reading active units (524288): stream pos = {0} (Target was 45760907, diff = {1})", fs.Position, fs.Position - 45760907));
        }
    }
}

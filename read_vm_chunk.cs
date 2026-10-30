using System;
using System.IO;
using System.Reflection;
using ColossalFramework.IO;

class Program
{
    static void Main()
    {
        string p = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";
        using (FileStream fs = new FileStream(p, FileMode.Open, FileAccess.Read))
        {
            fs.Seek(39986662 + 16, SeekOrigin.Begin);

            ConstructorInfo ctor = typeof(DataSerializer).GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)[0];
            DataSerializer ds = (DataSerializer)ctor.Invoke(new object[] { fs, (DataSerializer.Mode)1, false });

            FieldInfo vFld = typeof(DataSerializer).GetField("m_version", BindingFlags.NonPublic | BindingFlags.Instance);
            if (vFld != null) vFld.SetValue(ds, 121034u);

            EncodedArray.UShort flagsReader = EncodedArray.UShort.BeginRead(ds);
            int activeCount = 0;
            int totalRead = 0;
            try
            {
                while (true)
                {
                    ushort flag = flagsReader.Read();
                    if ((flag & 1) != 0) activeCount++; // Flags.Created = 1
                    totalRead++;
                }
            }
            catch
            {
                // end of encoded array
            }
            Console.WriteLine(string.Format("VehicleManager active vehicles: {0} / total read: {1}", activeCount, totalRead));
        }
    }
}

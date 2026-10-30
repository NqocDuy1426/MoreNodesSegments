using System;
using System.IO;
using System.Reflection;
using ColossalFramework.IO;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup .crp";
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            fs.Seek(370, SeekOrigin.Begin); // Asset 0 start
            ConstructorInfo ctor = typeof(DataSerializer).GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)[0];
            DataSerializer ds = (DataSerializer)ctor.Invoke(new object[] { fs, (DataSerializer.Mode)1, false });
            Console.WriteLine("Save Game DataSerializer Version: " + ds.version);
        }
    }
}

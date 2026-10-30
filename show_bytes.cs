using System;
using System.IO;
using System.Reflection;
using ColossalFramework.IO;

class ShowBytes
{
    static void Main()
    {
        var ms = new MemoryStream();
        var ctor = typeof(DataSerializer).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(Stream), typeof(DataSerializer.Mode), typeof(bool) }, null);
        var ds = ctor.Invoke(new object[] { ms, DataSerializer.Mode.File, true });
        MethodInfo mWrite = typeof(DataSerializer).GetMethod("WriteUniqueString");
        mWrite.Invoke(ds, new object[] { "PathManager" });
        byte[] bytes = ms.ToArray();
        Console.WriteLine("PathManager bytes: " + BitConverter.ToString(bytes));
        
        ms.SetLength(0);
        mWrite.Invoke(ds, new object[] { "SimulationManager" });
        Console.WriteLine("SimulationManager bytes: " + BitConverter.ToString(ms.ToArray()));

        ms.SetLength(0);
        mWrite.Invoke(ds, new object[] { "TreeManager" });
        Console.WriteLine("TreeManager bytes: " + BitConverter.ToString(ms.ToArray()));
    }
}

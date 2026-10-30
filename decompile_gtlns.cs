using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string csPath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/";
        AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
        {
            string name = new AssemblyName(args.Name).Name;
            string p = System.IO.Path.Combine(csPath, name + ".dll");
            if (System.IO.File.Exists(p)) return Assembly.LoadFrom(p);
            return null;
        };
        string tmpePath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        Assembly tmpe = Assembly.LoadFrom(tmpePath);
        Type t = tmpe.GetType("TrafficManager.Patch._RoadBaseAI.GetTrafficLightNodeStatePatch");
        MethodInfo m = t.GetMethod("Prefix", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        var mb = m.GetMethodBody();
        byte[] il = mb.GetILAsByteArray();
        Module mod = tmpe.ManifestModule;
        Console.WriteLine("Instructions:");
        // Let's print out all resolved tokens
        for (int i = 0; i < il.Length - 4; i++)
        {
            try
            {
                int tok = BitConverter.ToInt32(il, i);
                MemberInfo mem = mod.ResolveMember(tok);
                if (mem != null)
                {
                    Console.WriteLine(string.Format("Offset {0}: {1} in {2}", i, mem.Name, mem.DeclaringType.FullName));
                }
            }
            catch {}
        }
    }
}

using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string tmpePath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        Assembly asm = Assembly.LoadFrom(tmpePath);
        Type t = asm.GetType("TrafficManager.Manager.Impl.ExtVehicleManager");
        string[] methods = { "OnBeforeLoadData", "OnLevelLoading", "InitAllVehicles" };
        foreach (string name in methods)
        {
            MethodInfo m = t.GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            Console.WriteLine("=== " + name + " ===");
            byte[] il = m.GetMethodBody().GetILAsByteArray();
            Console.WriteLine("IL bytes: " + il.Length);
            for (int i = 0; i < il.Length - 4; i++)
            {
                try
                {
                    int token = BitConverter.ToInt32(il, i);
                    MemberInfo mi = m.Module.ResolveMember(token);
                    if (mi != null) Console.WriteLine(string.Format("  0x{0:X2}: {1}.{2}", i, mi.DeclaringType.Name, mi.Name));
                }
                catch {}
            }
        }
    }
}

using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string tmpePath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        Assembly asm = Assembly.LoadFrom(tmpePath);
        Type t = asm.GetType("TrafficManager.Patch._VehicleAI._CarAI.SimulationStepPatch");
        MethodInfo m = t.GetMethod("Prefix", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        Console.WriteLine("Prefix: " + m);
        byte[] il = m.GetMethodBody().GetILAsByteArray();
        Console.WriteLine("IL length: " + il.Length);
        
        // Print all member calls in Prefix
        Module mod = m.Module;
        for (int i = 0; i < il.Length - 4; i++)
        {
            try
            {
                int token = BitConverter.ToInt32(il, i);
                MemberInfo mi = mod.ResolveMember(token);
                if (mi != null && mi.Name != "op_Implicit")
                {
                    Console.WriteLine(string.Format("Offset 0x{0:X2}: {1}.{2}", i, mi.DeclaringType.Name, mi.Name));
                }
            }
            catch {}
        }
    }
}

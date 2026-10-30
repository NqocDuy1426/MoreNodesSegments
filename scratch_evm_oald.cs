using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string tmpePath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        Assembly asm = Assembly.LoadFrom(tmpePath);
        Type t = asm.GetType("TrafficManager.Manager.Impl.ExtVehicleManager");
        MethodInfo m = t.GetMethod("OnAfterLoadData", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        Console.WriteLine("OnAfterLoadData IL bytes: " + m.GetMethodBody().GetILAsByteArray().Length);
        byte[] il = m.GetMethodBody().GetILAsByteArray();
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

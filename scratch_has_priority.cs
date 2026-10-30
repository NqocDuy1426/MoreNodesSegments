using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string tmpePath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        Assembly asm = Assembly.LoadFrom(tmpePath);
        Type tpm = asm.GetType("TrafficManager.Manager.Impl.TrafficPriorityManager");
        foreach (MethodInfo m in tpm.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            if (m.Name == "HasPriority")
            {
                Console.WriteLine("Method: " + m);
                foreach (var p in m.GetParameters())
                {
                    Console.WriteLine("  " + p.ParameterType + " " + p.Name);
                }
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
}

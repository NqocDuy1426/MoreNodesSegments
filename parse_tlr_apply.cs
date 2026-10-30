using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        Assembly core = Assembly.LoadFrom("MoreNetworkCapacity.Core.dll");
        Type t = core.GetType("MoreNetworkCapacity.Compatibility.TMPETrafficLightRanges");
        MethodInfo m = t.GetMethod("Apply", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        var mb = m.GetMethodBody();
        byte[] il = mb.GetILAsByteArray();
        Module mod = core.ManifestModule;
        Console.WriteLine("Apply IL len: " + il.Length);
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

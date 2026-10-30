using System;
using System.Reflection;
using System.Collections;

class Program
{
    static void Main()
    {
        var asm = Assembly.LoadFrom("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/MoreNetworkCapacity.Core.dll");
        Type t = asm.GetType("MoreNetworkCapacity.Patches.NodeSimulationPhasePatch");
        MethodInfo tm = t.GetMethod("TargetMethods", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        var res = (IEnumerable)tm.Invoke(null, null);
        foreach (MethodBase mb in res)
        {
            Console.WriteLine("Target: " + mb.DeclaringType + "." + mb.Name);
        }
    }
}

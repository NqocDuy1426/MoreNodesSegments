using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string corePath = "/Users/nqocduy/CS_MoreNetworkCapacity_Renew/MoreNetworkCapacity.Core.dll";
        var assembly = AssemblyDefinition.ReadAssembly(corePath);
        var type = assembly.MainModule.GetType("MoreNetworkCapacity.Compatibility.TMPEIndexPatches");
        if (type == null)
        {
            Console.WriteLine("Type null");
            return;
        }
        foreach (var m in type.Methods)
        {
            Console.WriteLine("Method: " + m.ToString());
            if (m.Body != null)
            {
                foreach (var ins in m.Body.Instructions)
                {
                    Console.WriteLine("  " + ins.ToString());
                }
            }
        }
    }
}

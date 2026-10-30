using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/ColossalManaged.dll");
        var ds = asm.MainModule.GetType("ColossalFramework.IO.DataSerializer");
        foreach (var m in ds.Methods)
        {
            if (m.Name == "Deserialize" && m.GenericParameters.Count > 0)
            {
                Console.WriteLine("Method: " + m.FullName);
                foreach (var inst in m.Body.Instructions)
                    Console.WriteLine("  " + inst);
            }
        }
    }
}

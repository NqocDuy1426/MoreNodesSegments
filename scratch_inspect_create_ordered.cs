using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/MoreNetworkCapacity.Core.dll");
        var type = asm.MainModule.Types.First(t => t.FullName == "MoreNetworkCapacity.NetManagerCustomizer");
        foreach (var m in type.Methods)
        {
            if (m.Name.Contains("CreateOrdered"))
            {
                Console.WriteLine("Method: " + m.Name);
                foreach (var inst in m.Body.Instructions)
                    Console.WriteLine("  " + inst);
            }
        }
    }
}

using System;
using System.Linq;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/MoreNetworkCapacity.Core.dll");
        var type = asm.MainModule.Types.First(t => t.FullName == "MoreNetworkCapacity.Capacity");
        Console.WriteLine("Type: " + type.FullName);
        foreach (var m in type.Methods)
        {
            Console.WriteLine("Method: " + m.Name);
            if (m.HasBody)
            {
                foreach (var inst in m.Body.Instructions)
                {
                    Console.WriteLine("  " + inst);
                }
            }
        }
    }
}

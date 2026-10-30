using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/ColossalManaged.dll");
        var ea = asm.MainModule.GetType("ColossalFramework.IO.EncodedArray");
        var uintType = ea.NestedTypes.First(t => t.Name == "UInt");
        foreach (var m in uintType.Methods)
        {
            if (m.Name == "Read")
            {
                Console.WriteLine("EncodedArray.UInt.Read:");
                foreach (var inst in m.Body.Instructions)
                    Console.WriteLine("  " + inst);
            }
        }
    }
}

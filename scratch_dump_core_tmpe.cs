using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/MoreNetworkCapacity.Core.dll");
        var t = asm.MainModule.GetType("MoreNetworkCapacity.Compatibility.TMPEIndexPatches");
        Console.WriteLine("Type: " + t.FullName);
        foreach (var m in t.Methods)
        {
            Console.WriteLine("--- " + m.Name + " ---");
            if (m.Body != null)
            {
                foreach (var inst in m.Body.Instructions)
                {
                    Console.WriteLine("  {0:X4}: {1} {2}", inst.Offset, inst.OpCode, inst.Operand);
                }
            }
        }
    }
}

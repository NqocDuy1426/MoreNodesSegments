using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/MoreNetworkCapacity.Core.dll");
        foreach (var type in asm.MainModule.Types)
        {
            foreach (var m in type.Methods)
            {
                if (m.HasBody)
                {
                    foreach (var inst in m.Body.Instructions)
                    {
                        if (inst.OpCode == OpCodes.Ldstr && inst.Operand.ToString().Contains("Reset network and node-indexed buffers"))
                        {
                            Console.WriteLine("Found in: " + type.FullName + "." + m.Name);
                        }
                    }
                }
            }
        }
    }
}

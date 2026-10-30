using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/CS_MoreNetworkCapacity_Renew/MoreNetworkCapacity.Core.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var type = asm.MainModule.GetType("MoreNetworkCapacity.Patches.GeneratedPatchTargets");
        if (type != null)
        {
            var m = type.Methods[0]; // static ctor or get
            Console.WriteLine("Method: " + m.FullName);
            foreach (var ins in m.Body.Instructions)
            {
                if (ins.OpCode == OpCodes.Ldstr)
                {
                    Console.WriteLine("  " + ins.Operand);
                }
            }
        }
    }
}

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
            var m = type.Methods[0];
            for (int i = 0; i < m.Body.Instructions.Count; i++)
            {
                var ins = m.Body.Instructions[i];
                if (ins.OpCode == OpCodes.Ldstr && (string)ins.Operand == "NetManager")
                {
                    Console.WriteLine("NetManager method: " + m.Body.Instructions[i + 1].Operand);
                }
            }
        }
    }
}

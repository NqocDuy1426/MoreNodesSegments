using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/MoreNetworkCapacity.Core.dll");
        string[] patchTypes = new string[] {
            "MoreNetworkCapacity.Patches.ElectricityManagerSimulationStepPatch",
            "MoreNetworkCapacity.Patches.WaterManagerSimulationStepPatch",
            "MoreNetworkCapacity.Patches.PendingUpdateResetPatch"
        };

        foreach (var pName in patchTypes)
        {
            var t = asm.MainModule.GetType(pName);
            Console.WriteLine($"=== {pName} ===");
            foreach (var m in t.Methods)
            {
                Console.WriteLine($"Method: {m.Name}");
                if (m.HasBody)
                {
                    foreach (var inst in m.Body.Instructions)
                    {
                        Console.WriteLine($"   {inst.Offset:X4}: {inst.OpCode} {inst.Operand}");
                    }
                }
            }
        }
    }
}

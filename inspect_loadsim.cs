using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var lm = asm.MainModule.Types.FirstOrDefault(t => t.Name == "LoadingManager");
        var nested = lm.NestedTypes.FirstOrDefault(t => t.Name.Contains("LoadSimulationData"));
        Console.WriteLine("Nested iterator: " + (nested != null ? nested.FullName : "null"));
        if (nested != null) {
            var moveNext = nested.Methods.FirstOrDefault(m => m.Name == "MoveNext");
            if (moveNext != null) {
                Console.WriteLine("MoveNext found:");
                foreach (var ins in moveNext.Body.Instructions) {
                    if (ins.OpCode == OpCodes.Ldstr || ins.OpCode == OpCodes.Call || ins.OpCode == OpCodes.Callvirt) {
                        Console.WriteLine("  " + ins.ToString());
                    }
                }
            }
        }
    }
}

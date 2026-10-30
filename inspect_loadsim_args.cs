using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var lm = asm.MainModule.Types.FirstOrDefault(t => t.Name == "LoadingManager");
        var nested = lm.NestedTypes.FirstOrDefault(t => t.Name.Contains("LoadSimulationData"));
        var moveNext = nested.Methods.FirstOrDefault(m => m.Name == "MoveNext");
        for (int i = 0; i < moveNext.Body.Instructions.Count; i++) {
            var ins = moveNext.Body.Instructions[i];
            if (ins.OpCode == OpCodes.Call && ins.Operand.ToString().Contains("Managers_UpdateData")) {
                Console.WriteLine("Call at " + ins.Offset);
                for (int j = Math.Max(0, i - 10); j <= i; j++) {
                    Console.WriteLine("  " + moveNext.Body.Instructions[j].ToString());
                }
            }
        }
    }
}

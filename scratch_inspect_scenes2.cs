using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var lm = asm.MainModule.GetType("LoadingManager");
        var t = lm.NestedTypes.FirstOrDefault(x => x.Name.Contains("LoadLevelCoroutine"));
        var moveNext = t.Methods.First(m => m.Name == "MoveNext");
        for (int i = 0; i < moveNext.Body.Instructions.Count; i++) {
            var inst = moveNext.Body.Instructions[i];
            if (inst.OpCode == OpCodes.Ldstr && (inst.Operand.ToString().Contains("Properties") || inst.Operand.ToString().Contains("Weather"))) {
                for (int j = Math.Max(0, i - 15); j <= Math.Min(moveNext.Body.Instructions.Count - 1, i + 25); j++) {
                    Console.WriteLine(moveNext.Body.Instructions[j]);
                }
                Console.WriteLine("-------------------");
            }
        }
    }
}


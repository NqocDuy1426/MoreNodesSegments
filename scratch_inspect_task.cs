using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var lm = asm.MainModule.GetType("LoadingManager");
        var t = lm.NestedTypes.FirstOrDefault(x => x.Name.Contains("LoadLevelCoroutine"));
        var mn = t.Methods.First(m => m.Name == "MoveNext");
        for (int i = 0; i < mn.Body.Instructions.Count; i++) {
            var inst = mn.Body.Instructions[i];
            if (inst.OpCode == OpCodes.Stfld && inst.Operand.ToString().Contains("<task>__0")) {
                Console.WriteLine("Instruction index: " + i);
                for (int j = Math.Max(0, i - 15); j <= Math.Min(mn.Body.Instructions.Count - 1, i + 10); j++) {
                    Console.WriteLine("  " + mn.Body.Instructions[j]);
                }
                Console.WriteLine("--------------------------------------");
            }
        }
    }
}


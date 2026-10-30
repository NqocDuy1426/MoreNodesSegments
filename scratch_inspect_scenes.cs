using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var lm = asm.MainModule.GetType("LoadingManager");
        Console.WriteLine("LoadingManager methods/nested:");
        foreach (var t in lm.NestedTypes) {
            if (t.Name.Contains("LoadLevel") || t.Name.Contains("LoadSimulation")) {
                Console.WriteLine("Type: " + t.FullName);
                var moveNext = t.Methods.FirstOrDefault(m => m.Name == "MoveNext");
                if (moveNext != null) {
                    Console.WriteLine("  Strings in " + t.Name + ".MoveNext:");
                    foreach (var inst in moveNext.Body.Instructions) {
                        if (inst.OpCode == OpCodes.Ldstr) {
                            Console.WriteLine("    " + inst.Operand);
                        }
                    }
                }
            }
        }
    }
}


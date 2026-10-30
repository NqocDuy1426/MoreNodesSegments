using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        foreach (var type in asm.MainModule.Types) {
            foreach (var m in type.Methods) {
                if (m.HasBody) {
                    foreach (var inst in m.Body.Instructions) {
                        if (inst.OpCode == OpCodes.Call || inst.OpCode == OpCodes.Callvirt) {
                            if (inst.Operand != null && inst.Operand.ToString().Contains("TrySpawn")) {
                                Console.WriteLine(type.FullName + "::" + m.Name + " calls " + inst.Operand);
                            }
                        }
                    }
                }
            }
        }
    }
}

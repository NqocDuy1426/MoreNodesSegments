using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var lm = asm.MainModule.GetType("LoadingManager");
        foreach (var nested in lm.NestedTypes) {
            if (nested.Name.Contains("LoadSimulationData")) {
                Console.WriteLine("Nested type: " + nested.FullName);
                foreach (var m in nested.Methods) {
                    if (m.Name == "MoveNext") {
                        foreach (var instr in m.Body.Instructions) {
                            if (instr.OpCode == OpCodes.Ldstr) {
                                Console.WriteLine("  String: " + instr.Operand);
                            }
                        }
                    }
                }
            }
        }
    }
}

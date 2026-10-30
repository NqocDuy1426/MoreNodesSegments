using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var sm = asm.MainModule.GetType("SimulationManager");
        foreach (var nested in sm.NestedTypes) {
            if (nested.Name == "Data") {
                foreach (var m in nested.Methods) {
                    if (m.Name == "Deserialize") {
                        foreach (var instr in m.Body.Instructions) {
                            if (instr.OpCode == OpCodes.Ldstr) {
                                Console.WriteLine("  Manager: " + instr.Operand);
                            }
                        }
                    }
                }
            }
        }
    }
}

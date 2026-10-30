using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var sm = asm.MainModule.GetType("SimulationManager");
        foreach (var m in sm.Methods) {
            if (m.Name == "SimulationStep") {
                Console.WriteLine("Method: " + m.FullName);
                foreach (var inst in m.Body.Instructions) {
                    if (inst.OpCode == OpCodes.Call || inst.OpCode == OpCodes.Callvirt) {
                        Console.WriteLine("  calls: " + inst.Operand);
                    }
                }
            }
        }
    }
}

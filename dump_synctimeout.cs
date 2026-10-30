
using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var sm = asm.MainModule.GetType("SimulationManager");
        foreach (var m in sm.Methods) {
            if (m.Name == ".cctor") {
                foreach (var inst in m.Body.Instructions) {
                    if (inst.Operand != null && inst.Operand.ToString().Contains("SYNCHRONIZE_TIMEOUT")) {
                        Console.WriteLine(inst);
                    } else if (inst.OpCode.ToString().Contains("ldc")) {
                        Console.WriteLine("  " + inst);
                    }
                }
            }
        }
    }
}

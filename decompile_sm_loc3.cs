using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var sm = asm.MainModule.GetType("SimulationManager");
        foreach (var m in sm.Methods) {
            if (m.Name == "SimulationStep") {
                for (int i = 0x0150; i < 0x0250; i++) {
                    foreach (var inst in m.Body.Instructions) {
                        if (inst.Offset == i) Console.WriteLine(inst);
                    }
                }
            }
        }
    }
}

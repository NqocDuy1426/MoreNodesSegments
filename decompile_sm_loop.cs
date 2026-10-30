using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var sm = asm.MainModule.GetType("SimulationManager");
        foreach (var m in sm.Methods) {
            if (m.Name == "SimulationStep") {
                for (int i = 0; i < m.Body.Instructions.Count; i++) {
                    var inst = m.Body.Instructions[i];
                    if (inst.OpCode == OpCodes.Callvirt && inst.Operand.ToString().Contains("SimulationStep")) {
                        for (int j = Math.Max(0, i - 15); j <= Math.Min(m.Body.Instructions.Count - 1, i + 5); j++) {
                            Console.WriteLine(m.Body.Instructions[j]);
                        }
                    }
                }
            }
        }
    }
}

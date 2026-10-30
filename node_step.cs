using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var type = asm.MainModule.GetType("NetNode");
        foreach (var m in type.Methods) {
            if (m.Name == "SimulationStep" && m.Parameters.Count == 1) {
                foreach (var inst in m.Body.Instructions) {
                    if (inst.OpCode == OpCodes.Callvirt && inst.Operand.ToString().Contains("TrafficLightSimulationStep")) {
                        int idx = m.Body.Instructions.IndexOf(inst);
                        for (int j = Math.Max(0, idx - 15); j <= idx; j++) {
                            Console.WriteLine(m.Body.Instructions[j]);
                        }
                    }
                }
            }
        }
    }
}

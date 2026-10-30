using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var t = asm.MainModule.GetType("RoadBaseAI");
        foreach (var m in t.Methods) {
            if (m.Name == "SimulationStep" && m.Parameters.Count == 2) {
                int idx = 0;
                foreach (var i in m.Body.Instructions) {
                    if (i.Operand != null && i.Operand.ToString().Contains("TrafficLightSimulationStep")) {
                        idx = m.Body.Instructions.IndexOf(i);
                        break;
                    }
                }
                for (int j = Math.Max(0, idx - 25); j <= Math.Min(m.Body.Instructions.Count - 1, idx + 10); j++) {
                    Console.WriteLine(m.Body.Instructions[j]);
                }
            }
        }
    }
}

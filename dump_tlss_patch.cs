
using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var type = asm.MainModule.GetType("TrafficManager.Patch._RoadBaseAI.TrafficLightSimulationStepPatch");
        if (type == null) {
            foreach (var t in asm.MainModule.Types) {
                if (t.Name.Contains("TrafficLightSimulationStepPatch")) {
                    type = t;
                    break;
                }
            }
        }
        Console.WriteLine("Type: " + type?.FullName);
        if (type != null) {
            foreach (var m in type.Methods) {
                Console.WriteLine("Method: " + m.FullName);
                if (m.HasBody) {
                    foreach (var ins in m.Body.Instructions) {
                        Console.WriteLine("  " + ins.ToString());
                    }
                }
            }
        }
    }
}

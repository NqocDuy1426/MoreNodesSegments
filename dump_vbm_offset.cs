
using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var t = asm.MainModule.GetType("TrafficManager.Manager.Impl.VehicleBehaviorManager");
        foreach (var m in t.Methods) {
            if (m.Name == "MayChangeSegment" && m.ReturnType.Name.Contains("VehicleJunctionTransitState")) {
                foreach (var ins in m.Body.Instructions) {
                    if (ins.Offset >= 0x0320 && ins.Offset <= 0x0355) {
                        Console.WriteLine(ins);
                    }
                }
            }
        }
    }
}

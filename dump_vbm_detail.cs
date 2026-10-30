
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
                for (int i = 0; i < m.Body.Instructions.Count; i++) {
                    var ins = m.Body.Instructions[i];
                    if (ins.OpCode == OpCodes.Call && ins.Operand.ToString().Contains("SetTrafficLightState")) {
                        for (int j = Math.Max(0, i - 40); j <= i; j++) {
                            Console.WriteLine(m.Body.Instructions[j]);
                        }
                    }
                }
            }
        }
    }
}

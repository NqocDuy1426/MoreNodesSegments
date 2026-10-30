using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var resolver = new DefaultAssemblyResolver();
        string home = Environment.GetEnvironmentVariable("HOME");
        string gameDir = home + "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        resolver.AddSearchDirectory(gameDir);
        var p = new ReaderParameters { AssemblyResolver = resolver };
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll", p);
        
        var carAiPatch = asm.MainModule.Types.FirstOrDefault(t => t.FullName == "TrafficManager.Patch._VehicleAI._CarAI.SimulationStepPatch");
        if (carAiPatch != null) {
            Console.WriteLine("SimulationStepPatch methods:");
            foreach (var m in carAiPatch.Methods) {
                Console.WriteLine("  " + m.Name);
                if (m.HasBody) {
                    foreach (var inst in m.Body.Instructions) {
                        if (inst.OpCode == OpCodes.Call || inst.OpCode == OpCodes.Callvirt) {
                            Console.WriteLine("    " + inst.OpCode + " " + inst.Operand);
                        }
                    }
                }
            }
        }

        var vbm = asm.MainModule.Types.FirstOrDefault(t => t.FullName == "TrafficManager.Manager.Impl.VehicleBehaviorManager");
        if (vbm != null) {
            Console.WriteLine("\nVehicleBehaviorManager methods:");
            foreach (var m in vbm.Methods.Where(m => m.Name.Contains("MayChangeSegment"))) {
                Console.WriteLine("  " + m.FullName);
                if (m.HasBody) {
                    foreach (var inst in m.Body.Instructions) {
                        if (inst.OpCode == OpCodes.Call || inst.OpCode == OpCodes.Callvirt) {
                            Console.WriteLine("    " + inst.OpCode + " " + inst.Operand);
                        }
                    }
                }
            }
        }
    }
}

using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        string[] toolNames = new string[] {
            "TrafficManager.UI.SubTools.SpeedLimits.SpeedLimitsTool",
            "TrafficManager.UI.SubTools.PrioritySigns.PrioritySignsTool",
            "TrafficManager.UI.SubTools.JunctionRestrictionsTool",
            "TrafficManager.UI.SubTools.LaneConnectorTool",
            "TrafficManager.UI.SubTools.ToggleTrafficLightsTool",
            "TrafficManager.UI.SubTools.ManualTrafficLightsTool",
            "TrafficManager.UI.SubTools.TTL.TimedTrafficLightsTool",
            "TrafficManager.UI.SubTools.VehicleRestrictionsTool",
            "TrafficManager.UI.SubTools.ParkingRestrictionsTool",
            "TrafficManager.UI.SubTools.LaneArrows.LaneArrowTool"
        };

        foreach (var name in toolNames) {
            var t = asm.MainModule.GetType(name);
            if (t == null) {
                Console.WriteLine("Not found: " + name);
                continue;
            }
            Console.WriteLine("==================================================");
            Console.WriteLine("Tool: " + t.FullName);
            foreach (var m in t.Methods) {
                if (m.HasBody) {
                    foreach (var ins in m.Body.Instructions) {
                        if (ins.OpCode.Code == Mono.Cecil.Cil.Code.Ldc_I4) {
                            int v = (int)ins.Operand;
                            if (v == 32768 || v == 36864 || v == 73728 || v == 16384 || v == 65536) {
                                Console.WriteLine($"  Method {m.Name} uses constant {v}");
                            }
                        }
                    }
                }
            }
            foreach (var nested in t.NestedTypes) {
                foreach (var m in nested.Methods) {
                    if (m.HasBody) {
                        foreach (var ins in m.Body.Instructions) {
                            if (ins.OpCode.Code == Mono.Cecil.Cil.Code.Ldc_I4) {
                                int v = (int)ins.Operand;
                                if (v == 32768 || v == 36864 || v == 73728 || v == 16384 || v == 65536) {
                                    Console.WriteLine($"  Nested {nested.Name}::{m.Name} uses constant {v}");
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

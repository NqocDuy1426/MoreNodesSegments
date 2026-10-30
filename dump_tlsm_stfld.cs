
using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        foreach (var t in asm.MainModule.Types) {
            foreach (var m in t.Methods) {
                if (!m.HasBody) continue;
                foreach (var ins in m.Body.Instructions) {
                    if (ins.OpCode == OpCodes.Stfld && ins.Operand.ToString().Contains("TrafficLightSimulations")) {
                        Console.WriteLine($"{t.FullName}.{m.Name} sets TrafficLightSimulations");
                    }
                }
            }
        }
    }
}

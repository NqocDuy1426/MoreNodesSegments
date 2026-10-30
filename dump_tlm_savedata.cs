using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class P {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var t = asm.MainModule.GetType("TrafficManager.Manager.Impl.TrafficLightManager");
        foreach (var m in t.Methods) {
            if (m.Name.Contains("SaveData")) {
                Console.WriteLine("\nMethod: " + m.Name);
                if (m.HasBody) {
                    foreach (var i in m.Body.Instructions) {
                        Console.WriteLine("  " + i.OpCode + " " + i.Operand);
                    }
                }
            }
        }
    }
}

using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        foreach (var t in asm.MainModule.Types) {
            foreach (var m in t.Methods) {
                if (m.HasBody) {
                    foreach (var ins in m.Body.Instructions) {
                        if (ins.Operand != null && ins.Operand.ToString().Contains("DrawSpeedLimitHandles")) {
                            Console.WriteLine($"{t.FullName}::{m.Name} calls {ins.Operand}");
                        }
                    }
                }
            }
        }
    }
}

using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var t = asm.MainModule.GetType("TrafficManager.UI.SubTools.PrioritySigns.PrioritySignsTool");
        foreach (var m in t.Methods) {
            if (m.Name == "ShowGUI") {
                Console.WriteLine("=== " + m.FullName + " (Count: " + m.Body.Instructions.Count + ") ===");
                for (int i = 0; i < 70; i++) {
                    Console.WriteLine("  " + m.Body.Instructions[i]);
                }
            }
        }
    }
}

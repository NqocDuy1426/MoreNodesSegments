using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var t = asm.MainModule.GetType("TrafficManager.UI.SubTools.JunctionRestrictionsTool");
        foreach (var m in t.Methods) {
            if (m.Name == "RefreshCurrentRestrictedNodeIds") {
                Console.WriteLine("=== " + m.FullName + " ===");
                foreach (var ins in m.Body.Instructions) {
                    Console.WriteLine("  " + ins);
                }
            }
        }
    }
}

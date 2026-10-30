using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var t = asm.MainModule.GetType("TrafficManager.Manager.Impl.TrafficPriorityManager");
        foreach (var m in t.Methods) {
            if (m.Name.Contains("HasNodePrioritySign") || m.Name.Contains("HasSegmentPrioritySign") || m.Name.Contains("GetPrioritySign") || m.Name.Contains("SetPrioritySign")) {
                Console.WriteLine("=== " + m.FullName + " ===");
                foreach (var ins in m.Body.Instructions) {
                    Console.WriteLine("  " + ins);
                }
            }
        }
    }
}

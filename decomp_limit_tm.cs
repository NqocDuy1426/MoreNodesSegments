using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/455403039/TreeUnlimiter.dll");
        var t = asm.MainModule.GetType("TreeUnlimiter.Detours.LimitTreeManager");
        var m = t.Methods;
        foreach (var method in m) {
            if (method.Name == "UpdateData") {
                Console.WriteLine("=== LimitTreeManager.UpdateData ===");
                foreach (var i in method.Body.Instructions) {
                    Console.WriteLine("  " + i);
                }
            }
        }
    }
}

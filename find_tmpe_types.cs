using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        foreach (var t in asm.MainModule.Types) {
            CheckType(t);
        }
    }
    static void CheckType(TypeDefinition t) {
        if (t.FullName.IndexOf("Speed", StringComparison.OrdinalIgnoreCase) >= 0 ||
            t.FullName.IndexOf("Priority", StringComparison.OrdinalIgnoreCase) >= 0 ||
            t.FullName.IndexOf("Overlay", StringComparison.OrdinalIgnoreCase) >= 0 ||
            t.FullName.IndexOf("SubTool", StringComparison.OrdinalIgnoreCase) >= 0) {
            Console.WriteLine(t.FullName);
        }
        foreach (var nested in t.NestedTypes) {
            CheckType(nested);
        }
    }
}

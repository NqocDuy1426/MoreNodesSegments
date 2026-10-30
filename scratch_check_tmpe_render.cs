
using System;
using Mono.Cecil;
class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        foreach (var t in asm.MainModule.Types) {
            if (t.Name.Contains("Tool") || t.Name.Contains("Overlay") || t.Name.Contains("Renderer")) {
                Console.WriteLine(t.FullName);
                foreach (var m in t.Methods) {
                    if (m.Name.Contains("Render") || m.Name.Contains("GUI") || m.Name.Contains("Draw")) {
                        Console.WriteLine("   " + m.Name);
                    }
                }
            }
        }
    }
}

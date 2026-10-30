
using System;
using Mono.Cecil;

class Program {
    static void Main() {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        foreach (var t in asm.MainModule.Types) {
            if (t.FullName.Contains("TrafficLight")) {
                Console.WriteLine("Type: " + t.FullName);
                foreach (var m in t.Methods) {
                    if (m.IsPublic) Console.WriteLine("  Method: " + m.Name);
                }
            }
        }
    }
}

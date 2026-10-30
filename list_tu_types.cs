using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/455403039/TreeUnlimiter.dll");
        foreach (var t in asm.MainModule.Types) {
            Console.WriteLine("Type: " + t.FullName);
            foreach (var m in t.Methods) {
                if (m.Name.Contains("Init") || m.Name.Contains("Ensure") || m.Name.Contains("Update")) {
                    Console.WriteLine("  " + m.Name);
                }
            }
        }
    }
}

using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/455403039/TreeUnlimiter.dll");
        foreach (var t in asm.MainModule.Types) {
            if (t.Name.Contains("DisplayClass15_0")) {
                Console.WriteLine("Type: " + t.FullName);
                foreach (var m in t.Methods) {
                    Console.WriteLine("  Method: " + m.FullName);
                    if (m.HasBody) {
                        foreach (var i in m.Body.Instructions) {
                            Console.WriteLine("    " + i);
                        }
                    }
                }
            }
        }
    }
}

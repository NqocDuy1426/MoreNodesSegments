using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/455403039/TreeUnlimiter.dll");
        var t = asm.MainModule.GetType("TreeUnlimiter.Detours.LimitTreeManager");
        foreach (var nt in t.NestedTypes) {
            Console.WriteLine("NestedType: " + nt.FullName);
            foreach (var m in nt.Methods) {
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

using System;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("content/MoreNetworkCapacity.Core.dll");
        foreach (var t in asm.MainModule.Types) {
            if (t.Name.Contains("NetManager") || t.Name.Contains("Deserialize")) {
                Console.WriteLine("Type: " + t.FullName);
                foreach (var m in t.Methods) {
                    Console.WriteLine("  Method: " + m.FullName);
                }
            }
        }
    }
}

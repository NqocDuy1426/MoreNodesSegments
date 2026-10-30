using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/MoreNetworkCapacity.Core.dll");
        var t = asm.MainModule.GetType("MoreNetworkCapacity.Compatibility.TMPECompatibility");
        foreach (var m in t.Methods) {
            if (m.Name == "ApplyIndexPatches") {
                Console.WriteLine("=== " + m.FullName + " ===");
                foreach (var ins in m.Body.Instructions) {
                    Console.WriteLine("  " + ins);
                }
            }
        }
    }
}

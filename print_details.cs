using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Downloads/depot_255710_2829241538003963999 (1)/MoreNetworkCapacityRenew.dll");
        foreach (var type in asm.MainModule.Types) {
            if (type.Name == "RenewMod") {
                foreach (var m in type.Methods) {
                    if (m.Name == "get_Name" || m.Name == "get_Description" || m.Name == "OnEnabled") {
                        Console.WriteLine("Method: " + m.Name);
                        if (m.HasBody) {
                            foreach (var inst in m.Body.Instructions) {
                                Console.WriteLine("  " + inst);
                            }
                        }
                    }
                }
            }
        }
    }
}

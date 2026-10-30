using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Downloads/depot_255710_2829241538003963999 (1)/MoreNetworkCapacityRenew.dll");
        foreach (var type in asm.MainModule.Types) {
            if (type.Name == "RenewLoading") {
                foreach (var m in type.Methods) {
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

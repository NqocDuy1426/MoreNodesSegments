using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/MoreNetworkCapacity.Core.dll");
        foreach (var t in asm.MainModule.Types) {
            foreach (var m in t.Methods) {
                if (m.HasBody) {
                    foreach (var ins in m.Body.Instructions) {
                        if (ins.Operand != null && ins.Operand.ToString().Contains("SegmentEndGetIndexPrefix")) {
                            Console.WriteLine($"{t.FullName}::{m.Name} references {ins.Operand}");
                        }
                    }
                }
            }
        }
    }
}

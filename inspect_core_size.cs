using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/MoreNetworkCapacity.Core.dll");
        foreach (var t in asm.MainModule.Types) {
            foreach (var m in t.Methods) {
                if (m.HasBody) {
                    foreach (var ins in m.Body.Instructions) {
                        if (ins.OpCode.Code == Mono.Cecil.Cil.Code.Ldc_I4) {
                            int val = (int)ins.Operand;
                            if (val > 36864 && val <= 65536) {
                                Console.WriteLine($"{t.FullName}::{m.Name} uses {val}");
                            }
                        }
                    }
                }
            }
        }
    }
}

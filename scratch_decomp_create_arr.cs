using System;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("content/MoreNetworkCapacity.Core.dll");
        var t = asm.MainModule.Types.First(x => x.Name == "NetManagerCustomizer");
        foreach (var m in t.Methods) {
            if (m.Name.Contains("CreateOrdered")) {
                Console.WriteLine($"=== Method: {m.FullName} ===");
                foreach (var ins in m.Body.Instructions) {
                    Console.WriteLine($"{ins.Offset:X4}: {ins.OpCode} {ins.Operand}");
                }
            }
        }
    }
}

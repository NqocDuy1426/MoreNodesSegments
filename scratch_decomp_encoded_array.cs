using System;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("ColossalManaged.dll");
        var ea = asm.MainModule.Types.First(t => t.Name == "EncodedArray");
        foreach (var nested in ea.NestedTypes) {
            Console.WriteLine("Type: " + nested.FullName);
            foreach (var m in nested.Methods) {
                if (m.Name == "BeginRead" || m.Name == "Read") {
                    Console.WriteLine("  Method: " + m.FullName);
                    foreach (var ins in m.Body.Instructions) {
                        Console.WriteLine($"    {ins.Offset:X4}: {ins.OpCode} {ins.Operand}");
                    }
                }
            }
        }
    }
}

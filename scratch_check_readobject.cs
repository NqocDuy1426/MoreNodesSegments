using System;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("ColossalManaged.dll");
        var ds = asm.MainModule.Types.First(t => t.Name == "DataSerializer");
        foreach (var m in ds.Methods) {
            if (m.Name == "ReadObject" || m.Name == "ReadObjectArray" || m.Name == "ReadSharedType") {
                Console.WriteLine("Method: " + m.FullName);
                foreach (var ins in m.Body.Instructions) {
                    Console.WriteLine($"  {ins.Offset:X4}: {ins.OpCode} {ins.Operand}");
                }
            }
        }
    }
}

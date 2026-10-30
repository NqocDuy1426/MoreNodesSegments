
using System;
using Mono.Cecil;
class P {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("ColossalManaged.dll");
        var t = asm.MainModule.GetType("ColossalFramework.IO.DataSerializer");
        foreach (var m in t.Methods) {
            if (m.Name.Contains("UInt16")) {
                Console.WriteLine($"Method: {m.FullName}");
                foreach (var i in m.Body.Instructions) {
                    Console.WriteLine($"  {i.Offset:X4}: {i.OpCode} {i.Operand}");
                }
            }
        }
    }
}

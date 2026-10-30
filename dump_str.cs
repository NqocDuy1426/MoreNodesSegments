using System;
using System.Linq;
using Mono.Cecil;
class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("ColossalManaged.dll");
        var t = asm.MainModule.GetType("ColossalFramework.IO.DataSerializer");
        var m = t.Methods.First(x => x.Name == "WriteUniqueString");
        foreach (var i in m.Body.Instructions) {
            Console.WriteLine($"{i.Offset:X4}: {i.OpCode} {i.Operand}");
        }
    }
}

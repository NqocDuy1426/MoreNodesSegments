using System;
using System.Linq;
using Mono.Cecil;
class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var t = asm.MainModule.GetType("TreeManager/Data");
        var m = t.Methods.First(x => x.Name == "Deserialize");
        Console.WriteLine($"Method: {m.FullName}");
        foreach (var i in m.Body.Instructions) {
            Console.WriteLine($"{i.Offset:X4}: {i.OpCode} {i.Operand}");
        }
    }
}

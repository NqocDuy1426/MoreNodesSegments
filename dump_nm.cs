using System;
using System.Linq;
using Mono.Cecil;
class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var m = asm.MainModule.GetType("NetManager/Data").Methods.First(x => x.Name == "Deserialize");
        foreach (var i in m.Body.Instructions) {
            if (i.Offset >= 0x1F90 && i.Offset <= 0x20B0)
                Console.WriteLine($"{i.Offset:X4}: {i.OpCode} {i.Operand}");
        }
    }
}

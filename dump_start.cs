using System;
using System.Linq;
using Mono.Cecil;
class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var m = asm.MainModule.GetType("NetManager/Data").Methods.First(x => x.Name == "Deserialize");
        for (int i = 0; i < 40; i++) {
            var instr = m.Body.Instructions[i];
            Console.WriteLine($"{instr.Offset:X4}: {instr.OpCode} {instr.Operand}");
        }
    }
}

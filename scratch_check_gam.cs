using System;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var gam = asm.MainModule.Types.First(t => t.Name == "GameAreaManager");
        var data = gam.NestedTypes.First(t => t.Name == "Data");
        var des = data.Methods.First(m => m.Name == "Deserialize");
        for (int i = 0; i < Math.Min(25, des.Body.Instructions.Count); i++) {
            var ins = des.Body.Instructions[i];
            Console.WriteLine($"{ins.Offset:X4}: {ins.OpCode} {ins.Operand}");
        }
    }
}

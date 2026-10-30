using System;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var tm = asm.MainModule.Types.First(t => t.Name == "TerrainManager");
        var data = tm.NestedTypes.First(t => t.Name == "Data");
        var des = data.Methods.First(m => m.Name == "Deserialize");
        for (int i = 0; i < Math.Min(35, des.Body.Instructions.Count); i++) {
            var ins = des.Body.Instructions[i];
            Console.WriteLine($"{ins.Offset:X4}: {ins.OpCode} {ins.Operand}");
        }
    }
}

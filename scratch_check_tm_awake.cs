using System;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var tm = asm.MainModule.Types.First(t => t.Name == "TerrainManager");
        var m = tm.Methods.First(x => x.Name == "Awake");
        foreach (var ins in m.Body.Instructions) {
            if (ins.OpCode.Name.Contains("newarr") || ins.OpCode.Name.Contains("stfld")) {
                Console.WriteLine($"{ins.Offset:X4}: {ins.OpCode} {ins.Operand}");
            }
        }
    }
}

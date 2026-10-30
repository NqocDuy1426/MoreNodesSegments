using System;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var nm = asm.MainModule.Types.First(x => x.Name == "NetManager");
        var data = nm.NestedTypes.First(x => x.Name == "Data");
        var des = data.Methods.First(x => x.Name == "Deserialize");

        foreach (var ins in des.Body.Instructions) {
            if (ins.Offset >= 0x13F0 && ins.Offset <= 0x1430) {
                Console.WriteLine($"{ins.Offset:X4}: {ins.OpCode} {ins.Operand}");
            }
        }
    }
}

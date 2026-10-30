using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var nm = asm.MainModule.Types.First(x => x.Name == "NetManager");
        var data = nm.NestedTypes.First(x => x.Name == "Data");
        var des = data.Methods.First(x => x.Name == "Deserialize");

        foreach (var ins in des.Body.Instructions) {
            if (ins.Offset >= 0x1800 && ins.Offset < 0x2000) {
                string s = ins.ToString();
                if (ins.OpCode == OpCodes.Call || ins.OpCode == OpCodes.Callvirt || ins.OpCode.Name.Contains("elem") || ins.OpCode.Name.Contains("stloc") || s.Contains("Prefab") || s.Contains("Buffer") || s.Contains("Read")) {
                    Console.WriteLine($"{ins.Offset:X4}: {ins.OpCode} {ins.Operand}");
                }
            }
        }
    }
}

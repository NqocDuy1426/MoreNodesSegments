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
        Console.WriteLine($"=== NetManager.Data.Deserialize ({des.Body.Instructions.Count} instructions) ===");
        int i = 0;
        foreach (var instr in des.Body.Instructions) {
            string s = instr.ToString();
            // print lines around array accesses or loops or deserialization
            if (i < 80 || s.Contains("stelem") || s.Contains("ldelem") || s.Contains("throw") || s.Contains("Capacity") || s.Contains("OutOfRange")) {
                Console.WriteLine($"{instr.Offset:X4}: {instr.OpCode} {instr.Operand}");
            }
            i++;
        }
    }
}

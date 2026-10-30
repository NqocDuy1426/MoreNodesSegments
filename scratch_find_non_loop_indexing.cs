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

        // Find all instructions with ldelem or ldelema
        for (int i = 0; i < des.Body.Instructions.Count; i++) {
            var ins = des.Body.Instructions[i];
            if (ins.OpCode.Name.Contains("elem")) {
                // Look back at previous 5 instructions to see where index came from
                string prev = "";
                for (int k = Math.Max(0, i - 4); k < i; k++) {
                    prev += des.Body.Instructions[k].ToString() + " -> ";
                }
                Console.WriteLine($"{ins.Offset:X4}: {ins.OpCode} {ins.Operand} | PREV: {prev}");
            }
        }
    }
}

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

        Console.WriteLine("Locals in NetManager.Data.Deserialize:");
        for (int i = 0; i < des.Body.Variables.Count; i++) {
            var v = des.Body.Variables[i];
            Console.WriteLine($"  V_{i}: {v.VariableType.FullName}");
        }

        Console.WriteLine("\nArray accesses in NetManager.Data.Deserialize:");
        for (int i = 0; i < des.Body.Instructions.Count; i++) {
            var ins = des.Body.Instructions[i];
            var op = ins.OpCode.Name;
            if (op.Contains("elem")) {
                Console.WriteLine($"{ins.Offset:X4}: {ins.OpCode} (previous: {des.Body.Instructions[i-2].OpCode} {des.Body.Instructions[i-2].Operand}, {des.Body.Instructions[i-1].OpCode} {des.Body.Instructions[i-1].Operand})");
            }
        }
    }
}

using System;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var sm = asm.MainModule.Types.First(t => t.Name == "SimulationManager");
        var data = sm.NestedTypes.First(t => t.Name == "Data");
        var des = data.Methods.First(m => m.Name == "Deserialize");
        for (int i = 0; i < des.Body.Instructions.Count; i++) {
            var ins = des.Body.Instructions[i];
            if (ins.OpCode.Name.Contains("call") && ins.Operand.ToString().Contains("Read")) {
                Console.WriteLine($"{ins.Offset:X4}: {ins.OpCode} {ins.Operand}");
            }
        }
    }
}

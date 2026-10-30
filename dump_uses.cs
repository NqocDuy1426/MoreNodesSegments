using System;
using System.Linq;
using Mono.Cecil;
class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var m = asm.MainModule.GetType("NetManager/Data").Methods.First(x => x.Name == "Deserialize");
        for (int i = 0; i < m.Body.Instructions.Count; i++) {
            var instr = m.Body.Instructions[i];
            if (instr.Operand != null && (instr.Operand.ToString().Contains("V_7") || instr.Operand.ToString().Contains("V_8"))) {
                Console.WriteLine($"{instr.Offset:X4}: {instr.OpCode} {instr.Operand}");
            }
        }
    }
}

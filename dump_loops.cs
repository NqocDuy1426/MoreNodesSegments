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
                Console.WriteLine($"--- At {instr.Offset:X4}: {instr.OpCode} {instr.Operand} ---");
                // print 5 instructions before and after
                int start = Math.Max(0, i - 10);
                int end = Math.Min(m.Body.Instructions.Count - 1, i + 5);
                for (int j = start; j <= end; j++) {
                    var cur = m.Body.Instructions[j];
                    string mark = (j == i) ? "==> " : "    ";
                    Console.WriteLine($"{mark}{cur.Offset:X4}: {cur.OpCode} {cur.Operand}");
                }
            }
        }
    }
}

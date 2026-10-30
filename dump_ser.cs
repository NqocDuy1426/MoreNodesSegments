using System;
using System.Linq;
using Mono.Cecil;
class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var m = asm.MainModule.GetType("NetManager/Data").Methods.First(x => x.Name == "Serialize");
        for (int i = 0; i < m.Body.Instructions.Count; i++) {
            var instr = m.Body.Instructions[i];
            if (instr.Operand != null && instr.Operand.ToString().Contains("m_adjustedSegments")) {
                Console.WriteLine($"--- At {instr.Offset:X4}: {instr.OpCode} {instr.Operand} ---");
                int start = Math.Max(0, i - 5);
                int end = Math.Min(m.Body.Instructions.Count - 1, i + 15);
                for (int j = start; j <= end; j++) {
                    var cur = m.Body.Instructions[j];
                    Console.WriteLine($"    {cur.Offset:X4}: {cur.OpCode} {cur.Operand}");
                }
            }
        }
    }
}

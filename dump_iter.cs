using System;
using System.Linq;
using Mono.Cecil;
class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var t = asm.MainModule.GetType("LoadingManager/<LoadSimulationData>c__IteratorB");
        var m = t.Methods.First(x => x.Name == "MoveNext");
        for (int i = 0; i < m.Body.Instructions.Count; i++) {
            var instr = m.Body.Instructions[i];
            if (instr.Operand != null && instr.Operand.ToString().Contains("SimulationManager/Data")) {
                Console.WriteLine($"Instruction: {instr.Offset:X4}: {instr.OpCode} {instr.Operand}");
                int start = Math.Max(0, i - 5);
                int end = Math.Min(m.Body.Instructions.Count - 1, i + 10);
                for (int j = start; j <= end; j++) {
                    Console.WriteLine($"   {m.Body.Instructions[j].Offset:X4}: {m.Body.Instructions[j].OpCode} {m.Body.Instructions[j].Operand}");
                }
            }
        }
    }
}

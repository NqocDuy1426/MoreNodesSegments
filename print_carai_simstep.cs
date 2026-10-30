using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var carAi = asm.MainModule.GetType("CarAI");
        foreach (var m in carAi.Methods) {
            if (m.Name == "SimulationStep" && m.Parameters.Count == 6) {
                Console.WriteLine("=== " + m.FullName + " ===");
                var insts = m.Body.Instructions;
                for (int i = 0; i < insts.Count; i++) {
                    if (insts[i].Operand != null && insts[i].Operand.ToString().Contains("TrySpawn")) {
                        int start = Math.Max(0, i - 25);
                        int end = Math.Min(insts.Count - 1, i + 20);
                        for (int j = start; j <= end; j++) {
                            Console.WriteLine(string.Format("{0} {1,-8} {2,-15} {3}", (j == i ? "-->" : "   "), insts[j].Offset.ToString("X4"), insts[j].OpCode, insts[j].Operand));
                        }
                    }
                }
            }
        }
    }
}

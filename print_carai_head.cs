using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var carAi = asm.MainModule.GetType("CarAI");
        foreach (var m in carAi.Methods) {
            if (m.Name == "SimulationStep" && m.Parameters.Count == 3) {
                Console.WriteLine("=== " + m.FullName + " ===");
                var insts = m.Body.Instructions;
                for (int i = 0; i < Math.Min(50, insts.Count); i++) {
                    Console.WriteLine(string.Format("{0,-8} {1,-15} {2}", insts[i].Offset.ToString("X4"), insts[i].OpCode, insts[i].Operand));
                }
            }
        }
    }
}

using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var t = asm.MainModule.GetType("CarAI");
        foreach (var m in t.Methods) {
            if (m.Name == "SimulationStep" && m.Parameters.Count == 6) {
                for (int i = 0; i < m.Body.Instructions.Count; i++) {
                    var instr = m.Body.Instructions[i];
                    if (instr.OpCode == OpCodes.Stloc_S && instr.Operand.ToString().Contains("V_36")) {
                        Console.WriteLine("Found stloc V_36 at IL_" + instr.Offset.ToString("X4"));
                        for (int j = Math.Max(0, i - 10); j <= Math.Min(m.Body.Instructions.Count - 1, i + 5); j++) {
                            Console.WriteLine("  IL_" + m.Body.Instructions[j].Offset.ToString("X4") + ": " + m.Body.Instructions[j]);
                        }
                    }
                }
            }
        }
    }
}

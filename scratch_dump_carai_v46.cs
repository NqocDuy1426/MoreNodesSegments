using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var t = asm.MainModule.GetType("CarAI");
        foreach (var m in t.Methods) {
            if (m.Name == "SimulationStep" && m.Parameters.Count == 6) {
                foreach (var instr in m.Body.Instructions) {
                    if (instr.Offset >= 0x0A80 && instr.Offset <= 0x0AD0) {
                        Console.WriteLine("  IL_" + instr.Offset.ToString("X4") + ": " + instr.OpCode + " " + instr.Operand);
                    }
                }
            }
        }
    }
}

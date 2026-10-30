using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var carAi = asm.MainModule.GetType("CarAI");
        foreach (var m in carAi.Methods) {
            if (m.Name == "UpdateNodeTargetPos") {
                Console.WriteLine("CarAI.UpdateNodeTargetPos instructions: " + m.Body.Instructions.Count);
                for (int i = 0; i < m.Body.Instructions.Count; i++) {
                    var instr = m.Body.Instructions[i];
                    if (instr.OpCode.FlowControl == FlowControl.Call || instr.OpCode.ToString().StartsWith("stfld") || instr.OpCode.ToString().StartsWith("ldfld")) {
                        Console.WriteLine("  " + instr.Offset.ToString("X4") + ": " + instr.OpCode + " " + instr.Operand);
                    }
                }
            }
        }
    }
}

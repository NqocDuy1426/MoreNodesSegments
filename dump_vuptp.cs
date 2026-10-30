
using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var t = asm.MainModule.GetType("VehicleAI");
        foreach (var m in t.Methods) {
            if (m.Name == "UpdatePathTargetPositions") {
                Console.WriteLine("VehicleAI.UpdatePathTargetPositions:");
                for (int i = 0; i < m.Body.Instructions.Count; i++) {
                    var ins = m.Body.Instructions[i];
                    if (ins.OpCode == OpCodes.Ldc_I4 && (int)ins.Operand == 4096) {
                        for (int j = Math.Max(0, i - 10); j <= Math.Min(m.Body.Instructions.Count - 1, i + 15); j++) {
                            Console.WriteLine(m.Body.Instructions[j]);
                        }
                    }
                }
            }
        }
    }
}

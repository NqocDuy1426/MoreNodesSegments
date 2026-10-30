using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var carAi = asm.MainModule.GetType("CarAI");
        foreach (var m in carAi.Methods) {
            if (m.Name == "UpdateNodeTargetPos") {
                for (int i = 0; i < 25; i++) {
                    Console.WriteLine("  " + m.Body.Instructions[i].Offset.ToString("X4") + ": " + m.Body.Instructions[i].OpCode + " " + m.Body.Instructions[i].Operand);
                }
            }
        }
    }
}

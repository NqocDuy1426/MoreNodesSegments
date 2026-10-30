
using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var type = asm.MainModule.GetType("CarAI");
        foreach (var m in type.Methods) {
            if (m.Name == "CalculateSegmentPosition") {
                for (int i = 0; i < m.Body.Instructions.Count; i++) {
                    var ins = m.Body.Instructions[i];
                    if (ins.OpCode == OpCodes.Call && ins.Operand.ToString().Contains("GetTrafficLightState")) {
                        for (int j = Math.Max(0, i - 60); j <= Math.Max(0, i - 20); j++) {
                            Console.WriteLine(m.Body.Instructions[j]);
                        }
                    }
                }
            }
        }
    }
}

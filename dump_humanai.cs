
using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var type = asm.MainModule.GetType("HumanAI");
        foreach (var m in type.Methods) {
            if (m.Name == "CheckTrafficLights") {
                foreach (var ins in m.Body.Instructions) {
                    if (ins.OpCode == OpCodes.Call && ins.Operand.ToString().Contains("GetTrafficLightState")) {
                        int idx = m.Body.Instructions.IndexOf(ins);
                        for (int j = Math.Max(0, idx - 20); j <= idx; j++) {
                            Console.WriteLine(m.Body.Instructions[j]);
                        }
                    }
                }
            }
        }
    }
}

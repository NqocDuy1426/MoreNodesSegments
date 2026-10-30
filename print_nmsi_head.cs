using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var nm = asm.MainModule.GetType("NetManager");
        foreach (var m in nm.Methods) {
            if (m.Name == "SimulationStepImpl") {
                for (int i = 0; i < 100 && i < m.Body.Instructions.Count; i++) {
                    Console.WriteLine(m.Body.Instructions[i]);
                }
            }
        }
    }
}

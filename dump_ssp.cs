
using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var lm = asm.MainModule.GetType("LoadingManager");
        foreach (var m in lm.Methods) {
            if (m.Name == "SetSimulationProgress") {
                Console.WriteLine("Method: " + m.FullName);
                foreach (var inst in m.Body.Instructions) {
                    Console.WriteLine("  " + inst);
                }
            }
        }
    }
}

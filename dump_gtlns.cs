
using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var roadBaseAI = asm.MainModule.GetType("RoadBaseAI");
        foreach (var m in roadBaseAI.Methods) {
            if (m.Name == "GetTrafficLightNodeState") {
                Console.WriteLine("Method: " + m.FullName);
                foreach (var ins in m.Body.Instructions) {
                    Console.WriteLine("  " + ins.ToString());
                }
            }
        }
    }
}

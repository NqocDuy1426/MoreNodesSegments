using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var ai = asm.MainModule.GetType("RoadBaseAI");
        foreach (var m in ai.Methods) {
            if (m.Name == "SimulationStep" && m.Parameters.Count == 2 && m.Parameters[0].ParameterType.Name == "UInt16" && m.Parameters[1].ParameterType.Name == "NetNode&") {
                Console.WriteLine("Method: " + m.FullName);
                foreach (var inst in m.Body.Instructions) {
                    Console.WriteLine(inst);
                }
            }
        }
    }
}

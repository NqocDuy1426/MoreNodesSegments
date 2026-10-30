
using System;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var type = asm.MainModule.GetType("RoadBaseAI");
        foreach (var m in type.Methods) {
            if (m.Name == "TrafficLightSimulationStep") {
                using (var sw = new StreamWriter("tls_full_il.txt")) {
                    for (int i = 0; i < m.Body.Instructions.Count; i++) {
                        sw.WriteLine(m.Body.Instructions[i].ToString());
                    }
                }
                Console.WriteLine("Written tls_full_il.txt with " + m.Body.Instructions.Count + " instructions");
            }
        }
    }
}

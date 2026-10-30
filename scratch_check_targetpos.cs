using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var carAi = asm.MainModule.GetType("CarAI");
        foreach (var m in carAi.Methods) {
            if (m.Name.Contains("TargetPos") || m.Name.Contains("Node") || m.Name.Contains("Segment")) {
                Console.WriteLine("CarAI." + m.Name + " (" + m.Parameters.Count + " params)");
            }
        }
    }
}

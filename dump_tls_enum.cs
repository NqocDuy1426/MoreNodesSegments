
using System;
using Mono.Cecil;

class Program {
    static void Main() {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var roadBaseAI = asm.MainModule.GetType("RoadBaseAI");
        foreach (var nested in roadBaseAI.NestedTypes) {
            if (nested.Name == "TrafficLightState") {
                foreach (var f in nested.Fields) {
                    if (f.HasConstant) Console.WriteLine($"{f.Name} = {f.Constant}");
                }
            }
        }
    }
}

using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var carai = asm.MainModule.GetType("CarAI");
        foreach (var m in carai.Methods) {
            if (m.Name == "CalculateSegmentPosition") {
                Console.WriteLine("Method: " + m.FullName);
                foreach (var p in m.Parameters) {
                    Console.WriteLine("  " + p.ParameterType.FullName + " " + p.Name);
                }
            }
        }
    }
}


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
                foreach (var inst in m.Body.Instructions) {
                    Console.WriteLine(inst);
                }
            }
        }
    }
}

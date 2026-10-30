using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        foreach (var t in asm.MainModule.Types) {
            foreach (var m in t.Methods) {
                if (m.HasBody) {
                    foreach (var i in m.Body.Instructions) {
                        if (i.Operand != null && i.Operand.ToString().Contains("TrafficLightSimulationStep")) {
                            Console.WriteLine($"{t.FullName}.{m.Name} calls {i.Operand}");
                        }
                    }
                }
            }
        }
    }
}

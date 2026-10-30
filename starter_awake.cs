
using System;
using Mono.Cecil;
class P {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var starter = asm.MainModule.GetType("Starter");
        foreach (var m in starter.Methods) {
            if (m.Name == "Awake") {
                foreach (var i in m.Body.Instructions) {
                    if (i.Operand != null && i.Operand.ToString().Contains("RegisterManager")) {
                        Console.WriteLine("  " + i.Previous.Operand);
                    }
                }
            }
        }
    }
}

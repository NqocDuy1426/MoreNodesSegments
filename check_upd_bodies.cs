
using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        foreach (var t in asm.MainModule.Types) {
            foreach (var iface in t.Interfaces) {
                if (iface.InterfaceType.FullName == "ISimulationManager") {
                    foreach (var m in t.Methods) {
                        if (m.Name == "UpdateData") {
                            Console.WriteLine(t.Name + ".UpdateData: instructions=" + m.Body.Instructions.Count);
                        }
                    }
                }
            }
        }
    }
}


using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var sm = asm.MainModule.GetType("SimulationManager");
        foreach (var m in sm.Methods) {
            if (m.Name == "Awake" || m.Name == ".cctor" || m.Name.Contains("Register")) {
                Console.WriteLine("Method: " + m.FullName);
                foreach (var inst in m.Body.Instructions) {
                    if (inst.ToString().Contains("m_managers") || inst.ToString().Contains("Add")) {
                        Console.WriteLine("  " + inst);
                    }
                }
            }
        }
    }
}

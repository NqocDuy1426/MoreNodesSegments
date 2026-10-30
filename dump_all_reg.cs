
using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        // Look for all calls to RegisterManager or RegisterSimulationManager in all methods of all types
        foreach (var t in asm.MainModule.Types) {
            foreach (var m in t.Methods) {
                if (m.HasBody) {
                    foreach (var inst in m.Body.Instructions) {
                        if (inst.Operand != null && inst.Operand.ToString().Contains("RegisterSimulationManager")) {
                            Console.WriteLine(t.FullName + " -> " + m.FullName);
                        }
                        if (inst.Operand != null && inst.Operand.ToString().Contains("RegisterManager")) {
                            Console.WriteLine("RegisterManager: " + t.FullName + " -> " + m.FullName);
                        }
                    }
                }
            }
        }
    }
}

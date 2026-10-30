using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var lm = asm.MainModule.GetType("LoadingManager");
        var simField = lm.Fields.FirstOrDefault(f => f.Name == "m_simulationDataLoaded");
        Console.WriteLine("m_simulationDataLoaded found: " + (simField != null));
        foreach (var m in lm.Methods) {
            if (m.HasBody && m.Body.Instructions.Any(i => i.Operand == simField)) {
                Console.WriteLine("LoadingManager method touches m_simulationDataLoaded: " + m.Name);
            }
        }
        foreach (var t in lm.NestedTypes) {
            foreach (var m in t.Methods) {
                if (m.HasBody && m.Body.Instructions.Any(i => i.Operand == simField)) {
                    Console.WriteLine("Nested type method touches m_simulationDataLoaded: " + t.Name + "." + m.Name);
                }
            }
        }
    }
}


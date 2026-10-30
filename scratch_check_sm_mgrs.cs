using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var sm = asm.MainModule.Types.First(t => t.Name == "SimulationManager");
        foreach (var m in sm.Methods) {
            if (m.HasBody && m.Body.Instructions.Any(i => i.Operand != null && i.Operand.ToString().Contains("m_managers"))) {
                Console.WriteLine("Uses m_managers: " + m.FullName);
            }
        }
    }
}

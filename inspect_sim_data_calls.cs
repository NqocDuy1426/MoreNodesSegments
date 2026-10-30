using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var sm = asm.MainModule.Types.FirstOrDefault(t => t.Name == "SimulationManager");
        var data = sm.NestedTypes.FirstOrDefault(t => t.Name == "Data");
        var m = data.Methods.FirstOrDefault(x => x.Name == "Deserialize");
        foreach (var ins in m.Body.Instructions) {
            if (ins.OpCode == OpCodes.Call || ins.OpCode == OpCodes.Callvirt) {
                Console.WriteLine("  " + ins.Operand);
            }
        }
    }
}

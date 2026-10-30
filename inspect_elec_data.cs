using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var em = asm.MainModule.Types.FirstOrDefault(t => t.Name == "ElectricityManager");
        var data = em.NestedTypes.FirstOrDefault(t => t.Name == "Data");
        var m = data.Methods.FirstOrDefault(x => x.Name == "Deserialize");
        Console.WriteLine("Method: " + m.FullName);
        foreach (var ins in m.Body.Instructions) {
            if (ins.OpCode == OpCodes.Call || ins.OpCode == OpCodes.Callvirt) {
                Console.WriteLine("  " + ins.Operand);
            }
        }
    }
}

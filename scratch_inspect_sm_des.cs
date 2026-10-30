using System;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var sm = asm.MainModule.GetType("SimulationManager");
        var data = sm.NestedTypes.FirstOrDefault(x => x.Name == "Data");
        var des = data.Methods.FirstOrDefault(x => x.Name == "Deserialize");
        if (des != null) {
            foreach (var inst in des.Body.Instructions) {
                if (inst.OpCode == Mono.Cecil.Cil.OpCodes.Callvirt && inst.Operand.ToString().Contains("ReadObject")) {
                    Console.WriteLine("ReadObject: " + inst.Operand);
                }
            }
        }
    }
}


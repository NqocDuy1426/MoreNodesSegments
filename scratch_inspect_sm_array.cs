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
            for (int i = 0; i < des.Body.Instructions.Count; i++) {
                if (des.Body.Instructions[i].Operand != null && des.Body.Instructions[i].Operand.ToString().Contains("ReadObjectArray")) {
                    for (int j = Math.Max(0, i - 5); j <= Math.Min(des.Body.Instructions.Count - 1, i + 15); j++) {
                        Console.WriteLine(des.Body.Instructions[j]);
                    }
                }
            }
        }
    }
}

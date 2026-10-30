using System;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var wm = asm.MainModule.GetType("WeatherManager");
        var data = wm.NestedTypes.FirstOrDefault(x => x.Name == "Data");
        var des = data.Methods.FirstOrDefault(x => x.Name == "Deserialize");
        if (des != null) {
            foreach (var inst in des.Body.Instructions) {
                Console.WriteLine(inst);
            }
        }
    }
}


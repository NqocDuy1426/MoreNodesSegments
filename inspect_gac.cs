using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var t = asm.MainModule.GetType("TrafficManager.Util.Caching.GenericArrayCache`1");
        Console.WriteLine("Type: " + t.FullName);
        foreach (var f in t.Fields) {
            Console.WriteLine("  " + f.FieldType.FullName + " " + f.Name);
        }
        foreach (var m in t.Methods) {
            Console.WriteLine("  " + m.FullName);
            if (m.Name == "Add") {
                foreach (var ins in m.Body.Instructions) {
                    Console.WriteLine("    " + ins);
                }
            }
        }
    }
}

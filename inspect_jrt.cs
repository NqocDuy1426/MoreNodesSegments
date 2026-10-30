using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var t = asm.MainModule.GetType("TrafficManager.UI.SubTools.JunctionRestrictionsTool");
        Console.WriteLine("Type: " + t.FullName);
        foreach (var f in t.Fields) {
            Console.WriteLine("  " + f.FieldType.Name + " " + f.Name);
        }
        foreach (var m in t.Methods) {
            Console.WriteLine("  " + m.FullName);
        }
    }
}

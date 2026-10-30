using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var jrm = asm.MainModule.GetType("TrafficManager.Manager.Impl.JunctionRestrictionsManager");
        foreach (var f in jrm.Fields) {
            Console.WriteLine("Field: " + f.FieldType.FullName + " " + f.Name);
        }
    }
}

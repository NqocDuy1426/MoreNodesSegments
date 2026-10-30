using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var t = asm.MainModule.GetType("TrafficManager.UI.SubTools.SpeedLimits.Overlay.SpeedLimitsOverlay");
        Console.WriteLine("Type: " + t.FullName);
        Console.WriteLine("Fields:");
        foreach (var f in t.Fields) {
            Console.WriteLine("  " + f.FieldType.Name + " " + f.Name);
        }
        Console.WriteLine("Methods:");
        foreach (var m in t.Methods) {
            Console.WriteLine("  " + m.FullName);
        }
    }
}

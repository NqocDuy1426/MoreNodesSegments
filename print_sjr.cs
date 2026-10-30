using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        foreach (var t in asm.MainModule.Types) {
            if (t.Name.Contains("SegmentJunctionRestrictions") || t.Name.Contains("JunctionRestrictions")) {
                Console.WriteLine("Type: " + t.FullName + " (isVal=" + t.IsValueType + ")");
                foreach (var f in t.Fields) {
                    Console.WriteLine("  Field: " + f.FieldType.FullName + " " + f.Name);
                }
            }
        }
    }
}

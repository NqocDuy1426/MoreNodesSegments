
using System;
using Mono.Cecil;
class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        foreach (var t in asm.MainModule.Types) {
            foreach (var m in t.Methods) {
                if (m.HasBody) {
                    foreach (var inst in m.Body.Instructions) {
                        if (inst.Operand is MethodReference mr && mr.DeclaringType.Name.Contains("RoadBaseAI")) {
                            Console.WriteLine(t.FullName + "." + m.Name + " -> " + mr.DeclaringType.Name + "." + mr.Name);
                        }
                    }
                }
            }
        }
    }
}

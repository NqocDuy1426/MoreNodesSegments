using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var tmpe = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        foreach (var type in tmpe.MainModule.Types) {
            foreach (var m in type.Methods) {
                if (m.HasBody) {
                    foreach (var ins in m.Body.Instructions) {
                        var fr = ins.Operand as FieldReference;
                        if (fr != null && fr.DeclaringType.Name == "ExtSegmentEnd") {
                            if (fr.Name == "incoming" || fr.Name == "outgoing" || fr.Name == "nodeId" || fr.Name == "laneArrows") {
                                Console.WriteLine($"{type.Name}.{m.Name} accesses ExtSegmentEnd.{fr.Name}");
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}

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
                    foreach (var instr in m.Body.Instructions) {
                        if (instr.OpCode == OpCodes.Newobj) {
                            var mr = instr.Operand as MethodReference;
                            if (mr != null && mr.DeclaringType.Name == "ExtSegmentEnd") {
                                Console.WriteLine($"Found new ExtSegmentEnd in {type.FullName}.{m.Name}");
                            }
                        }
                    }
                }
            }
        }
    }
}

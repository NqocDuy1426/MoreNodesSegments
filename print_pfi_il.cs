using System;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var t = asm.MainModule.GetType("TrafficManager.Custom.PathFinding.CustomPathFind");
        foreach (var m in t.Methods) {
            if (m.Name == "PathFindImplementation") {
                using (var sw = new StreamWriter("pfi_full_il.txt")) {
                    for (int i = 0; i < m.Body.Instructions.Count; i++) {
                        var ins = m.Body.Instructions[i];
                        sw.WriteLine($"{i,4} IL_{ins.Offset:x4}: {ins.OpCode} {ins.Operand}");
                    }
                }
                Console.WriteLine("Wrote " + m.Body.Instructions.Count + " instructions.");
            }
        }
    }
}

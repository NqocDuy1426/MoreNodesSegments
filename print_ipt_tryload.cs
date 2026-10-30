using System;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/3690061052/ImprovedPublicTransport3.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var t = asm.MainModule.GetType("ImprovedPublicTransport.Data.CachedTransportLineData");
        foreach (var m in t.Methods) {
            if (m.Name == "TryLoadData") {
                using (var sw = new StreamWriter("ipt_tryload.txt")) {
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

using System;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("ColossalManaged.dll");
        var ds = asm.MainModule.Types.First(t => t.Name == "DataSerializer");
        foreach (var m in ds.Methods) {
            if (m.IsConstructor) {
                Console.WriteLine("Ctor: " + m.FullName);
                foreach (var ins in m.Body.Instructions) {
                    if (ins.ToString().Contains("m_typeList") || ins.ToString().Contains("List")) {
                        Console.WriteLine($"  {ins.Offset:X4}: {ins.OpCode} {ins.Operand}");
                    }
                }
            }
        }
    }
}

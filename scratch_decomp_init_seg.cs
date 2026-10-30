using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var nm = asm.MainModule.Types.First(x => x.Name == "NetManager");
        Check(nm, "InitializeNode");
        Check(nm, "InitializeSegment");
    }

    static void Check(TypeDefinition nm, string name) {
        var m = nm.Methods.First(x => x.Name == name);
        Console.WriteLine($"=== {name} ({m.Body.Instructions.Count} instructions) ===");
        foreach (var ins in m.Body.Instructions) {
            string s = ins.ToString();
            if (s.Contains("ldelem") || s.Contains("stelem") || s.Contains("call") || s.Contains("m_")) {
                Console.WriteLine($"{ins.Offset:X4}: {ins.OpCode} {ins.Operand}");
            }
        }
    }
}

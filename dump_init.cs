using System;
using System.Linq;
using Mono.Cecil;
class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var t = asm.MainModule.GetType("NetManager");
        var mNode = t.Methods.First(x => x.Name == "InitializeNode");
        Console.WriteLine($"=== InitializeNode ({mNode.Body.Instructions.Count} instructions) ===");
        foreach (var i in mNode.Body.Instructions) {
            Console.WriteLine($"{i.Offset:X4}: {i.OpCode} {i.Operand}");
        }
        var mSeg = t.Methods.First(x => x.Name == "InitializeSegment");
        Console.WriteLine($"=== InitializeSegment ({mSeg.Body.Instructions.Count} instructions) ===");
        foreach (var i in mSeg.Body.Instructions) {
            Console.WriteLine($"{i.Offset:X4}: {i.OpCode} {i.Operand}");
        }
    }
}

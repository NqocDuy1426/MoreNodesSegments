using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var resolver = new DefaultAssemblyResolver();
        resolver.AddSearchDirectory(".");
        var readerParams = new ReaderParameters { AssemblyResolver = resolver };
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll", readerParams);
        var lm = asm.MainModule.Types.First(t => t.Name == "LoadingManager");
        var iter = lm.NestedTypes.FirstOrDefault(t => t.Name.Contains("LoadSimulationData") && t.Name.Contains("Iterator"));
        if (iter != null) {
            Console.WriteLine("Iterator: " + iter.FullName);
            var moveNext = iter.Methods.FirstOrDefault(m => m.Name == "MoveNext");
            if (moveNext != null) {
                for (int i = 0; i < moveNext.Body.Instructions.Count; i++) {
                    var instr = moveNext.Body.Instructions[i];
                    string s = instr.ToString();
                    if (s.Contains("Deserialize") || s.Contains("SimulationManager") || s.Contains("Electricity") || s.Contains("NetManager") || s.Contains("LoadingProfiler")) {
                        Console.WriteLine($"{instr.Offset:X4}: {instr.OpCode} {instr.Operand}");
                    }
                }
            }
        }
    }
}

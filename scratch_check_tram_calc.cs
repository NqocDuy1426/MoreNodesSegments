
using System;
using Mono.Cecil;
class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var t = asm.MainModule.GetType("TramBaseAI");
        foreach (var m in t.Methods) {
            if (m.Name == "CalculateSegmentPosition") {
                foreach (var inst in m.Body.Instructions) {
                    if (inst.Operand is MethodReference mr && mr.Name.Contains("TrafficLight")) {
                        Console.WriteLine("  {0:X4}: {1} {2}", inst.Offset, inst.OpCode, inst.Operand);
                    }
                }
            }
        }
    }
}

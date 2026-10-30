
using System;
using Mono.Cecil;
class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var t = asm.MainModule.GetType("RoadBaseAI");
        foreach (var m in t.Methods) {
            if (m.Name.Contains("Bollard")) {
                Console.WriteLine("RoadBaseAI." + m.Name + " (" + m.Parameters.Count + " params)");
                foreach (var inst in m.Body.Instructions) {
                    Console.WriteLine("  {0:X4}: {1} {2}", inst.Offset, inst.OpCode, inst.Operand);
                }
            }
        }
    }
}

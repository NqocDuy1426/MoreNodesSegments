
using System;
using Mono.Cecil;
class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var t = asm.MainModule.GetType("TrafficManager.TrafficLight.Impl.CustomSegmentLight");
        foreach (var m in t.Methods) {
            if (m.Name == "UpdateVisuals") {
                Console.WriteLine(m.Name + " (" + m.Parameters.Count + " params)");
                foreach (var inst in m.Body.Instructions) {
                    Console.WriteLine("  {0:X4}: {1} {2}", inst.Offset, inst.OpCode, inst.Operand);
                }
            }
        }
    }
}

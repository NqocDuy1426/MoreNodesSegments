using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        foreach (var t in asm.MainModule.Types) {
            foreach (var m in t.Methods) {
                if (m.HasBody) {
                    foreach (var inst in m.Body.Instructions) {
                        if (inst.OpCode == OpCodes.Call || inst.OpCode == OpCodes.Callvirt) {
                            var opStr = inst.Operand.ToString();
                            if (opStr.Contains("MayChangeSegment")) {
                                Console.WriteLine(t.FullName + "." + m.Name + " calls " + opStr);
                            }
                        }
                    }
                }
            }
        }
    }
}

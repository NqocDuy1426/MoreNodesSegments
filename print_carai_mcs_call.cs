using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var carai = asm.MainModule.GetType("CarAI");
        foreach (var m in carai.Methods) {
            if (m.HasBody) {
                foreach (var inst in m.Body.Instructions) {
                    if (inst.OpCode == OpCodes.Call || inst.OpCode == OpCodes.Callvirt) {
                        var opStr = inst.Operand.ToString();
                        if (opStr.Contains("MayChangeSegment")) {
                            Console.WriteLine("Call in CarAI." + m.Name + ": " + opStr);
                        }
                    }
                }
            }
        }
    }
}

using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        Check(asm, "ElectricityManager");
        Check(asm, "NetManager");
    }

    static void Check(AssemblyDefinition asm, string mgrName) {
        var mgr = asm.MainModule.Types.FirstOrDefault(t => t.Name == mgrName);
        var data = mgr.NestedTypes.FirstOrDefault(t => t.Name == "Data");
        var des = data.Methods.FirstOrDefault(m => m.Name == "Deserialize");
        Console.WriteLine($"=== {mgrName}.Data.Deserialize ===");
        foreach (var instr in des.Body.Instructions) {
            if (instr.Operand != null && instr.Operand.ToString().Contains("BeginDeserialize")) {
                Console.WriteLine($"{instr.Offset:X4}: {instr.OpCode} {instr.Operand}");
            }
        }
    }
}

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
        var sm = asm.MainModule.Types.First(t => t.Name == "SimulationManager");
        var data = sm.NestedTypes.First(t => t.Name == "Data");
        var ser = data.Methods.First(m => m.Name == "Serialize");
        Console.WriteLine("SimulationManager.Data.Serialize calls:");
        foreach (var instr in ser.Body.Instructions) {
            if (instr.Operand != null && (instr.OpCode == OpCodes.Call || instr.OpCode == OpCodes.Callvirt || instr.OpCode == OpCodes.Newobj)) {
                string s = instr.Operand.ToString();
                if (s.Contains("Write") || s.Contains("Manager") || s.Contains("Data"))
                    Console.WriteLine($"{instr.Offset:X4}: {instr.OpCode} {instr.Operand}");
            }
        }
    }
}

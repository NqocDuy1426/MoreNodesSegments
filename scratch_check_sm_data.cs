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
        var des = data.Methods.First(m => m.Name == "Deserialize");
        Console.WriteLine("SimulationManager.Data.Deserialize instructions:");
        foreach (var instr in des.Body.Instructions) {
            if (instr.Operand != null)
                Console.WriteLine($"{instr.Offset:X4}: {instr.OpCode} {instr.Operand}");
            else
                Console.WriteLine($"{instr.Offset:X4}: {instr.OpCode}");
        }
    }
}

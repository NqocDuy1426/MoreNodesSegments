using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var sm = asm.MainModule.Types.First(t => t.Name == "SimulationManager");
        var awake = sm.Methods.FirstOrDefault(m => m.Name == "Awake");
        Console.WriteLine("SimulationManager.Awake:");
        foreach (var instr in awake.Body.Instructions) {
            if (instr.Operand != null && (instr.Operand.ToString().Contains("Add") || instr.Operand.ToString().Contains("instance"))) {
                Console.WriteLine($"{instr.Offset:X4}: {instr.OpCode} {instr.Operand}");
            }
        }
    }
}

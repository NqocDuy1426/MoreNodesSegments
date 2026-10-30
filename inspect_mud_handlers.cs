using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var sm = asm.MainModule.Types.FirstOrDefault(t => t.Name == "SimulationManager");
        var m = sm.Methods.FirstOrDefault(x => x.Name == "Managers_UpdateData");
        Console.WriteLine("Body instructions count: " + m.Body.Instructions.Count);
        Console.WriteLine("Exception handlers count: " + m.Body.ExceptionHandlers.Count);
        foreach (var eh in m.Body.ExceptionHandlers) {
            Console.WriteLine($"Handler: Type={eh.HandlerType}, CatchType={eh.CatchType?.FullName}, TryStart={eh.TryStart?.Offset}, TryEnd={eh.TryEnd?.Offset}, HandlerStart={eh.HandlerStart?.Offset}");
        }
    }
}

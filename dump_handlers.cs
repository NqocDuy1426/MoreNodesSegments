
using System;
using Mono.Cecil;
class P {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var t = asm.MainModule.GetType("LoadingManager/<LoadSimulationData>c__IteratorB");
        var m = System.Linq.Enumerable.First(t.Methods, x => x.Name == "MoveNext");
        Console.WriteLine("Exception handlers: " + m.Body.ExceptionHandlers.Count);
        foreach (var h in m.Body.ExceptionHandlers) {
            Console.WriteLine($"Type: {h.HandlerType}, CatchType: {h.CatchType?.FullName}, TryStart: {h.TryStart.Offset:X4}, TryEnd: {h.TryEnd.Offset:X4}, HandlerStart: {h.HandlerStart.Offset:X4}, HandlerEnd: {h.HandlerEnd?.Offset:X4}");
        }
        for (int i = 0x240; i < 0x2a0; i++) {
            // print instructions around offset
        }
    }
}

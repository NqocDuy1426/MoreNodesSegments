using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Addons/Mods/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var t = asm.MainModule.GetType("TrafficManager.Patch._VehicleAI._CarAI.SimulationStepPatch");
        var m = t.Methods[1]; // Prefix
        Console.WriteLine($"=== {t.FullName}.{m.Name} ===");
        foreach (var inst in m.Body.Instructions)
        {
            Console.WriteLine($"  {inst.OpCode} {inst.Operand}");
        }
    }
}

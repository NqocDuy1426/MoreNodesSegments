using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Addons/Mods/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        foreach (var t in asm.MainModule.Types)
        {
            if (t.FullName.Contains("SimulationStepPatch") || t.FullName.Contains("MayChangeSegment") || t.FullName.Contains("MayDespawn"))
            {
                Console.WriteLine($"Type: {t.FullName}");
                foreach (var m in t.Methods)
                {
                    Console.WriteLine($"  Method: {m.Name}");
                }
            }
        }
    }
}

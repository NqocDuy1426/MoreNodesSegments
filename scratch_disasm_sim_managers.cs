using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Addons/Mods/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        string[] targets = new string[] {
            "TrafficManager.Manager.Impl.GeometryManager",
            "TrafficManager.Manager.Impl.RoutingManager",
            "TrafficManager.Manager.Impl.TrafficLightSimulationManager"
        };
        foreach (var name in targets)
        {
            var t = asm.MainModule.GetType(name);
            foreach (var m in t.Methods)
            {
                if (m.Name == "SimulationStep")
                {
                    Console.WriteLine($"=== {t.Name}.{m.Name} ===");
                    foreach (var inst in m.Body.Instructions)
                    {
                        Console.WriteLine($"  {inst.OpCode} {inst.Operand}");
                    }
                }
            }
        }
    }
}

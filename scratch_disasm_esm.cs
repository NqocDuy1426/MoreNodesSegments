using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Addons/Mods/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var t = asm.MainModule.GetType("TrafficManager.Manager.Impl.ExtSegmentManager");
        foreach (var m in t.Methods)
        {
            if (m.Name.Contains("Load") || m.Name.Contains("Recalculate") || m.Name.Contains("Init"))
            {
                Console.WriteLine($"=== {m.Name} ===");
                foreach (var inst in m.Body.Instructions)
                {
                    Console.WriteLine($"  {inst.OpCode} {inst.Operand}");
                }
            }
        }
    }
}

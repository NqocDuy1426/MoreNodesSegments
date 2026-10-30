using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Addons/Mods/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var t = asm.MainModule.GetType("TrafficManager.Custom.PathFinding.CustomPathFind");
        foreach (var m in t.Methods)
        {
            if (m.Name == "ProcessItemCosts")
            {
                Console.WriteLine($"=== {m.Name} ===");
                foreach (var inst in m.Body.Instructions)
                {
                    if (inst.OpCode == OpCodes.Call || inst.OpCode == OpCodes.Callvirt)
                    {
                        Console.WriteLine($"  {inst.OpCode} {inst.Operand}");
                    }
                }
            }
        }
    }
}

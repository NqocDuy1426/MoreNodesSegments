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
            if (t.FullName.Contains("RoutingManager"))
            {
                foreach (var m in t.Methods)
                {
                    if (m.Name.Contains("Index") || m.Name.Contains("Routing") || m.Name.Contains("Calc"))
                    {
                        Console.WriteLine($"--- {t.FullName}.{m.Name} ---");
                        foreach (var inst in m.Body.Instructions)
                        {
                            Console.WriteLine($"  {inst.OpCode} {inst.Operand}");
                        }
                    }
                }
            }
        }
    }
}

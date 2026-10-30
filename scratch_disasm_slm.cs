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
            if (t.FullName.Contains("SpeedLimitManager"))
            {
                foreach (var m in t.Methods)
                {
                    if (m.Name.Contains("Speed") || m.Name.Contains("Init") || m.Name.Contains("Load") || m.Name.Contains("GetCustomSpeedLimit"))
                    {
                        Console.WriteLine($"=== {t.FullName}.{m.Name} ===");
                        if (m.HasBody)
                        {
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
}

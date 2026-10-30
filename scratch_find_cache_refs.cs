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
            if (t.FullName == "TrafficManager.Manager.Impl.SpeedLimitManager")
            {
                foreach (var m in t.Methods)
                {
                    if (!m.HasBody) continue;
                    foreach (var inst in m.Body.Instructions)
                    {
                        if (inst.Operand is FieldReference fr && fr.Name == "cachedLaneSpeedLimits_")
                        {
                            Console.WriteLine($"{m.Name} accesses cachedLaneSpeedLimits_");
                            break;
                        }
                    }
                }
            }
        }
    }
}

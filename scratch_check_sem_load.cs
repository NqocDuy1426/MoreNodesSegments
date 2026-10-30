using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Addons/Mods/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var t = asm.MainModule.GetType("TrafficManager.Manager.Impl.SegmentEndManager");
        foreach (var m in t.Methods)
        {
            if (m.Name.Contains("Load") || m.Name.Contains("Save") || m.Name.Contains("Data"))
            {
                Console.WriteLine($"{m.Name}");
            }
        }
    }
}

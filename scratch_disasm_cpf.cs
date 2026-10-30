using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Addons/Mods/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var t = asm.MainModule.GetType("TrafficManager.Custom.PathFinding.CustomPathFind");
        foreach (var f in t.Fields)
        {
            Console.WriteLine($"Field: {f.Name} : {f.FieldType.FullName}");
        }
    }
}

using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var type = asm.MainModule.Types.FirstOrDefault(t => t.FullName == "TrafficManager.Custom.PathFinding.CustomPathFind");
        var m = type.Methods.FirstOrDefault(meth => meth.Name == "PathFindThread");
        Console.WriteLine($"Method: {m.Name}");

        foreach (var ins in m.Body.Instructions.Take(100))
        {
            Console.WriteLine($"  {ins.Offset:X4}: {ins}");
        }
    }
}

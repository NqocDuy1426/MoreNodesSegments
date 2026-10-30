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
        var m = type.Methods.FirstOrDefault(meth => meth.Name == "PathFindImplementation");

        foreach (var ins in m.Body.Instructions.Where(i => i.Offset >= 0x0520 && i.Offset <= 0x0560))
        {
            Console.WriteLine($"  {ins.Offset:X4}: {ins}");
        }
    }
}

using System;
using System.IO;
using System.Linq;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var type = asm.MainModule.Types.FirstOrDefault(t => t.FullName.Contains("CustomPathFind"));
        Console.WriteLine("Type: " + type.FullName + " BaseType: " + type.BaseType.FullName);
        foreach (var m in type.Methods)
        {
            Console.WriteLine("  Method: " + m.Name);
        }
    }
}

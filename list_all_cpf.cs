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
        foreach (var type in asm.MainModule.Types.Where(t => t.Name.Contains("PathFind") || t.Name.Contains("PathManager")))
        {
            Console.WriteLine("Type: " + type.FullName + " BaseType: " + (type.BaseType != null ? type.BaseType.FullName : "none"));
            if (type.Name == "CustomPathFind")
            {
                foreach (var m in type.Methods)
                {
                    Console.WriteLine("  Method: " + m.Name);
                }
            }
        }
    }
}

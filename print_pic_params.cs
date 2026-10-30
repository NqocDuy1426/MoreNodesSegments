using System;
using System.IO;
using System.Linq;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string tmpePath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var assembly = AssemblyDefinition.ReadAssembly(tmpePath);
        var type = assembly.MainModule.Types.FirstOrDefault(t => t.FullName == "TrafficManager.Custom.PathFinding.CustomPathFind");

        foreach (var m in type.Methods.Where(m => m.Name == "ProcessItemCosts"))
        {
            Console.WriteLine("METHOD: " + m.FullName);
            foreach (var p in m.Parameters)
            {
                Console.WriteLine("  " + p.ParameterType.FullName + " " + p.Name);
            }
        }
    }
}

using System;
using System.Linq;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string tmpePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var assembly = AssemblyDefinition.ReadAssembly(tmpePath);
        foreach (var t in assembly.Modules.SelectMany(m => m.Types).Where(t => t.FullName.Contains("CalculateSegmentPosition")))
        {
            Console.WriteLine(t.FullName);
        }
    }
}

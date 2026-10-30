using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string tmpePath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(tmpePath);
        foreach (var type in asm.MainModule.Types)
        {
            if (type.Name.Contains("TrafficLight") || type.Name.Contains("SimulationStep"))
            {
                Console.WriteLine("TMPE Type: " + type.FullName);
            }
        }
    }
}

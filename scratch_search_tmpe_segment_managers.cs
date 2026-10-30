using System;
using System.IO;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string p = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + 
            "/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(p);

        foreach (var t in asm.MainModule.Types)
        {
            if (t.Name.Contains("Segment") || t.Name.Contains("Traffic"))
            {
                foreach (var m in t.Methods)
                {
                    if (m.Name.Contains("SimulationStep") || m.Name.Contains("OnBeforeSimulationStep"))
                    {
                        Console.WriteLine($"{t.FullName}::{m.Name}");
                    }
                }
            }
        }
    }
}

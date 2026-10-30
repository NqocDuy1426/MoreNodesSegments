using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/2858591409/LoadingScreenModRevisited.dll");
        foreach (var t in asm.MainModule.Types)
        {
            if (t.FullName.Contains("Simulation") || t.FullName.Contains("LevelLoader"))
            {
                Console.WriteLine("TYPE: " + t.FullName);
                foreach (var m in t.Methods)
                {
                    if (m.Name.Contains("Simulation") || m.Name.Contains("Load") || m.Name.Contains("Deserialize"))
                    {
                        Console.WriteLine("  METHOD: " + m.FullName);
                    }
                }
            }
        }
    }
}

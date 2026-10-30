using System;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string p = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + 
            "/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(p);

        foreach (var t in asm.MainModule.Types)
        {
            if (t.Namespace != null && (t.Namespace.Contains("Tool") || t.Namespace.Contains("UI")))
            {
                if (t.Name.Contains("Tool") || t.Name.Contains("SubTool"))
                {
                    Console.WriteLine($"\n=== {t.FullName} ===");
                    foreach (var m in t.Methods)
                    {
                        if (m.Name.Contains("Node") || m.Name.Contains("Segment") || m.Name.Contains("Hover") || m.Name.Contains("Click") || m.Name.Contains("Raycast") || m.Name.Contains("Determine"))
                            Console.WriteLine($"  {m.Name}");
                    }
                }
            }
        }
    }
}

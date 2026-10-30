using System;
using System.IO;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string[] searchPaths = new string[] {
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll",
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Library/Application Support/Steam/steamapps/workshop/content/255710/1806963141/TrafficManager.dll"
        };

        AssemblyDefinition asm = null;
        foreach (var p in searchPaths)
        {
            if (File.Exists(p)) { asm = AssemblyDefinition.ReadAssembly(p); Console.WriteLine("Loaded: " + p); break; }
        }
        if (asm == null) { Console.WriteLine("TMPE not found in standard paths"); return; }

        var type = asm.MainModule.GetType("TrafficManager.Manager.Impl.TrafficMeasurementManager");
        if (type != null)
        {
            Console.WriteLine($"=== {type.FullName} ===");
            foreach (var f in type.Fields)
                Console.WriteLine($"  field: {f.FieldType.Name} {f.Name}");
            foreach (var m in type.Methods)
                Console.WriteLine($"  method: {m.Name}");
        }
    }
}

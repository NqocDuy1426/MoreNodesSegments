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
            if (t.Namespace != null && t.Namespace.Contains("TrafficManager.Manager.Impl"))
            {
                Console.WriteLine($"\n=== {t.Name} ===");
                foreach (var f in t.Fields)
                {
                    if (f.FieldType.IsArray || f.FieldType.Name.Contains("List") || f.FieldType.Name.Contains("Dictionary") || f.FieldType.Name.Contains("Array"))
                        Console.WriteLine($"  field: {f.FieldType.Name} {f.Name}");
                }
            }
        }
    }
}

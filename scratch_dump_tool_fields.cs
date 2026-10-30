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

        var type = asm.MainModule.GetType("TrafficManager.UI.TrafficManagerTool");
        foreach (var f in type.Fields)
        {
            Console.WriteLine($"  {f.FieldType.Name} {f.Name}");
        }
    }
}

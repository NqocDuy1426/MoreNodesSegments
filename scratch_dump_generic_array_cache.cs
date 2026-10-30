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
            if (t.Name.Contains("GenericArrayCache"))
            {
                Console.WriteLine($"Type: {t.FullName}");
                foreach (var f in t.Fields) Console.WriteLine($"  field: {f.FieldType.Name} {f.Name}");
                foreach (var m in t.Methods) Console.WriteLine($"  method: {m.Name}");
            }
        }
    }
}

using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string dll = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/ColossalManaged.dll";
        var mod = ModuleDefinition.ReadModule(dll);
        foreach (var t in mod.GetTypes())
        {
            if (t.FullName.Contains("Package"))
            {
                Console.WriteLine("Type: " + t.FullName);
                foreach (var m in t.Methods)
                {
                    if (m.IsPublic && (m.Name.Contains("Load") || m.Name.Contains("Deserialize") || m.Name.Contains("Open") || m.Name.Contains("Save")))
                        Console.WriteLine("  Method: " + m.FullName);
                }
            }
        }
    }
}

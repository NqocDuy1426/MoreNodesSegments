using System;
using System.IO;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string[] dirs = new string[] {
            "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710",
            "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Addons/Mods"
        };

        foreach (var dir in dirs)
        {
            if (!Directory.Exists(dir)) continue;
            foreach (var dll in Directory.GetFiles(dir, "*.dll", SearchOption.AllDirectories))
            {
                try
                {
                    var asm = AssemblyDefinition.ReadAssembly(dll);
                    foreach (var type in asm.MainModule.Types)
                    {
                        CheckType(type, dll);
                    }
                }
                catch {}
            }
        }
    }

    static void CheckType(TypeDefinition type, string dll)
    {
        foreach (var iface in type.Interfaces)
        {
            if (iface.InterfaceType.Name == "ISerializableDataExtension" || iface.InterfaceType.Name == "ISimulationManager")
            {
                Console.WriteLine($"[{iface.InterfaceType.Name}] {type.FullName} in {Path.GetFileName(dll)} ({Path.GetFileName(Path.GetDirectoryName(dll))})");
            }
        }
        foreach (var nested in type.NestedTypes)
        {
            CheckType(nested, dll);
        }
    }
}

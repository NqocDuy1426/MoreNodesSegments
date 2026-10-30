using System;
using System.IO;
using System.Linq;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string dir = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710";
        var dlls = Directory.GetFiles(dir, "*.dll", SearchOption.AllDirectories);
        foreach (var dll in dlls)
        {
            try
            {
                var asm = AssemblyDefinition.ReadAssembly(dll);
                foreach (var mod in asm.Modules)
                {
                    foreach (var type in mod.Types)
                    {
                        foreach (var ca in type.CustomAttributes)
                        {
                            if (ca.AttributeType.Name == "HarmonyPatch")
                            {
                                string text = string.Join(" ", ca.ConstructorArguments.Select(a => a.Value?.ToString() ?? ""));
                                if (text.Contains("NetManager") || text.Contains("Deserialize"))
                                {
                                    Console.WriteLine("HarmonyPatch on type: " + Path.GetFileName(dll) + " -> " + type.FullName + " args: " + text);
                                }
                            }
                        }
                    }
                }
            }
            catch {}
        }
    }
}

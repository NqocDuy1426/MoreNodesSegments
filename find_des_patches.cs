using System;
using System.IO;
using System.Linq;
using Mono.Cecil;

class FindDesPatches
{
    static void Main()
    {
        string[] dirs = {
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
                        CheckType(dll, type);
                    }
                }
                catch {}
            }
        }
    }

    static void CheckType(string dll, TypeDefinition type)
    {
        foreach (var ca in type.CustomAttributes)
        {
            if (ca.AttributeType.Name.Contains("HarmonyPatch"))
            {
                string s = string.Join(", ", ca.ConstructorArguments.Select(a => a.Value != null ? a.Value.ToString() : "null").ToArray());
                if (s.Contains("Deserialize") || s.Contains("NetManager"))
                {
                    Console.WriteLine(string.Format("{0} -> Type {1}: {2}", Path.GetFileName(dll), type.FullName, s));
                }
            }
        }
        foreach (var nested in type.NestedTypes)
        {
            CheckType(dll, nested);
        }
    }
}

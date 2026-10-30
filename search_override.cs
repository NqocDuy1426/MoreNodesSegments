using System;
using System.IO;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string dir = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710";
        foreach (var dll in Directory.GetFiles(dir, "*.dll", SearchOption.AllDirectories))
        {
            try
            {
                var asm = AssemblyDefinition.ReadAssembly(dll);
                foreach (var t in asm.MainModule.Types)
                {
                    CheckType(t, dll);
                }
            }
            catch {}
        }
    }

    static void CheckType(TypeDefinition t, string dll)
    {
        foreach (var m in t.Methods)
        {
            if (m.HasBody)
            {
                foreach (var i in m.Body.Instructions)
                {
                    if (i.ToString().Contains("m_modOverride"))
                    {
                        Console.WriteLine(Path.GetFileName(dll) + " -> " + t.FullName + "::" + m.Name + " -> " + i);
                    }
                }
            }
        }
        foreach (var n in t.NestedTypes)
        {
            CheckType(n, dll);
        }
    }
}

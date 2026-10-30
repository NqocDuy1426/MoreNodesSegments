using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/2858591409/LoadingScreenModRevisited.dll");
        foreach (var t in asm.MainModule.Types)
        {
            Scan(t);
        }
    }

    static void Scan(TypeDefinition t)
    {
        foreach (var m in t.Methods)
        {
            if (m.HasBody)
            {
                foreach (var i in m.Body.Instructions)
                {
                    string s = i.ToString();
                    if (s.Contains("Deserialize") || s.Contains("SimulationManager") || s.Contains("SimulationThread"))
                    {
                        Console.WriteLine(t.FullName + "::" + m.Name + " -> " + s);
                    }
                }
            }
        }
        foreach (var nested in t.NestedTypes)
        {
            Scan(nested);
        }
    }
}

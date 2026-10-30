using System;
using System.Linq;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string tmpePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var assembly = AssemblyDefinition.ReadAssembly(tmpePath);

        foreach (var t in assembly.Modules.SelectMany(m => m.Types))
        {
            foreach (var m in t.Methods)
            {
                if (m.Name.IndexOf("stuck", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    m.Name.IndexOf("unstick", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    m.Name.IndexOf("demolish", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    Console.WriteLine(t.FullName + "::" + m.Name);
                }
            }
        }
    }
}

using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string tmpePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var assembly = AssemblyDefinition.ReadAssembly(tmpePath);

        foreach (var t in assembly.Modules.SelectMany(m => m.Types).Where(t => t.Name.Contains("NetManager")))
        {
            Console.WriteLine("Type: " + t.FullName);
            foreach (var m in t.Methods)
            {
                Console.WriteLine("   " + m.Name);
            }
        }
    }
}

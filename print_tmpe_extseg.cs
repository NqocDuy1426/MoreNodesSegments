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

        var type = assembly.Modules.SelectMany(m => m.Types).FirstOrDefault(t => t.FullName == "TrafficManager.Manager.Impl.ExtSegmentManager");
        if (type != null)
        {
            Console.WriteLine("ExtSegmentManager methods:");
            foreach (var m in type.Methods)
            {
                Console.WriteLine("   " + m.Name);
            }
        }
    }
}

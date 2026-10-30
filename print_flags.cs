using System;
using System.Linq;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string tmpePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var assembly = AssemblyDefinition.ReadAssembly(tmpePath);
        var type = assembly.Modules.SelectMany(m => m.GetTypes()).FirstOrDefault(t => t.FullName == "TrafficManager.State.Flags");
        if (type != null)
        {
            Console.WriteLine("Flags fields:");
            foreach (var f in type.Fields)
            {
                Console.WriteLine("  " + f.FieldType + " " + f.Name);
            }
        }
    }
}

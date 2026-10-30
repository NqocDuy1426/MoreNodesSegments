using System;
using System.Linq;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string dir = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252";
        foreach (var dll in System.IO.Directory.GetFiles(dir, "*.dll"))
        {
            var asm = AssemblyDefinition.ReadAssembly(dll);
            var type = asm.Modules.SelectMany(m => m.GetTypes()).FirstOrDefault(t => t.Name == "LaneTransitionData");
            if (type != null)
            {
                Console.WriteLine(dll + " -> " + type.FullName);
                foreach (var f in type.Fields)
                {
                    Console.WriteLine("  " + f.FieldType + " " + f.Name);
                }
            }
        }
    }
}

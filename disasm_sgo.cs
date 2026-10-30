using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        foreach (var type in asm.MainModule.Types)
        {
            if (type.FullName == "TrafficManager.State.SavedGameOptions")
            {
                Console.WriteLine("Type: " + type.FullName);
                foreach (var f in type.Fields)
                {
                    Console.WriteLine("Field: " + f.Name + " (" + f.FieldType.FullName + ")");
                }
                foreach (var m in type.Methods)
                {
                    Console.WriteLine("Method: " + m.Name);
                    if (m.Name == "Create" || m.Name == "Ensure" || m.Name == ".ctor")
                    {
                        Console.WriteLine("--- " + m.Name + " ---");
                        if (m.Body != null)
                        {
                            foreach (var inst in m.Body.Instructions)
                                Console.WriteLine(inst);
                        }
                    }
                }
            }
        }
    }
}

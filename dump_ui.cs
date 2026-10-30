 using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        foreach (var type in asm.MainModule.Types)
        {
            if (type.FullName.Contains("PrioritySignsTool") || type.FullName.Contains("JunctionRestrictionsTool") || type.FullName.Contains("RoadUI"))
            {
                foreach (var m in type.Methods)
                {
                    if (m.Name.Contains("Refresh") || m.Name.Contains("Unload"))
                    {
                        Console.WriteLine("=== Method: " + type.FullName + "." + m.Name + " ===");
                        foreach (var inst in m.Body.Instructions)
                        {
                            Console.WriteLine("    " + inst.OpCode + " " + inst.Operand);
                        }
                    }
                }
            }
        }
    }
}

using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string gameDir = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var assembly = AssemblyDefinition.ReadAssembly(gameDir + "/ColossalManaged.dll");
        foreach (var type in assembly.MainModule.Types)
        {
            if (type.FullName.Contains("EncodedArray"))
            {
                Console.WriteLine("Type: " + type.FullName);
                foreach (var nested in type.NestedTypes)
                {
                    Console.WriteLine("  Nested: " + nested.Name);
                    foreach (var m in nested.Methods)
                    {
                        if (m.Name.Contains("Read") || m.Name.Contains("Write") || m.Name.Contains("ctor"))
                        {
                            Console.WriteLine("    Method: " + m.Name);
                        }
                    }
                }
            }
        }
    }
}

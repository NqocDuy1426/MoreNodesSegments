using System;
using Mono.Cecil;

public class DecompileEncodedArray
{
    public static void Main()
    {
        string gameDir = Environment.GetEnvironmentVariable("HOME") + "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var resolver = new DefaultAssemblyResolver();
        resolver.AddSearchDirectory(gameDir);
        var readerParams = new ReaderParameters { AssemblyResolver = resolver };

        var asm = AssemblyDefinition.ReadAssembly(gameDir + "/ColossalManaged.dll", readerParams);
        foreach (var t in asm.MainModule.Types)
        {
            if (t.FullName.Contains("EncodedArray"))
            {
                Console.WriteLine("Type: " + t.FullName);
                foreach (var nested in t.NestedTypes)
                {
                    Console.WriteLine("Nested: " + nested.FullName);
                    foreach (var m in nested.Methods)
                    {
                        Console.WriteLine("  Method: " + m.FullName);
                        if (m.HasBody)
                        {
                            foreach (var inst in m.Body.Instructions)
                                Console.WriteLine("    " + inst);
                        }
                    }
                }
            }
        }
    }
}

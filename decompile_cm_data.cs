using System;
using Mono.Cecil;

public class DecompileCMData
{
    public static void Main()
    {
        string gameDir = Environment.GetEnvironmentVariable("HOME") + "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var resolver = new DefaultAssemblyResolver();
        resolver.AddSearchDirectory(gameDir);
        var readerParams = new ReaderParameters { AssemblyResolver = resolver };

        var asm = AssemblyDefinition.ReadAssembly(gameDir + "/Assembly-CSharp.dll", readerParams);
        var cmType = asm.MainModule.GetType("CombinedMilestone");
        foreach (var nested in cmType.NestedTypes)
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

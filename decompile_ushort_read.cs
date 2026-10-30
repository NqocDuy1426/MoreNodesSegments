using System;
using Mono.Cecil;

public class DecompileUShortRead
{
    public static void Main()
    {
        string gameDir = Environment.GetEnvironmentVariable("HOME") + "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var resolver = new DefaultAssemblyResolver();
        resolver.AddSearchDirectory(gameDir);
        var readerParams = new ReaderParameters { AssemblyResolver = resolver };

        var asm = AssemblyDefinition.ReadAssembly(gameDir + "/ColossalManaged.dll", readerParams);
        var eaType = asm.MainModule.GetType("ColossalFramework.IO.EncodedArray");
        foreach (var nested in eaType.NestedTypes)
        {
            if (nested.Name == "UShort")
            {
                foreach (var m in nested.Methods)
                {
                    if (m.Name == "Read")
                    {
                        Console.WriteLine("Method: " + m.FullName);
                        if (m.HasBody)
                        {
                            foreach (var inst in m.Body.Instructions)
                                Console.WriteLine("  " + inst);
                        }
                    }
                }
            }
        }
    }
}

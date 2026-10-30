using System;
using Mono.Cecil;

public class DecompileDSWriteArr
{
    public static void Main()
    {
        string gameDir = Environment.GetEnvironmentVariable("HOME") + "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var resolver = new DefaultAssemblyResolver();
        resolver.AddSearchDirectory(gameDir);
        var readerParams = new ReaderParameters { AssemblyResolver = resolver };

        var asm = AssemblyDefinition.ReadAssembly(gameDir + "/ColossalManaged.dll", readerParams);
        var dsType = asm.MainModule.GetType("ColossalFramework.IO.DataSerializer");
        foreach (var m in dsType.Methods)
        {
            if (m.Name.Contains("WriteObject"))
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

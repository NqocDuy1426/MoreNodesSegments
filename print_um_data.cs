using System;
using Mono.Cecil;

public class PrintUMData
{
    public static void Main()
    {
        string gameDir = Environment.GetEnvironmentVariable("HOME") + "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var resolver = new DefaultAssemblyResolver();
        resolver.AddSearchDirectory(gameDir);
        var readerParams = new ReaderParameters { AssemblyResolver = resolver };

        var asm = AssemblyDefinition.ReadAssembly(gameDir + "/Assembly-CSharp.dll", readerParams);
        var umType = asm.MainModule.GetType("UnlockManager");
        foreach (var nested in umType.NestedTypes)
        {
            if (nested.Name == "Data")
            {
                Console.WriteLine("UnlockManager.Data methods:");
                foreach (var m in nested.Methods)
                {
                    Console.WriteLine("  " + m.FullName);
                    if (m.Name == "Deserialize")
                    {
                        foreach (var inst in m.Body.Instructions)
                            Console.WriteLine("    " + inst);
                    }
                }
            }
        }
    }
}

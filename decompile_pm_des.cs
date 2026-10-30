using System;
using Mono.Cecil;

public class DecompilePMDes
{
    public static void Main()
    {
        string gameDir = Environment.GetEnvironmentVariable("HOME") + "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var resolver = new DefaultAssemblyResolver();
        resolver.AddSearchDirectory(gameDir);
        var readerParams = new ReaderParameters { AssemblyResolver = resolver };

        var asm = AssemblyDefinition.ReadAssembly(gameDir + "/Assembly-CSharp.dll", readerParams);
        var pmType = asm.MainModule.GetType("PathManager");
        foreach (var nested in pmType.NestedTypes)
        {
            if (nested.Name == "Data")
            {
                foreach (var m in nested.Methods)
                {
                    if (m.Name == "Deserialize")
                    {
                        Console.WriteLine("PathManager.Data.Deserialize:");
                        foreach (var inst in m.Body.Instructions)
                            Console.WriteLine("  " + inst);
                    }
                }
            }
        }
    }
}

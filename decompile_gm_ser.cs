using System;
using Mono.Cecil;

public class DecompileGMSer
{
    public static void Main()
    {
        string gameDir = Environment.GetEnvironmentVariable("HOME") + "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var resolver = new DefaultAssemblyResolver();
        resolver.AddSearchDirectory(gameDir);
        var readerParams = new ReaderParameters { AssemblyResolver = resolver };

        var asm = AssemblyDefinition.ReadAssembly(gameDir + "/Assembly-CSharp.dll", readerParams);
        var gmType = asm.MainModule.GetType("GuideManager");
        foreach (var nested in gmType.NestedTypes)
        {
            if (nested.Name == "Data")
            {
                foreach (var m in nested.Methods)
                {
                    if (m.Name == "Serialize")
                    {
                        Console.WriteLine("GuideManager.Data.Serialize:");
                        foreach (var inst in m.Body.Instructions)
                            Console.WriteLine("  " + inst);
                    }
                }
            }
        }
    }
}

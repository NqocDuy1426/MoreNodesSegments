using System;
using Mono.Cecil;

public class DecompileSimDes
{
    public static void Main()
    {
        string gameDir = Environment.GetEnvironmentVariable("HOME") + "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var resolver = new DefaultAssemblyResolver();
        resolver.AddSearchDirectory(gameDir);
        var readerParams = new ReaderParameters { AssemblyResolver = resolver };

        var asm = AssemblyDefinition.ReadAssembly(gameDir + "/Assembly-CSharp.dll", readerParams);
        var smType = asm.MainModule.GetType("SimulationManager");
        foreach (var nested in smType.NestedTypes)
        {
            if (nested.Name == "Data")
            {
                foreach (var m in nested.Methods)
                {
                    if (m.Name == "Deserialize")
                    {
                        Console.WriteLine("SimulationManager.Data.Deserialize:");
                        foreach (var inst in m.Body.Instructions)
                            Console.WriteLine("  " + inst);
                    }
                }
            }
        }
    }
}

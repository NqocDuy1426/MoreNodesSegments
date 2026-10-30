using System;
using Mono.Cecil;

public class DecompileSMDDes
{
    public static void Main()
    {
        string gameDir = Environment.GetEnvironmentVariable("HOME") + "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var resolver = new DefaultAssemblyResolver();
        resolver.AddSearchDirectory(gameDir);
        var readerParams = new ReaderParameters { AssemblyResolver = resolver };

        var asm = AssemblyDefinition.ReadAssembly(gameDir + "/Assembly-CSharp.dll", readerParams);
        var smdType = asm.MainModule.GetType("SimulationMetaData");
        foreach (var m in smdType.Methods)
        {
            if (m.Name == "Deserialize")
            {
                Console.WriteLine("SimulationMetaData.Deserialize:");
                foreach (var inst in m.Body.Instructions)
                    Console.WriteLine("  " + inst);
            }
        }
    }
}

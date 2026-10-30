using System;
using Mono.Cecil;

public class DecompileLP
{
    public static void Main()
    {
        string gameDir = Environment.GetEnvironmentVariable("HOME") + "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var resolver = new DefaultAssemblyResolver();
        resolver.AddSearchDirectory(gameDir);
        var readerParams = new ReaderParameters { AssemblyResolver = resolver };

        var asm = AssemblyDefinition.ReadAssembly(gameDir + "/Assembly-CSharp.dll", readerParams);
        var lpType = asm.MainModule.GetType("LoadingProfiler");
        foreach (var m in lpType.Methods)
        {
            if (m.Name.Contains("EndDeserialize"))
            {
                Console.WriteLine("LoadingProfiler method: " + m.FullName);
                if (m.HasBody)
                {
                    foreach (var inst in m.Body.Instructions)
                    {
                        Console.WriteLine("  " + inst);
                    }
                }
            }
        }
    }
}

using System;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;

public class DecompileUnlock
{
    public static void Main()
    {
        string cecilPath = "/Library/Frameworks/Mono.framework/Versions/6.12.0/lib/mono/gac/Mono.Cecil/0.11.1.0__0738eb9f132ed756/Mono.Cecil.dll";
        string gameDir = Environment.GetEnvironmentVariable("HOME") + "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        
        var resolver = new DefaultAssemblyResolver();
        resolver.AddSearchDirectory(gameDir);
        var readerParams = new ReaderParameters { AssemblyResolver = resolver };

        var asm = AssemblyDefinition.ReadAssembly(gameDir + "/Assembly-CSharp.dll", readerParams);
        var umType = asm.MainModule.GetType("UnlockManager");
        Console.WriteLine("UnlockManager found: " + (umType != null));
        foreach (var nested in umType.NestedTypes)
        {
            Console.WriteLine("Nested: " + nested.FullName);
            foreach (var m in nested.Methods)
            {
                if (m.Name.Contains("Deserialize"))
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

        foreach (var m in umType.Methods)
        {
            if (m.Name.Contains("DeserializeMilestones"))
            {
                Console.WriteLine("Method: " + m.FullName);
                if (m.HasBody)
                {
                    foreach (var inst in m.Body.Instructions)
                        Console.WriteLine("    " + inst);
                }
            }
        }
    }
}

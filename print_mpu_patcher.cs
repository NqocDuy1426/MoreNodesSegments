using System;
using Mono.Cecil;

public class PrintMPUPatcher
{
    public static void Main()
    {
        string mpuPath = Environment.GetEnvironmentVariable("HOME") + "/Library/Application Support/Steam/steamapps/workshop/content/255710/2710657019/MorePathUnits.dll";
        var asm = AssemblyDefinition.ReadAssembly(mpuPath);
        foreach (var t in asm.MainModule.Types)
        {
            if (t.Name.Contains("Patch") || t.Name.Contains("Patcher"))
            {
                Console.WriteLine("Type: " + t.FullName);
                foreach (var m in t.Methods)
                {
                    Console.WriteLine("  Method: " + m.FullName);
                    if (m.HasBody)
                    {
                        foreach (var inst in m.Body.Instructions)
                        {
                            Console.WriteLine("    " + inst);
                        }
                    }
                }
            }
        }
    }
}

using System;
using Mono.Cecil;

public class PrintMPUPre
{
    public static void Main()
    {
        string mpuPath = Environment.GetEnvironmentVariable("HOME") + "/Library/Application Support/Steam/steamapps/workshop/content/255710/2710657019/MorePathUnits.dll";
        var asm = AssemblyDefinition.ReadAssembly(mpuPath);
        var t = asm.MainModule.GetType("MorePathUnits.PathDeserialize");
        foreach (var m in t.Methods)
        {
            if (m.Name == "Prefix")
            {
                Console.WriteLine("Method: " + m.FullName);
                for (int i = 0; i < Math.Min(60, m.Body.Instructions.Count); i++)
                {
                    Console.WriteLine("  " + m.Body.Instructions[i]);
                }
            }
        }
    }
}

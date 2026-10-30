using System;
using Mono.Cecil;

public class PrintMPUTrans
{
    public static void Main()
    {
        string mpuPath = Environment.GetEnvironmentVariable("HOME") + "/Library/Application Support/Steam/steamapps/workshop/content/255710/2710657019/MorePathUnits.dll";
        var asm = AssemblyDefinition.ReadAssembly(mpuPath);
        var t = asm.MainModule.GetType("MorePathUnits.PathDeserialize");
        foreach (var m in t.Methods)
        {
            Console.WriteLine("Method: " + m.FullName);
            if (m.Name.Contains("Transpiler") || m.Name.Contains("DeserializeSize"))
            {
                foreach (var inst in m.Body.Instructions)
                    Console.WriteLine("  " + inst);
            }
        }
    }
}

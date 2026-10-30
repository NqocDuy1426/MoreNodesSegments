using System;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string p = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + 
            "/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(p);

        var type = asm.MainModule.GetType("TrafficManager.Manager.Impl.TrafficMeasurementManager");
        foreach (var m in type.Methods)
        {
            if (m.Body == null) continue;
            foreach (var inst in m.Body.Instructions)
            {
                var fr = inst.Operand as FieldReference;
                if (fr != null && fr.Name == "meanSpeeds")
                {
                    Console.WriteLine($"=== {m.Name} uses meanSpeeds ===");
                    foreach (var i2 in m.Body.Instructions)
                        Console.WriteLine($"  IL_{i2.Offset:X4}: {i2.OpCode} {i2.Operand}");
                    break;
                }
            }
        }
    }
}

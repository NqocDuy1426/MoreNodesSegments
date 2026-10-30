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
            if (m.Name == "GetDirIndex")
            {
                Console.WriteLine($"=== {m.Name} ({m.Parameters.Count} params) ===");
                foreach (var p2 in m.Parameters) Console.Write($"{p2.ParameterType.Name} {p2.Name}, ");
                Console.WriteLine();
                foreach (var inst in m.Body.Instructions)
                    Console.WriteLine($"  IL_{inst.Offset:X4}: {inst.OpCode} {inst.Operand}");
            }
        }
    }
}

using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var gameDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + 
            "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var asm = AssemblyDefinition.ReadAssembly(gameDir + "/Assembly-CSharp.dll");

        var ns = asm.MainModule.GetType("NetSegment");
        foreach (var f in ns.Fields)
        {
            if (f.Name.IndexOf("traffic", StringComparison.OrdinalIgnoreCase) >= 0 ||
                f.Name.IndexOf("flow", StringComparison.OrdinalIgnoreCase) >= 0 ||
                f.Name.IndexOf("density", StringComparison.OrdinalIgnoreCase) >= 0 ||
                f.Name.IndexOf("color", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Console.WriteLine($"NetSegment field: {f.FieldType.Name} {f.Name}");
            }
        }

        foreach (var m in ns.Methods)
        {
            if (m.Name == "AddTraffic")
            {
                Console.WriteLine($"\n=== NetSegment.AddTraffic ===");
                foreach (var inst in m.Body.Instructions)
                {
                    Console.WriteLine($"  IL_{inst.Offset:X4}: {inst.OpCode} {inst.Operand}");
                }
            }
        }
    }
}

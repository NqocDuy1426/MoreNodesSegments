using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string gameDir = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var assembly = AssemblyDefinition.ReadAssembly(gameDir + "/ColossalManaged.dll");
        var ea = assembly.MainModule.GetType("ColossalFramework.IO.EncodedArray");
        foreach (var nested in ea.NestedTypes)
        {
            if (nested.Name == "UShort")
            {
                foreach (var m in nested.Methods)
                {
                    if (m.Name == "BeginWrite" || m.Name == "BeginRead")
                    {
                        Console.WriteLine("=== Method: " + m.FullName + " ===");
                        foreach (var inst in m.Body.Instructions)
                        {
                            Console.WriteLine("  " + inst.OpCode + " " + inst.Operand);
                        }
                    }
                }
            }
        }
    }
}

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
        
        var type = asm.MainModule.GetType("TransportLine");
        foreach (var m in type.Methods)
        {
            if (m.Name == "SimulationStep" && m.Parameters.Count == 1)
            {
                Console.WriteLine("=== TransportLine.SimulationStep ===");
                foreach (var inst in m.Body.Instructions)
                {
                    if (inst.Offset >= 0x0150 && inst.Offset <= 0x0455)
                        Console.WriteLine($"  IL_{inst.Offset:X4}: {inst.OpCode} {inst.Operand}");
                }
            }
        }
    }
}

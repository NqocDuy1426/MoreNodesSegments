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
        
        DumpMethod(asm, "TransportLine", "SimulationStep", 0x0380, 0x0500);
        DumpMethod(asm, "PlayerNetAI", "SimulationStep", 0x0050, 0x0100);
    }

    static void DumpMethod(AssemblyDefinition asm, string typeName, string methodName, int minOffset, int maxOffset)
    {
        Console.WriteLine($"\n================== {typeName}.{methodName} ({minOffset:X4} - {maxOffset:X4}) ==================");
        var type = asm.MainModule.GetType(typeName);
        foreach (var m in type.Methods)
        {
            if (m.Name == methodName && m.Body != null)
            {
                foreach (var inst in m.Body.Instructions)
                {
                    if (inst.Offset >= minOffset && inst.Offset <= maxOffset)
                    {
                        Console.WriteLine($"  IL_{inst.Offset:X4}: {inst.OpCode} {inst.Operand}");
                    }
                }
            }
        }
    }
}

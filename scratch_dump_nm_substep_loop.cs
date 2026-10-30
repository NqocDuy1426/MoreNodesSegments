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

        var type = asm.MainModule.GetType("NetManager");
        foreach (var m in type.Methods)
        {
            if (m.Name == "SimulationStepImpl")
            {
                for (int i = 0x0480; i <= 0x04C0; i++)
                {
                    // find instruction near offset
                }
                foreach (var inst in m.Body.Instructions)
                {
                    if (inst.Offset >= 0x0480 && inst.Offset <= 0x04D0)
                        Console.WriteLine($"  IL_{inst.Offset:X4}: {inst.OpCode} {inst.Operand}");
                    if (inst.Offset >= 0x0680 && inst.Offset <= 0x06D0)
                        Console.WriteLine($"  IL_{inst.Offset:X4}: {inst.OpCode} {inst.Operand}");
                }
            }
        }
    }
}

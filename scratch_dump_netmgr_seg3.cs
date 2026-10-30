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

        var t = asm.MainModule.GetType("NetManager");
        foreach (var m in t.Methods)
        {
            if (m.Name == "SimulationStepImpl")
            {
                foreach (var inst in m.Body.Instructions)
                {
                    if (inst.Offset >= 0x0480 && inst.Offset <= 0x0500)
                    {
                        Console.WriteLine($"  IL_{inst.Offset:X4}: {inst.OpCode} {inst.Operand}");
                    }
                }
            }
        }
    }
}

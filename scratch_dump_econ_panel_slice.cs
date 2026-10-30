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

        var t = asm.MainModule.GetType("EconomyPanel");
        foreach (var m in t.Methods)
        {
            if (m.Name == "InitializePolls")
            {
                foreach (var inst in m.Body.Instructions)
                {
                    if (inst.Offset >= 0x0980 && inst.Offset <= 0x0A20)
                    {
                        Console.WriteLine($"  IL_{inst.Offset:X4}: {inst.OpCode} {inst.Operand}");
                    }
                }
            }
        }
    }
}

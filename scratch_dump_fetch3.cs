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

        var type = asm.MainModule.GetType("EconomyManager");
        foreach (var m in type.Methods)
        {
            if (m.Name == "FetchResource" && m.Parameters.Count == 3)
            {
                Console.WriteLine($"=== FetchResource (3 params) ===");
                foreach (var inst in m.Body.Instructions)
                {
                    Console.WriteLine($"  IL_{inst.Offset:X4}: {inst.OpCode} {inst.Operand}");
                }
            }
        }
    }
}

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

        var t = asm.MainModule.GetType("ItemClass");
        foreach (var m in t.Methods)
        {
            if (m.Name.Contains("SubServiceIndex") || m.Name.Contains("ServiceIndex"))
            {
                Console.WriteLine($"=== ItemClass.{m.Name} ===");
                if (m.Body != null)
                {
                    foreach (var inst in m.Body.Instructions)
                    {
                        Console.WriteLine($"  IL_{inst.Offset:X4}: {inst.OpCode} {inst.Operand}");
                    }
                }
            }
        }
    }
}

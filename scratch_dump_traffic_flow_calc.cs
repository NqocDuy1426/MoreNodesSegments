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

        var type = asm.MainModule.GetType("VehicleManager");
        foreach (var m in type.Methods)
        {
            if (m.Body == null) continue;
            foreach (var inst in m.Body.Instructions)
            {
                var fr = inst.Operand as FieldReference;
                if (fr != null && fr.Name == "m_lastTrafficFlow")
                {
                    Console.WriteLine($"=== VehicleManager.{m.Name} modifies/reads m_lastTrafficFlow ===");
                    foreach (var i2 in m.Body.Instructions)
                    {
                        Console.WriteLine($"  IL_{i2.Offset:X4}: {i2.OpCode} {i2.Operand}");
                    }
                    break;
                }
            }
        }
    }
}

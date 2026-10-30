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

        foreach (var t in asm.MainModule.Types)
        {
            if (t.IsSubclassOf("VehicleAI") || t.Name == "VehicleAI")
            {
                foreach (var m in t.Methods)
                {
                    if (m.Name == "GetTicketPrice")
                    {
                        Console.WriteLine($"=== {t.Name}.{m.Name} ===");
                        foreach (var inst in m.Body.Instructions)
                        {
                            Console.WriteLine($"  IL_{inst.Offset:X4}: {inst.OpCode} {inst.Operand}");
                        }
                    }
                }
            }
        }
    }
}
static class Extensions
{
    public static bool IsSubclassOf(this TypeDefinition type, string baseName)
    {
        var b = type.BaseType;
        while (b != null)
        {
            if (b.Name == baseName) return true;
            try { b = b.Resolve()?.BaseType; } catch { break; }
        }
        return false;
    }
}

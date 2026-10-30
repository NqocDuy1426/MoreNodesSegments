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

        foreach (var type in asm.MainModule.Types)
        {
            if (type.Name.Contains("TrainAI"))
            {
                foreach (var m in type.Methods)
                {
                    if (m.Name.Contains("GetPassengerCapacity"))
                    {
                        Console.WriteLine($"=== {type.FullName}.{m.Name} ===");
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
    }
}

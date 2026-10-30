using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var gameDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + 
            "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var asm = AssemblyDefinition.ReadAssembly(gameDir + "/Assembly-CSharp.dll");
        
        foreach (var type in asm.MainModule.Types)
        {
            if (type.Name.Contains("NetAI") || type.Name.Contains("Track"))
            {
                foreach (var m in type.Methods)
                {
                    if (m.Name.Contains("GetMaintenanceCost"))
                    {
                        Console.WriteLine($"\n================== {type.FullName}.{m.Name} ==================");
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

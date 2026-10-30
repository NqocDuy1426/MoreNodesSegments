using System;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var gameDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + 
            "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var asm = AssemblyDefinition.ReadAssembly(gameDir + "/Assembly-CSharp.dll");

        var econType = asm.MainModule.GetType("EconomyManager");
        using (var sw = new StreamWriter("impl_dump.txt"))
        {
            foreach (var m in econType.Methods)
            {
                if (m.Name == "GetIncomeAndExpensesImpl" || m.Name == "SimulationStepImpl")
                {
                    sw.WriteLine("=== Method: " + m.Name + " ===");
                    if (m.Body != null)
                    {
                        foreach (var inst in m.Body.Instructions)
                        {
                            sw.WriteLine($"  IL_{inst.Offset:X4}: {inst.OpCode} {inst.Operand}");
                        }
                    }
                }
            }
        }
        Console.WriteLine("Done writing impl_dump.txt");
    }
}

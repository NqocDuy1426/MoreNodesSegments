using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string dll = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/ColossalManaged.dll";
        var mod = ModuleDefinition.ReadModule(dll);
        var t = mod.GetType("ColossalFramework.Packaging.PackageManager");
        foreach (var m in t.Methods)
        {
            if (m.Name == "Update" && m.Parameters.Count == 1)
            {
                Console.WriteLine("\nMethod: " + m.FullName);
                foreach (var instr in m.Body.Instructions)
                {
                    Console.WriteLine(string.Format("  IL_{0:x4}: {1} {2}", instr.Offset, instr.OpCode, instr.Operand));
                }
            }
        }
    }
}

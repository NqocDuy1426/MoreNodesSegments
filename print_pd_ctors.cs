using System;
using System.IO;
using System.Linq;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Desktop/CSL_Mods_Backup_16bit_FINAL/Local_2804719780/TransferManagerCE.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var type = asm.MainModule.Types.FirstOrDefault(t => t.FullName == "TransferManagerCore.PathDistance");
        foreach (var m in type.Methods.Where(m => m.Name == ".ctor"))
        {
            Console.WriteLine(".ctor (" + string.Join(", ", m.Parameters.Select(p => p.ParameterType.FullName + " " + p.Name)) + ")");
        }
    }
}

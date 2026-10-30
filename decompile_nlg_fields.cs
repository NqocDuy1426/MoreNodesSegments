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
        foreach (var type in asm.MainModule.Types.Where(t => t.FullName.Contains("NodeLink")))
        {
            Console.WriteLine("Type: " + type.FullName);
            foreach (var f in type.Fields)
            {
                Console.WriteLine("  Field: " + f.Name + " : " + f.FieldType.FullName);
            }
        }
    }
}

using System;
using System.IO;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + 
            "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/ColossalManaged.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        TypeDefinition td = asm.MainModule.GetType("ColossalFramework.IO.DataSerializer");
        using (var sw = new StreamWriter("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/ds_deser_dump.txt"))
        {
            foreach (var m in td.Methods)
            {
                if (m.Name == "Deserialize" && m.HasGenericParameters)
                {
                    sw.WriteLine("METHOD: " + m.FullName);
                    foreach (var inst in m.Body.Instructions)
                    {
                        sw.WriteLine(string.Format("  IL_{0:X4}: {1} {2}", inst.Offset, inst.OpCode, inst.Operand));
                    }
                }
            }
        }
        Console.WriteLine("Done writing ds_deser_dump.txt");
    }
}

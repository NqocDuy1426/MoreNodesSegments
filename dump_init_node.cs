using System;
using System.IO;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + 
            "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        TypeDefinition td = asm.MainModule.GetType("NetManager");
        foreach (var m in td.Methods)
        {
            if (m.Name == "InitializeNode" || m.Name == "InitializeSegment")
            {
                Console.WriteLine("METHOD: " + m.FullName);
                foreach (var inst in m.Body.Instructions)
                {
                    Console.WriteLine(string.Format("  IL_{0:X4}: {1} {2}", inst.Offset, inst.OpCode, inst.Operand));
                }
            }
        }
    }
}

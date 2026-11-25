using System;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + 
            "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        TypeDefinition td = asm.MainModule.GetType("NetManager/Data");
        MethodDefinition md = null;
        foreach (var m in td.Methods)
        {
            if (m.Name == "Deserialize") md = m;
        }

        Console.WriteLine("Method: " + md.FullName);
        using (var sw = new StreamWriter("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/net_deser_cecil.txt"))
        {
            foreach (var inst in md.Body.Instructions)
            {
                sw.WriteLine(string.Format("IL_{0:X4}: {1} {2}", inst.Offset, inst.OpCode, inst.Operand));
            }
        }
        Console.WriteLine("Done writing net_deser_cecil.txt. Instructions: " + md.Body.Instructions.Count);
    }
}

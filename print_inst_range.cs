using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + 
            "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        TypeDefinition td = asm.MainModule.GetType("NetManager/Data");
        MethodDefinition md = null;
        foreach (var m in td.Methods) if (m.Name == "Deserialize") md = m;

        for (int i = 2720; i < 2740; i++)
        {
            var inst = md.Body.Instructions[i];
            Console.WriteLine(string.Format("{0}: IL_{1:X4} {2} {3} (type: {4})", 
                i, inst.Offset, inst.OpCode, inst.Operand, inst.Operand?.GetType().Name));
        }
    }
}

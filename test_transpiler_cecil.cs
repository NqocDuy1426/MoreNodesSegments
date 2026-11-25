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
        foreach (var m in td.Methods) if (m.Name == "Deserialize") md = m;

        var insts = md.Body.Instructions;
        int targetIdx = -1;
        for (int i = 0; i < insts.Count; i++)
        {
            if (insts[i].Offset == 0x1F91)
            {
                targetIdx = i;
                break;
            }
        }

        Console.WriteLine("Target index at 0x1F91: " + targetIdx);
        Console.WriteLine("Instruction at 0x1F91: " + insts[targetIdx].OpCode);
        Console.WriteLine("Instruction before: " + insts[targetIdx - 1].OpCode);
    }
}

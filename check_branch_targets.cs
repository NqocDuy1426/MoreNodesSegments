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

        int targetOffset = 0x1F91;
        Console.WriteLine("Checking branches into [0x1F91, 0x2093] from outside...");
        foreach (var inst in md.Body.Instructions)
        {
            if (inst.Offset < targetOffset)
            {
                if (inst.Operand is Mono.Cecil.Cil.Instruction targetInst)
                {
                    if (targetInst.Offset >= targetOffset)
                    {
                        Console.WriteLine(string.Format("Branch at IL_{0:X4} ({1}) jumps to IL_{2:X4}", inst.Offset, inst.OpCode, targetInst.Offset));
                    }
                }
            }
        }
        Console.WriteLine("Done checking.");
    }
}

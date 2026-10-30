using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var nm = asm.MainModule.GetType("NetManager");
        foreach (var m in nm.Methods)
        {
            if (m.Name == "InitializeNode" || m.Name == "InitializeSegment")
            {
                Console.WriteLine("=== Method: " + m.Name + " ===");
                foreach (var inst in m.Body.Instructions)
                {
                    if (inst.OpCode.ToString().Contains("elem") || inst.OpCode == OpCodes.Ldfld || inst.OpCode == OpCodes.Call || inst.OpCode == OpCodes.Callvirt)
                    {
                        Console.WriteLine($"  IL_{inst.Offset:X4}: {inst.OpCode} {inst.Operand}");
                    }
                }
            }
        }
    }
}

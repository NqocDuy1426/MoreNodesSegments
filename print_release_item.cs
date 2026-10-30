using System;
using System.Linq;
using Mono.Cecil;

class PrintReleaseItem
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var arr16 = asm.MainModule.Types.FirstOrDefault(t => t.Name == "Array16`1");
        var m = arr16.Methods.FirstOrDefault(x => x.Name == "ReleaseItem");
        Console.WriteLine("Array16.ReleaseItem:");
        foreach (var inst in m.Body.Instructions)
        {
            Console.WriteLine(string.Format("  {0}: {1} {2}", inst.Offset.ToString("X4"), inst.OpCode, inst.Operand));
        }
    }
}

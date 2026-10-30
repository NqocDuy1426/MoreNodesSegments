using System;
using System.Linq;
using Mono.Cecil;

class PrintTranspiler
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("content/MoreNetworkCapacity.Core.dll");
        var patchType = asm.MainModule.Types.FirstOrDefault(t => t.FullName.Contains("NetManagerDeserializePatch"));
        var transpiler = patchType.Methods.FirstOrDefault(m => m.Name == "Transpiler");
        Console.WriteLine("Transpiler instructions: " + transpiler.Body.Instructions.Count);
        foreach (var inst in transpiler.Body.Instructions)
        {
            Console.WriteLine(string.Format("  {0}: {1} {2}", inst.Offset.ToString("X4"), inst.OpCode, inst.Operand));
        }
    }
}

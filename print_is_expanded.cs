using System;
using System.Linq;
using Mono.Cecil;

class PrintIsExpanded
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("content/MoreNetworkCapacity.Core.dll");
        var customizer = asm.MainModule.Types.FirstOrDefault(t => t.FullName.Contains("NetManagerCustomizer"));
        var m = customizer.Methods.FirstOrDefault(x => x.Name == "IsExpanded");
        Console.WriteLine("Method: " + m.Name);
        foreach (var inst in m.Body.Instructions)
        {
            Console.WriteLine(string.Format("  {0}: {1} {2}", inst.Offset.ToString("X4"), inst.OpCode, inst.Operand));
        }
    }
}

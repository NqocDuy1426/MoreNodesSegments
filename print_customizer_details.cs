using System;
using System.Linq;
using Mono.Cecil;

class PrintDetails
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("content/MoreNetworkCapacity.Core.dll");
        var type = asm.MainModule.Types.FirstOrDefault(t => t.FullName.Contains("NetManagerCustomizer"));
        foreach (var m in type.Methods.Where(m => m.Name.Contains("Reset") || m.Name.Contains("EnsureExpanded")))
        {
            Console.WriteLine("Method: " + m.Name);
            if (m.HasBody)
            {
                foreach (var inst in m.Body.Instructions)
                {
                    Console.WriteLine(string.Format("  {0}: {1} {2}", inst.Offset.ToString("X4"), inst.OpCode, inst.Operand));
                }
            }
        }
    }
}

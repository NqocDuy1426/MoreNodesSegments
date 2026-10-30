using System;
using System.Linq;
using Mono.Cecil;

class PrintCreateOrdered
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("content/MoreNetworkCapacity.Core.dll");
        var customizer = asm.MainModule.Types.FirstOrDefault(t => t.FullName.Contains("NetManagerCustomizer"));
        foreach (var m in customizer.Methods.Where(m => m.Name.Contains("CreateOrdered")))
        {
            Console.WriteLine("Method: " + m.Name);
            foreach (var inst in m.Body.Instructions)
            {
                Console.WriteLine(string.Format("  {0}: {1} {2}", inst.Offset.ToString("X4"), inst.OpCode, inst.Operand));
            }
        }
    }
}

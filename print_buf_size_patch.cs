using System;
using System.Linq;
using Mono.Cecil;

class PrintBufSizePatch
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("content/MoreNetworkCapacity.Core.dll");
        var type = asm.MainModule.Types.FirstOrDefault(t => t.FullName.Contains("NetworkBufferSizePatch"));
        if (type != null)
        {
            Console.WriteLine("Type: " + type.FullName);
            foreach (var m in type.Methods)
            {
                Console.WriteLine("  Method: " + m.Name);
                if (m.HasBody)
                {
                    foreach (var inst in m.Body.Instructions)
                    {
                        Console.WriteLine(string.Format("    {0}: {1} {2}", inst.Offset.ToString("X4"), inst.OpCode, inst.Operand));
                    }
                }
            }
        }
    }
}

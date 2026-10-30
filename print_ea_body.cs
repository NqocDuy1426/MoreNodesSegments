using System;
using System.Linq;
using Mono.Cecil;

class PrintEABody
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("ColossalManaged.dll");
        var ea = asm.MainModule.Types.FirstOrDefault(t => t.FullName == "ColossalFramework.IO.EncodedArray");
        var uintType = ea.NestedTypes.FirstOrDefault(t => t.Name == "UInt");
        foreach (var m in uintType.Methods)
        {
            if (m.Name == "BeginWrite" || m.Name == "BeginRead" || m.Name == "EndRead" || m.Name == "EndWrite")
            {
                Console.WriteLine("Method: " + m.Name);
                foreach (var inst in m.Body.Instructions)
                {
                    Console.WriteLine(string.Format("  {0}: {1} {2}", inst.Offset.ToString("X4"), inst.OpCode, inst.Operand));
                }
            }
        }
    }
}

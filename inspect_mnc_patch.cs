using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class InspectMncPatch
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("content/MoreNetworkCapacity.Core.dll");
        foreach (var type in asm.MainModule.Types)
        {
            if (type.Name.Contains("NetManager") || type.Name.Contains("Deserialize"))
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
}

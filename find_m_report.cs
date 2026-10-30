using System;
using System.Linq;
using Mono.Cecil;

class FindReport
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("content/MoreNetworkCapacity.Core.dll");
        foreach (var type in asm.MainModule.Types)
        {
            foreach (var m in type.Methods)
            {
                if (m.HasBody)
                {
                    foreach (var inst in m.Body.Instructions)
                    {
                        if (inst.OpCode.ToString() == "ldstr" && inst.Operand != null && inst.Operand.ToString().Contains("Expansion enabled"))
                        {
                            Console.WriteLine(string.Format("Type: {0}, Method: {1}", type.FullName, m.Name));
                        }
                    }
                }
            }
        }
    }
}

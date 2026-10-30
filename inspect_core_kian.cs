using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Linq;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("MoreNetworkCapacity.Core.dll");
        foreach (var t in asm.MainModule.GetTypes())
        {
            if (t.Name.Contains("StaticBufferCompatibility") || t.Name.Contains("ThirdPartyCapacityCompatibility"))
            {
                Console.WriteLine("Type: " + t.FullName);
                foreach (var m in t.Methods)
                {
                    Console.WriteLine("  Method: " + m.Name);
                    if (m.Name.Contains("Kian") || m.Name.Contains("ApplyEarly"))
                    {
                        if (m.HasBody)
                        {
                            foreach (var inst in m.Body.Instructions)
                            {
                                Console.WriteLine("    " + inst.OpCode + " " + inst.Operand);
                            }
                        }
                    }
                }
            }
        }
    }
}

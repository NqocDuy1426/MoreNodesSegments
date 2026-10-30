using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/Assembly-CSharp.dll");
        var a16 = asm.MainModule.GetType("Array16`1");
        foreach (var m in a16.Methods)
        {
            if (m.Name == "ReleaseItem" || m.Name == "CreateItem" || m.Name == "ClearUnused")
            {
                Console.WriteLine("Method: " + m.Name);
                foreach (var inst in m.Body.Instructions)
                    Console.WriteLine("  " + inst);
            }
        }
    }
}

using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/Assembly-CSharp.dll");
        var pc = asm.MainModule.GetType("PrefabCollection`1");
        foreach (var m in pc.Methods)
        {
            if (m.Name == "Deserialize")
            {
                Console.WriteLine("PrefabCollection.Deserialize (" + m.Parameters.Count + " params):");
                foreach (var inst in m.Body.Instructions)
                    Console.WriteLine("  " + inst);
            }
        }
    }
}

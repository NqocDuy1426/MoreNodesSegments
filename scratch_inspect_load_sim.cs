using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var resolver = new DefaultAssemblyResolver();
        resolver.AddSearchDirectory("/Users/nqocduy/CS_MoreNetworkCapacity_Renew");
        var readerParams = new ReaderParameters { AssemblyResolver = resolver };
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/Assembly-CSharp.dll", readerParams);

        // Find LoadingManager.<LoadSimulationData>
        var lm = asm.MainModule.GetType("LoadingManager");
        Console.WriteLine("LoadingManager nested types:");
        foreach (var t in lm.NestedTypes)
        {
            if (t.Name.Contains("LoadSimulationData"))
            {
                Console.WriteLine("Type: " + t.FullName);
                foreach (var m in t.Methods)
                {
                    Console.WriteLine("  Method: " + m.Name);
                    if (m.Name == "MoveNext")
                    {
                        foreach (var inst in m.Body.Instructions)
                        {
                            if (inst.OpCode == OpCodes.Call || inst.OpCode == OpCodes.Callvirt)
                            {
                                if (inst.Operand.ToString().Contains("Deserialize"))
                                    Console.WriteLine("    " + inst);
                            }
                        }
                    }
                }
            }
        }
    }
}

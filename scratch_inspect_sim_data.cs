using System;
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

        var sm = asm.MainModule.GetType("SimulationManager");
        foreach (var nested in sm.NestedTypes)
        {
            if (nested.Name == "Data")
            {
                foreach (var m in nested.Methods)
                {
                    if (m.Name == "Deserialize")
                    {
                        Console.WriteLine("SimulationManager.Data.Deserialize instructions:");
                        foreach (var inst in m.Body.Instructions)
                        {
                            if (inst.OpCode == OpCodes.Call || inst.OpCode == OpCodes.Callvirt || inst.OpCode == OpCodes.Ldstr)
                            {
                                Console.WriteLine("  " + inst);
                            }
                        }
                    }
                }
            }
        }
    }
}

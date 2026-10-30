using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string corePath = "/Users/nqocduy/CS_MoreNetworkCapacity_Renew/MoreNetworkCapacity.Core.dll";
        var assembly = AssemblyDefinition.ReadAssembly(corePath);
        foreach (var t in assembly.MainModule.Types)
        {
            foreach (var m in t.Methods)
            {
                if (m.Body != null)
                {
                    foreach (var ins in m.Body.Instructions)
                    {
                        if (ins.ToString().Contains("TMPEIndexPatches"))
                        {
                            Console.WriteLine(t.FullName + "::" + m.Name + " -> " + ins.ToString());
                        }
                    }
                }
            }
        }
    }
}

using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string gameDir = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var assembly = AssemblyDefinition.ReadAssembly(gameDir + "/Assembly-CSharp.dll");
        var iterB = assembly.MainModule.GetType("LoadingManager/<LoadSimulationData>c__IteratorB");
        foreach (var m in iterB.Methods)
        {
            if (m.Name == "MoveNext")
            {
                foreach (var inst in m.Body.Instructions)
                {
                    if (inst.OpCode == OpCodes.Ldstr)
                    {
                        Console.WriteLine("  str: " + inst.Operand);
                    }
                }
            }
        }
    }
}

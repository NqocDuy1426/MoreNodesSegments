using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string gameDir = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var assembly = AssemblyDefinition.ReadAssembly(gameDir + "/Assembly-CSharp.dll");
        var lm = assembly.MainModule.GetType("LoadingManager");
        foreach (var m in lm.Methods)
        {
            if (m.Name.Contains("LoadSimulationData") || m.Name.Contains("LoadData"))
            {
                Console.WriteLine("Method: " + m.FullName);
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

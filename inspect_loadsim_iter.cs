using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string gameDir = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var assembly = AssemblyDefinition.ReadAssembly(gameDir + "/Assembly-CSharp.dll");
        foreach (var type in assembly.MainModule.Types)
        {
            if (type.FullName.Contains("LoadSimulationData") && type.IsNested)
            {
                Console.WriteLine("Type: " + type.FullName);
                foreach (var m in type.Methods)
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
    }
}

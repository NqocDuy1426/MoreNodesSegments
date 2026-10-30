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
            foreach (var m in type.Methods)
            {
                if (m.HasBody)
                {
                    foreach (var inst in m.Body.Instructions)
                    {
                        if (inst.OpCode == OpCodes.Ldstr && inst.Operand is string s && s.Contains("Simulation failed"))
                        {
                            Console.WriteLine(string.Format("Found in {0}::{1} -> \"{2}\"", type.FullName, m.Name, s));
                        }
                    }
                }
            }
        }
    }
}

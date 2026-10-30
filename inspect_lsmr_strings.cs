using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string lsmrPath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/2858591409/LoadingScreenModRevisited.dll";
        var assembly = AssemblyDefinition.ReadAssembly(lsmrPath);
        foreach (var type in assembly.MainModule.Types)
        {
            foreach (var m in type.Methods)
            {
                if (m.HasBody)
                {
                    foreach (var inst in m.Body.Instructions)
                    {
                        if (inst.OpCode == OpCodes.Ldstr && inst.Operand is string s)
                        {
                            if (s.IndexOf("Simulation", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                s.IndexOf("failed", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                Console.WriteLine(string.Format("{0}::{1} -> \"{2}\"", type.Name, m.Name, s));
                            }
                        }
                    }
                }
            }
        }
    }
}

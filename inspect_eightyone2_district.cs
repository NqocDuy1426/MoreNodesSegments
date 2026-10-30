using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string eoPath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/2862121823/EightyOne2.dll";
        var assembly = AssemblyDefinition.ReadAssembly(eoPath);
        foreach (var type in assembly.MainModule.Types)
        {
            if (type.FullName.Contains("District"))
            {
                Console.WriteLine("Type: " + type.FullName);
                foreach (var m in type.Methods)
                {
                    Console.WriteLine("  Method: " + m.FullName);
                    if (m.HasBody)
                    {
                        foreach (var inst in m.Body.Instructions)
                        {
                            if (inst.OpCode == OpCodes.Ldfld || inst.OpCode == OpCodes.Ldsfld || inst.OpCode == OpCodes.Call)
                            {
                                Console.WriteLine("    " + inst.OpCode + " " + inst.Operand);
                            }
                        }
                    }
                }
            }
        }
    }
}

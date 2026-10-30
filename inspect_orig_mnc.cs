using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string origPath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/3814019901/MoreNetworkCapacity.dll";
        var assembly = AssemblyDefinition.ReadAssembly(origPath);
        foreach (var type in assembly.MainModule.Types)
        {
            foreach (var m in type.Methods)
            {
                if (m.HasBody)
                {
                    foreach (var inst in m.Body.Instructions)
                    {
                        if (inst.OpCode == OpCodes.Call || inst.OpCode == OpCodes.Callvirt)
                        {
                            if (inst.Operand is MethodReference mr && mr.Name.Contains("SetExpandedSave"))
                            {
                                Console.WriteLine(string.Format("{0}::{1} calls SetExpandedSave", type.FullName, m.Name));
                            }
                        }
                    }
                }
            }
        }
    }
}

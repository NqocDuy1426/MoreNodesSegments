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
            CheckType(type);
        }
    }

    static void CheckType(TypeDefinition type)
    {
        foreach (var m in type.Methods)
        {
            if (m.HasBody)
            {
                foreach (var inst in m.Body.Instructions)
                {
                    if (inst.OpCode == OpCodes.Call || inst.OpCode == OpCodes.Callvirt)
                    {
                        if (inst.Operand is MethodReference mr && mr.DeclaringType.Name == "LoadingProfiler" && mr.Name == "BeginDeserialize")
                        {
                            Console.WriteLine(string.Format("{0}::{1} calls BeginDeserialize", type.FullName, m.Name));
                        }
                    }
                }
            }
        }
        foreach (var nested in type.NestedTypes)
        {
            CheckType(nested);
        }
    }
}

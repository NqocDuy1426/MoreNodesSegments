using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        foreach (var type in asm.MainModule.Types)
        {
            foreach (var m in type.Methods)
            {
                if (!m.HasBody) continue;
                foreach (var ins in m.Body.Instructions)
                {
                    if (ins.OpCode == OpCodes.Call || ins.OpCode == OpCodes.Callvirt)
                    {
                        var target = ins.Operand as MethodReference;
                        if (target != null && target.Name == "GetTrafficLightState")
                        {
                            Console.WriteLine($"{type.Name}.{m.Name} calls GetTrafficLightState");
                        }
                    }
                }
            }
        }
    }
}

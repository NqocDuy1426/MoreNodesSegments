using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        foreach (var t in asm.MainModule.Types)
        {
            foreach (var m in t.Methods)
            {
                if (m.Body == null) continue;
                for (int i = 0; i < m.Body.Instructions.Count; i++)
                {
                    var inst = m.Body.Instructions[i];
                    if (inst.OpCode == OpCodes.Call || inst.OpCode == OpCodes.Callvirt)
                    {
                        var mr = inst.Operand as MethodReference;
                        if (mr != null && mr.Name == "GetGameSpeedLimit")
                        {
                            Console.WriteLine("Type: {0}, Method: {1} calls GetGameSpeedLimit", t.FullName, m.FullName);
                        }
                    }
                }
            }
        }
    }
}

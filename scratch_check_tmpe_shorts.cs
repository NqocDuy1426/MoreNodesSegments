using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        int count = 0;
        foreach (var t in asm.MainModule.Types)
        {
            foreach (var m in t.Methods)
            {
                if (m.Body == null) continue;
                foreach (var inst in m.Body.Instructions)
                {
                    if (inst.OpCode == OpCodes.Conv_I2)
                    {
                        Console.WriteLine("Type: {0}, Method: {1} uses conv.i2 (short)", t.FullName, m.FullName);
                        count++;
                        if (count > 20) return;
                    }
                }
            }
        }
    }
}

using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string mpuPath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/2710657019/MorePathUnits.dll";
        var mod = ModuleDefinition.ReadModule(mpuPath);
        Console.WriteLine("MorePathUnits types:");
        foreach (var t in mod.GetTypes())
        {
            Console.WriteLine("Type: " + t.FullName);
            foreach (var m in t.Methods)
            {
                Console.WriteLine("  Method: " + m.Name);
                if (m.Body != null)
                {
                    foreach (var inst in m.Body.Instructions)
                    {
                        if (inst.OpCode == OpCodes.Ldstr || inst.OpCode == OpCodes.Call || inst.OpCode == OpCodes.Callvirt)
                        {
                            Console.WriteLine(string.Format("    {0} {1} {2}", inst.Offset.ToString("x4"), inst.OpCode, inst.Operand));
                        }
                    }
                }
            }
        }
    }
}

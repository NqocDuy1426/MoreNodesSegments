using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var t = asm.MainModule.GetType("TrafficManager.Manager.Impl.SpeedLimitManager");
        foreach (var m in t.Methods)
        {
            if (m.Name == ".ctor" || m.Name.Contains("Init") || m.Name.Contains("Awake"))
            {
                Console.WriteLine("--- " + m.FullName + " ---");
                foreach (var inst in m.Body.Instructions)
                {
                    if (inst.OpCode == OpCodes.Newarr || inst.OpCode == OpCodes.Ldfld)
                    {
                        Console.WriteLine("  {0:X4}: {1} {2}", inst.Offset, inst.OpCode, inst.Operand);
                    }
                }
            }
        }
    }
}

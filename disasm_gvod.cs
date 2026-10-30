using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var jrm = asm.MainModule.GetType("TrafficManager.Manager.Impl.JunctionRestrictionsManager");
        foreach (var nested in jrm.NestedTypes)
        {
            if (nested.Name == "JunctionRestrictions" || nested.Name == "SegmentJunctionRestrictions")
            {
                foreach (var m in nested.Methods)
                {
                    if (m.Name == "GetValueOrDefault" || m.Name == "GetTernaryBool")
                    {
                        Console.WriteLine("========================================");
                        Console.WriteLine("METHOD: " + nested.Name + "::" + m.Name);
                        Console.WriteLine("========================================");
                        if (m.Body != null)
                        {
                            foreach (var inst in m.Body.Instructions)
                                Console.WriteLine(inst);
                        }
                    }
                }
            }
        }
    }
}

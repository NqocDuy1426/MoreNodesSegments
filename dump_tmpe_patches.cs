using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class DumpTMPEPatches
{
    static void Main()
    {
        string tmpePath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(tmpePath);
        foreach (var type in asm.MainModule.Types)
        {
            if (type.FullName.Contains("Patch") || type.FullName.Contains("RoadBaseAI"))
            {
                foreach (var m in type.Methods)
                {
                    if (m.Name.Contains("Prefix") || m.Name.Contains("Postfix") || m.Name.Contains("TargetMethod"))
                    {
                        Console.WriteLine(type.FullName + " -> " + m.Name);
                    }
                }
            }
        }
    }
}

using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        foreach (var t in asm.MainModule.Types)
        {
            if (t.Name.Contains("SegmentEnd"))
            {
                Console.WriteLine("Type: " + t.FullName);
                foreach (var m in t.Methods)
                {
                    if (m.Name.Contains("GetIndex") || m.Name.Contains("SegmentAndNode"))
                        Console.WriteLine("  Method: " + m.FullName);
                }
            }
        }
    }
}

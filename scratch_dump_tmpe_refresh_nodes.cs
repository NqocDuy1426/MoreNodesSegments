using System;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string p = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + 
            "/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(p);

        string[] targets = new string[] {
            "RefreshCurrentNodeMarkers",
            "RefreshCurrentTimedNodeIds",
            "RefreshCurrentPriorityNodeIds",
            "RefreshCurrentRestrictedNodeIds",
            "FilterVisibleNodes",
            "RefreshCurrentRestrictedSegmentIds"
        };

        foreach (var t in asm.MainModule.Types)
        {
            foreach (var m in t.Methods)
            {
                foreach (var name in targets)
                {
                    if (m.Name == name)
                    {
                        Console.WriteLine($"\n=== {t.Name}.{m.Name} ===");
                        foreach (var inst in m.Body.Instructions)
                            Console.WriteLine($"  IL_{inst.Offset:X4}: {inst.OpCode} {inst.Operand}");
                    }
                }
            }
        }
    }
}

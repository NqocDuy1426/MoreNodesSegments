using System;
using System.IO;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

class AllMethodsScanner
{
    static readonly HashSet<int> Consts = new HashSet<int>
    {
        32768, 36864, 262144, 73728, 512, 576, 1016, 1024
    };

    static void Main()
    {
        var paths = new string[]
        {
            "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/3683231517/AdaptiveRoads.dll",
            "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/2472062376/NodeControllerRenewal.dll",
            "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/2140418403/IntersectionMarkingTool.dll",
            "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/2560782729/NetworkMultitool.dll",
            "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1619685021/MoveIt.dll",
            "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/2172488844/Picker.dll",
            "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/2862881785/NetworkAnarchy.dll",
            "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/3702700108/NetworkSkins.dll"
        };

        var resolver = new DefaultAssemblyResolver();
        resolver.AddSearchDirectory("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed");
        resolver.AddSearchDirectory("/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Addons/Mods/1680642819");
        var readerParams = new ReaderParameters { AssemblyResolver = resolver };

        foreach (var path in paths)
        {
            if (!File.Exists(path)) continue;
            var asm = AssemblyDefinition.ReadAssembly(path, readerParams);
            Console.WriteLine($"\n================== {asm.Name.Name} ==================");
            foreach (var mod in asm.Modules)
            {
                foreach (var t in mod.Types)
                {
                    ScanType(t);
                }
            }
        }
    }

    static void ScanType(TypeDefinition t)
    {
        foreach (var m in t.Methods)
        {
            if (!m.HasBody) continue;
            for (int i = 0; i < m.Body.Instructions.Count; i++)
            {
                var instr = m.Body.Instructions[i];
                if (instr.OpCode == OpCodes.Ldc_I4 && instr.Operand is int val && Consts.Contains(val))
                {
                    string context = "";
                    for (int j = Math.Max(0, i - 1); j <= Math.Min(m.Body.Instructions.Count - 1, i + 2); j++)
                    {
                        var it = m.Body.Instructions[j];
                        context += $" [{it.OpCode} {it.Operand}]";
                    }
                    Console.WriteLine($"  {t.FullName}::{m.Name} -> const {val}: {context}");
                }
            }
        }
        foreach (var nested in t.NestedTypes)
        {
            ScanType(nested);
        }
    }
}

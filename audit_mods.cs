using System;
using System.IO;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

class ModAuditor
{
    static readonly HashSet<int> TargetConstants = new HashSet<int>
    {
        32768, 32767, 36864, 36863, 262144, 262143, 73728, 49152, 16384
    };

    static void Main()
    {
        var modPaths = new Dictionary<string, string>
        {
            { "NetworkAnarchy", "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/2862881785/NetworkAnarchy.dll" },
            { "NetworkMultitool", "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/2560782729/NetworkMultitool.dll" },
            { "FindIt", "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/2133885971/FindIt.dll" },
            { "MoveIt", "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1619685021/MoveIt.dll" },
            { "NetworkSkins", "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/3702700108/NetworkSkins.dll" },
            { "AdaptiveRoads", "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/3683231517/AdaptiveRoads.dll" },
            { "NetworkExtensions2", "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/812125426/NetworkExtensions2.dll" },
            { "NodeControllerRenewal", "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/2472062376/NodeControllerRenewal.dll" },
            { "IntersectionMarkingTool", "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/2140418403/IntersectionMarkingTool.dll" },
            { "AdvancedRoadTools", "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1925527132/AdvancedRoadTools.dll" },
            { "Picker", "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/2172488844/Picker.dll" },
            { "PropAnarchy", "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/2862882210/PropAnarchy.dll" },
            { "PropLineTool", "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/694512541/PropLineTool_v1.dll" },
            { "LineTool", "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/2956939040/LineTool.dll" },
            { "TerraformTool", "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/411095553/TerraformTool.dll" }
        };

        var resolver = new DefaultAssemblyResolver();
        resolver.AddSearchDirectory("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed");
        resolver.AddSearchDirectory("/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Addons/Mods/1680642819");
        var readerParams = new ReaderParameters { AssemblyResolver = resolver };

        foreach (var kvp in modPaths)
        {
            if (!File.Exists(kvp.Value))
            {
                Console.WriteLine($"[-] {kvp.Key}: File not found: {kvp.Value}");
                continue;
            }

            try
            {
                var assembly = AssemblyDefinition.ReadAssembly(kvp.Value, readerParams);
                Console.WriteLine($"\n================== {kvp.Key} ({assembly.Name.Name} v{assembly.Name.Version}) ==================");
                int matchCount = 0;
                var arrayAllocs = new List<string>();

                foreach (var module in assembly.Modules)
                {
                    foreach (var type in module.Types)
                    {
                        CheckType(type, ref matchCount, arrayAllocs);
                    }
                }

                if (arrayAllocs.Count > 0)
                {
                    Console.WriteLine($"  [Array Allocations with constants ({arrayAllocs.Count})]:");
                    foreach (var a in arrayAllocs)
                    {
                        Console.WriteLine($"    {a}");
                    }
                }
                Console.WriteLine($"  Total network constant references: {matchCount}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Error reading {kvp.Key}: {ex.Message}");
            }
        }
    }

    static void CheckType(TypeDefinition type, ref int matchCount, List<string> arrayAllocs)
    {
        foreach (var method in type.Methods)
        {
            if (!method.HasBody) continue;

            for (int i = 0; i < method.Body.Instructions.Count; i++)
            {
                var instr = method.Body.Instructions[i];
                int val = 0;
                bool isConst = false;

                if (instr.OpCode == OpCodes.Ldc_I4)
                {
                    val = (int)instr.Operand;
                    isConst = true;
                }
                else if (instr.OpCode == OpCodes.Ldc_I4_S)
                {
                    val = (sbyte)instr.Operand;
                    isConst = true;
                }

                if (isConst && TargetConstants.Contains(val))
                {
                    matchCount++;
                    if (i + 1 < method.Body.Instructions.Count && method.Body.Instructions[i + 1].OpCode == OpCodes.Newarr)
                    {
                        var arrType = method.Body.Instructions[i + 1].Operand;
                        arrayAllocs.Add($"{type.FullName}.{method.Name}: newarr {arrType}[{val}]");
                    }
                    else
                    {
                        if (matchCount <= 8)
                        {
                            Console.WriteLine($"  Ref in {type.Name}.{method.Name}: const {val} (Op: {instr.OpCode})");
                        }
                    }
                }
            }
        }

        foreach (var nested in type.NestedTypes)
        {
            CheckType(nested, ref matchCount, arrayAllocs);
        }
    }
}

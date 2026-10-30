using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Analyzer
{
    static void Main()
    {
        string[] dlls = new string[]
        {
            "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/3358543190/ParallelRoadTool.dll",
            "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/3690061052/ImprovedPublicTransport3.dll",
            "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/3714961481/IPTEssentials.dll",
            "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1548831935/AdvancedVehicleOptions.dll",
            "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/3686395676/TrainCarSelector.dll"
        };

        var resolver = new DefaultAssemblyResolver();
        resolver.AddSearchDirectory("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed");
        resolver.AddSearchDirectory("/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Addons/Mods/1680642819");
        var readerParams = new ReaderParameters { AssemblyResolver = resolver };

        foreach (var path in dlls)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("File not found: " + path);
                continue;
            }

            Console.WriteLine("================================================================================");
            Console.WriteLine("ANALYZING: " + Path.GetFileName(path));
            Console.WriteLine("Path: " + path);
            Console.WriteLine("================================================================================");

            var asm = AssemblyDefinition.ReadAssembly(path, readerParams);
            
            // Check all types
            foreach (var type in asm.MainModule.Types)
            {
                InspectType(type);
            }
        }
    }

    static void InspectType(TypeDefinition type)
    {
        // 1. Check fields
        foreach (var field in type.Fields)
        {
            string ftype = field.FieldType.FullName;
            if (ftype.Contains("[]"))
            {
                // Array field
                // Console.WriteLine("  [Array Field] " + type.FullName + "::" + field.Name + " : " + ftype);
            }
        }

        // 2. Check methods
        foreach (var method in type.Methods)
        {
            InspectMethod(type, method);
        }

        // Nested types
        foreach (var nested in type.NestedTypes)
        {
            InspectType(nested);
        }
    }

    static void InspectMethod(TypeDefinition type, MethodDefinition method)
    {
        if (!method.HasBody) return;

        bool hasInteresting = false;
        var interestingLines = new List<string>();

        foreach (var instr in method.Body.Instructions)
        {
            // Check constants
            if (instr.OpCode == OpCodes.Ldc_I4)
            {
                int val = (int)instr.Operand;
                if (val == 32768 || val == 36864 || val == 73728 || val == 262144 || val == 49152 || val == 16384 || val == 65536)
                {
                    interestingLines.Add(string.Format("    LDC.I4 {0} at {1}", val, instr.Offset));
                    hasInteresting = true;
                }
            }
            else if (instr.OpCode == OpCodes.Ldc_I4_S)
            {
                sbyte val = (sbyte)instr.Operand;
                // not a network limit
            }

            // Check newarr
            if (instr.OpCode == OpCodes.Newarr)
            {
                var typeRef = instr.Operand as TypeReference;
                string tname = typeRef != null ? typeRef.Name : "";
                if (tname.Contains("Node") || tname.Contains("Segment") || tname.Contains("Lane") || tname.Contains("Int64") || tname.Contains("UInt64"))
                {
                    interestingLines.Add(string.Format("    NEWARR {0} at {1}", tname, instr.Offset));
                    hasInteresting = true;
                }
            }

            // Check calls or field accesses to NetManager or ToolController or NetTool
            string opStr = instr.Operand != null ? instr.Operand.ToString() : "";
            if (opStr.Contains("m_colliding") || opStr.Contains("m_updatedNodes") || opStr.Contains("m_updatedSegments"))
            {
                interestingLines.Add(string.Format("    ACCESS {0} at {1}", opStr, instr.Offset));
                hasInteresting = true;
            }
        }

        if (hasInteresting)
        {
            Console.WriteLine("-> Method: " + type.FullName + "::" + method.Name);
            foreach (var l in interestingLines)
            {
                Console.WriteLine(l);
            }
        }
    }
}

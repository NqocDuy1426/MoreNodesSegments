
using System;
using System.IO;
using Mono.Cecil;

class D {
    static void Main() {
        var paths = new string[]
        {
            "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/2862881785/NetworkAnarchy.dll",
            "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/2560782729/NetworkMultitool.dll",
            "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/2133885971/FindIt.dll",
            "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1619685021/MoveIt.dll",
            "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/3702700108/NetworkSkins.dll",
            "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/3683231517/AdaptiveRoads.dll",
            "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/812125426/NetworkExtensions2.dll",
            "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/2472062376/NodeControllerRenewal.dll",
            "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/2140418403/IntersectionMarkingTool.dll",
            "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1925527132/AdvancedRoadTools.dll",
            "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/2172488844/Picker.dll",
            "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/2862882210/PropAnarchy.dll",
            "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/694512541/PropLineTool_v1.dll",
            "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/2956939040/LineTool.dll",
            "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/411095553/TerraformTool.dll"
        };
        var r = new DefaultAssemblyResolver();
        r.AddSearchDirectory("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed");
        r.AddSearchDirectory("/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Addons/Mods/1680642819");
        var readerParams = new ReaderParameters { AssemblyResolver = r };

        foreach (var p in paths) {
            if (!File.Exists(p)) continue;
            var asm = AssemblyDefinition.ReadAssembly(p, readerParams);
            foreach (var mod in asm.Modules) {
                foreach (var t in mod.Types) {
                    CheckFields(t, asm.Name.Name);
                }
            }
        }
    }

    static void CheckFields(TypeDefinition t, string asmName) {
        foreach (var f in t.Fields) {
            string ft = f.FieldType.FullName;
            if (ft.Contains("NetNode[]") || ft.Contains("NetSegment[]") || ft.Contains("NetLane[]") || ft.Contains("Array16`1<NetNode>") || ft.Contains("Array16`1<NetSegment>") || ft.Contains("Array32`1<NetLane>")) {
                Console.WriteLine($"[{asmName}] {t.FullName}::{f.Name} : {ft} (Static: {f.IsStatic})");
            }
        }
        foreach (var n in t.NestedTypes) CheckFields(n, asmName);
    }
}

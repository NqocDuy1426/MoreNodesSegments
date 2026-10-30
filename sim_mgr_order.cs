
using System;
using Mono.Cecil;
class P {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        string[] mgrs = {
            "ToolManager", "GameAreaManager", "TerrainManager", "InstanceManager",
            "PathManager", "NetManager", "BuildingManager", "CitizenManager",
            "TransferManager", "NaturalResourceManager", "ImmaterialResourceManager",
            "CoverageManager", "ElectricityManager", "WaterManager", "ZoneManager",
            "PropManager", "TreeManager", "WeatherManager", "VehicleManager",
            "TransportManager", "EconomyManager", "NotificationManager", "DistrictManager",
            "EventManager", "DisasterManager", "InfoManager", "RenderManager",
            "AudioManager", "StatisticsManager", "UnlockManager", "MessageManager",
            "GuideManager", "EffectManager"
        };
        int idx = 0;
        foreach (var m in mgrs) {
            var t = asm.MainModule.GetType(m);
            bool isSim = false;
            foreach (var ifc in t.Interfaces) {
                if (ifc.InterfaceType.Name == "ISimulationManager") isSim = true;
            }
            if (isSim) {
                Console.WriteLine((idx++) + ": " + m);
            }
        }
    }
}

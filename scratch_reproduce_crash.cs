using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using ColossalFramework;
using ColossalFramework.IO;

class Program
{
    static void SetSingleton(Type t, object instance)
    {
        var genericType = typeof(Singleton<>).MakeGenericType(t);
        var f = genericType.GetField("sInstance", BindingFlags.NonPublic | BindingFlags.Static);
        if (f != null) f.SetValue(null, instance);
    }

    static object CreateInstance(Type t)
    {
        return System.Runtime.Serialization.FormatterServices.GetUninitializedObject(t);
    }

    static void Main()
    {
        Console.WriteLine("Initializing dummy managers...");
        
        var lm = (LoadingManager)CreateInstance(typeof(LoadingManager));
        lm.m_loadingProfilerSimulation = new LoadingProfiler();
        SetSingleton(typeof(LoadingManager), lm);

        var sm = (SimulationManager)CreateInstance(typeof(SimulationManager));
        sm.m_metaData = new SimulationMetaData();
        typeof(SimulationManager).GetField("m_buildIndexHistory", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(sm, new uint[32]);
        sm.m_serializableDataStorage = new Dictionary<string, byte[]>();
        typeof(SimulationManager).GetField("m_pauseOnLoad", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).SetValue(sm, new SavedBool("pauseOnLoad", "gameSettings", false));
        SetSingleton(typeof(SimulationManager), sm);

        var gs = CreateInstance(typeof(GameSettings));
        var dict = new Dictionary<string, SettingsFile>();
        dict["gameSettings"] = new SettingsFile();
        typeof(GameSettings).GetField("m_SettingsFiles", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).SetValue(gs, dict);
        SetSingleton(typeof(GameSettings), gs);

        var em = (ElectricityManager)CreateInstance(typeof(ElectricityManager));
        em.m_nodeGroups = new ushort[32768];
        SetSingleton(typeof(ElectricityManager), em);

        var wm = (WaterManager)CreateInstance(typeof(WaterManager));
        SetSingleton(typeof(WaterManager), wm);

        var nm = (NetManager)CreateInstance(typeof(NetManager));
        nm.m_nodes = new Array16<NetNode>(32768);
        nm.m_segments = new Array16<NetSegment>(36864);
        nm.m_lanes = new Array32<NetLane>(262144);
        nm.m_nodeGrid = new ushort[270 * 270];
        nm.m_segmentGrid = new ushort[270 * 270];
        nm.m_updatedNodes = new ulong[1024];
        nm.m_updatedSegments = new ulong[1024];
        nm.m_adjustedSegments = new ulong[576];
        var sField = typeof(NetManager).GetField("m_serviceSegments", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (sField != null) {
            var arr = Array.CreateInstance(sField.FieldType.GetElementType(), 8);
            for (int i = 0; i < 8; i++) {
                arr.SetValue(Activator.CreateInstance(sField.FieldType.GetElementType()), i);
            }
            sField.SetValue(nm, arr);
        }
        SetSingleton(typeof(NetManager), nm);

        string[] managers = {
            "AudioManager", "BuildingManager", "CitizenManager", "DisasterManager",
            "DistrictManager", "EventManager", "EconomyManager", 
            "ImmaterialResourceManager", "InfoManager", "InstanceManager",
            "NaturalResourceManager", "TransferManager", "WeatherManager",
            "GuideManager", "MessageManager", "PathManager", "PropManager",
            "ErrorManager", "StatisticsManager", "TerrainManager", "WaterSimulation",
            "TransportManager", "TreeManager", "UnlockManager", "VehicleManager", "ZoneManager"
        };
        var gam = (GameAreaManager)CreateInstance(typeof(GameAreaManager));
        gam.m_areaGrid = new int[25];
        SetSingleton(typeof(GameAreaManager), gam);
        var asm = typeof(NetManager).Assembly;
        foreach (var name in managers)
        {
            var t = asm.GetType(name);
            if (t != null) SetSingleton(t, CreateInstance(t));
        }

        Console.WriteLine("Reading backup4_sim.bin...");
        using (var fs = File.OpenRead("backup4_sim.bin"))
        {
            var meta = DataSerializer.Deserialize<SimulationMetaData>(fs, DataSerializer.Mode.Memory);
            Console.WriteLine("Meta read successfully. Environment: " + meta.m_environment + ", UpdateMode: " + meta.m_updateMode);
            sm.m_metaData = meta;

            try
            {
                Console.WriteLine("Starting SimulationManager.Data deserialization...");
                var data = DataSerializer.Deserialize<SimulationManager.Data>(fs, DataSerializer.Mode.Memory);
                Console.WriteLine("SimulationManager.Data deserialized SUCCESS!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("CAUGHT EXCEPTION: " + ex.GetType().FullName + ": " + ex.Message);
                Console.WriteLine(ex.ToString());
            }
        }
    }
}

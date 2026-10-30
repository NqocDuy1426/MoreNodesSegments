using System;
using System.Reflection;
using System.Runtime.Serialization;
using ColossalFramework;

class Program
{
    static void Main()
    {
        var lm = (LoadingManager)FormatterServices.GetUninitializedObject(typeof(LoadingManager));
        typeof(Singleton<LoadingManager>).GetField("sInstance", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, lm);
        lm.m_loadingProfilerSimulation = new LoadingProfiler();

        var nm = (NetManager)FormatterServices.GetUninitializedObject(typeof(NetManager));
        typeof(Singleton<NetManager>).GetField("sInstance", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, nm);

        Console.WriteLine("Singletons initialized successfully!");
        Console.WriteLine("Singleton<NetManager>.instance is null? " + (Singleton<NetManager>.instance == null));
    }
}

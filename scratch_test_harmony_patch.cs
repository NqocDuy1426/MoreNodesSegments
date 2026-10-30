using System;
using System.Reflection;
using Harmony;

public static class RoadBaseAI_TrafficLight_Patches
{
    [HarmonyPatch(typeof(RoadBaseAI), "SetTrafficLightState", new Type[] {
        typeof(ushort),
        typeof(NetSegment),
        typeof(uint),
        typeof(RoadBaseAI.TrafficLightState),
        typeof(RoadBaseAI.TrafficLightState),
        typeof(bool),
        typeof(bool)
    }, new ArgumentType[] {
        ArgumentType.Normal,
        ArgumentType.Ref,
        ArgumentType.Normal,
        ArgumentType.Normal,
        ArgumentType.Normal,
        ArgumentType.Normal,
        ArgumentType.Normal
    })]
    public static class SetTrafficLightStatePatch
    {
        public static bool Prefix(ushort nodeID, ref NetSegment segmentData, uint frame, RoadBaseAI.TrafficLightState vehicleLightState, RoadBaseAI.TrafficLightState pedestrianLightState, bool vehicles, bool pedestrians)
        {
            return true;
        }
    }

    [HarmonyPatch(typeof(RoadBaseAI), "GetTrafficLightState", new Type[] {
        typeof(ushort),
        typeof(NetSegment),
        typeof(uint),
        typeof(RoadBaseAI.TrafficLightState),
        typeof(RoadBaseAI.TrafficLightState)
    }, new ArgumentType[] {
        ArgumentType.Normal,
        ArgumentType.Ref,
        ArgumentType.Normal,
        ArgumentType.Out,
        ArgumentType.Out
    })]
    public static class GetTrafficLightState5PPatch
    {
        public static bool Prefix(ushort nodeID, ref NetSegment segmentData, uint frame, out RoadBaseAI.TrafficLightState vehicleLightState, out RoadBaseAI.TrafficLightState pedestrianLightState)
        {
            vehicleLightState = RoadBaseAI.TrafficLightState.Green;
            pedestrianLightState = RoadBaseAI.TrafficLightState.Green;
            return true;
        }
    }

    [HarmonyPatch(typeof(RoadBaseAI), "GetTrafficLightState", new Type[] {
        typeof(ushort),
        typeof(NetSegment),
        typeof(uint),
        typeof(RoadBaseAI.TrafficLightState),
        typeof(RoadBaseAI.TrafficLightState),
        typeof(bool),
        typeof(bool)
    }, new ArgumentType[] {
        ArgumentType.Normal,
        ArgumentType.Ref,
        ArgumentType.Normal,
        ArgumentType.Out,
        ArgumentType.Out,
        ArgumentType.Out,
        ArgumentType.Out
    })]
    public static class GetTrafficLightState7PPatch
    {
        public static bool Prefix(ushort nodeID, ref NetSegment segmentData, uint frame, out RoadBaseAI.TrafficLightState vehicleLightState, out RoadBaseAI.TrafficLightState pedestrianLightState, out bool vehicles, out bool pedestrians)
        {
            vehicleLightState = RoadBaseAI.TrafficLightState.Green;
            pedestrianLightState = RoadBaseAI.TrafficLightState.Green;
            vehicles = false;
            pedestrians = false;
            return true;
        }
    }
}
class Program {
    static void Main() {
        var h = HarmonyInstance.Create("test.patch");
        h.PatchAll(Assembly.GetExecutingAssembly());
        Console.WriteLine("Harmony PatchAll OK!");
    }
}

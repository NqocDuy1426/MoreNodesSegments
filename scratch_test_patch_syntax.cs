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
            int num = (int)(((int)pedestrianLightState << 2) | (int)vehicleLightState);
            if (segmentData.m_startNode == nodeID)
            {
                segmentData.m_trafficLightState0 = (segmentData.m_trafficLightState0 & -16) | num;
                segmentData.m_trafficLightState1 = (segmentData.m_trafficLightState1 & -16) | num;
                if (vehicles)
                    segmentData.m_flags |= NetSegment.Flags.TrafficStart;
                else
                    segmentData.m_flags &= ~NetSegment.Flags.TrafficStart;

                if (pedestrians)
                    segmentData.m_flags |= NetSegment.Flags.CrossingStart;
                else
                    segmentData.m_flags &= ~NetSegment.Flags.CrossingStart;
            }
            else
            {
                segmentData.m_trafficLightState0 = (segmentData.m_trafficLightState0 & -241) | (num << 4);
                segmentData.m_trafficLightState1 = (segmentData.m_trafficLightState1 & -241) | (num << 4);
                if (vehicles)
                    segmentData.m_flags |= NetSegment.Flags.TrafficEnd;
                else
                    segmentData.m_flags &= ~NetSegment.Flags.TrafficEnd;

                if (pedestrians)
                    segmentData.m_flags |= NetSegment.Flags.CrossingEnd;
                else
                    segmentData.m_flags &= ~NetSegment.Flags.CrossingEnd;
            }
            return false;
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
            int num = ((((frame >> 8) & 1) != 0) ? segmentData.m_trafficLightState1 : segmentData.m_trafficLightState0);
            if (num == 0 && (segmentData.m_trafficLightState0 != 0 || segmentData.m_trafficLightState1 != 0))
            {
                num = (segmentData.m_trafficLightState0 != 0) ? segmentData.m_trafficLightState0 : segmentData.m_trafficLightState1;
            }

            if (segmentData.m_startNode == nodeID)
            {
                num &= 15;
            }
            else
            {
                num = (num >> 4) & 15;
            }
            vehicleLightState = (RoadBaseAI.TrafficLightState)(num & 3);
            pedestrianLightState = (RoadBaseAI.TrafficLightState)(num >> 2);
            return false;
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
            int num;
            if (segmentData.m_startNode == nodeID)
            {
                vehicles = (segmentData.m_flags & NetSegment.Flags.TrafficStart) != NetSegment.Flags.None;
                pedestrians = (segmentData.m_flags & NetSegment.Flags.CrossingStart) != NetSegment.Flags.None;
                num = ((((frame >> 8) & 1) != 0) ? segmentData.m_trafficLightState1 : segmentData.m_trafficLightState0);
                if (num == 0 && (segmentData.m_trafficLightState0 != 0 || segmentData.m_trafficLightState1 != 0))
                {
                    num = (segmentData.m_trafficLightState0 != 0) ? segmentData.m_trafficLightState0 : segmentData.m_trafficLightState1;
                }
                num &= 15;
            }
            else
            {
                vehicles = (segmentData.m_flags & NetSegment.Flags.TrafficEnd) != NetSegment.Flags.None;
                pedestrians = (segmentData.m_flags & NetSegment.Flags.CrossingEnd) != NetSegment.Flags.None;
                num = ((((frame >> 8) & 1) != 0) ? segmentData.m_trafficLightState1 : segmentData.m_trafficLightState0);
                if (num == 0 && (segmentData.m_trafficLightState0 != 0 || segmentData.m_trafficLightState1 != 0))
                {
                    num = (segmentData.m_trafficLightState0 != 0) ? segmentData.m_trafficLightState0 : segmentData.m_trafficLightState1;
                }
                num = (num >> 4) & 15;
            }
            vehicleLightState = (RoadBaseAI.TrafficLightState)(num & 3);
            pedestrianLightState = (RoadBaseAI.TrafficLightState)(num >> 2);
            return false;
        }
    }
}
class Program { static void Main() { Console.WriteLine("Compiles OK!"); } }

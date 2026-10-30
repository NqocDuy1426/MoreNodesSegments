using System;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        string tmpeDll = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        string gameDir = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        
        Assembly.LoadFrom(Path.Combine(gameDir, "Assembly-CSharp.dll"));
        Assembly.LoadFrom(Path.Combine(gameDir, "ColossalManaged.dll"));
        Assembly.LoadFrom(Path.Combine(gameDir, "ICities.dll"));
        Assembly.LoadFrom(Path.Combine(gameDir, "UnityEngine.dll"));
        Assembly tmpe = Assembly.LoadFrom(tmpeDll);

        Console.WriteLine("Testing TM:PE manager APIs:");

        // 1. JunctionRestrictionsManager
        Type jrmType = tmpe.GetType("TrafficManager.Manager.Impl.JunctionRestrictionsManager");
        Console.WriteLine("JunctionRestrictionsManager: " + (jrmType != null));
        Console.WriteLine("  GetUturnAllowed: " + (jrmType.GetMethod("GetUturnAllowed", new Type[] { typeof(ushort), typeof(bool) }) != null));
        Console.WriteLine("  SetUturnAllowed: " + (jrmType.GetMethod("SetUturnAllowed", new Type[] { typeof(ushort), typeof(bool), typeof(bool) }) != null));

        // 2. TrafficPriorityManager
        Type tpmType = tmpe.GetType("TrafficManager.Manager.Impl.TrafficPriorityManager");
        Console.WriteLine("TrafficPriorityManager: " + (tpmType != null));
        Console.WriteLine("  GetPrioritySign: " + (tpmType.GetMethod("GetPrioritySign", new Type[] { typeof(ushort), typeof(bool) }) != null));

        // 3. SpeedLimitManager
        Type slmType = tmpe.GetType("TrafficManager.Manager.Impl.SpeedLimitManager");
        Console.WriteLine("SpeedLimitManager: " + (slmType != null));
        Console.WriteLine("  GetAllLaneSpeedLimits: " + (slmType.GetMethod("GetAllLaneSpeedLimits", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) != null));
        Console.WriteLine("  SetLaneSpeedLimit: " + (slmType.GetMethod("SetLaneSpeedLimit", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) != null));

        // 4. Flags (LaneArrows, VehicleRestrictions)
        Type flagsType = tmpe.GetType("TrafficManager.State.Flags");
        Console.WriteLine("Flags: " + (flagsType != null));
        Console.WriteLine("  GetLaneArrowFlags: " + (flagsType.GetMethod("GetLaneArrowFlags", new Type[] { typeof(uint) }) != null));
        Console.WriteLine("  SetLaneArrowFlags: " + (flagsType.GetMethod("SetLaneArrowFlags", BindingFlags.Public | BindingFlags.Static) != null));
        Console.WriteLine("  GetAllLaneAllowedVehicleTypes: " + (flagsType.GetMethod("GetAllLaneAllowedVehicleTypes", BindingFlags.Public | BindingFlags.Static) != null));

        // 5. ParkingRestrictionsManager
        Type prmType = tmpe.GetType("TrafficManager.Manager.Impl.ParkingRestrictionsManager");
        Console.WriteLine("ParkingRestrictionsManager: " + (prmType != null));
        Console.WriteLine("  SetParkingAllowed: " + (prmType.GetMethod("SetParkingAllowed", BindingFlags.Public | BindingFlags.Instance) != null));

        // 6. LaneConnectionManager
        Type lcmType = tmpe.GetType("TrafficManager.Manager.Impl.LaneConnection.LaneConnectionManager");
        Console.WriteLine("LaneConnectionManager: " + (lcmType != null));

        // 7. TrafficLightSimulationManager
        Type tlsmType = tmpe.GetType("TrafficManager.Manager.Impl.TrafficLightSimulationManager");
        Console.WriteLine("TrafficLightSimulationManager: " + (tlsmType != null));
        Console.WriteLine("  SetUpTimedTrafficLight: " + (tlsmType.GetMethod("SetUpTimedTrafficLight", new Type[] { typeof(ushort), typeof(IList<ushort>) }) != null));
    }
}

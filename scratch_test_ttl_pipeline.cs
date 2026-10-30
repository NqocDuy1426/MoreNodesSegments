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

        Type tlsmType = tmpe.GetType("TrafficManager.Manager.Impl.TrafficLightSimulationManager");
        Type ttlType = tmpe.GetType("TrafficManager.TrafficLight.Impl.TimedTrafficLights");
        Type ttlsType = tmpe.GetType("TrafficManager.TrafficLight.Impl.TimedTrafficLightsStep");
        Type cslType = tmpe.GetType("TrafficManager.TrafficLight.Impl.CustomSegmentLights");
        Type lightType = tmpe.GetType("TrafficManager.TrafficLight.Impl.CustomSegmentLight");

        Console.WriteLine("Types found: " + (tlsmType != null && ttlType != null && ttlsType != null && cslType != null && lightType != null));

        // Inspect AddStep
        foreach (var m in ttlType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            if (m.Name == "AddStep")
            {
                Console.WriteLine("AddStep: (" + string.Join(", ", Array.ConvertAll(m.GetParameters(), p => p.ParameterType.Name + " " + p.Name)) + ")");
            }
        }

        // Inspect CustomSegmentLights methods
        foreach (var m in ttlsType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            if (m.Name.Contains("SegmentLights"))
            {
                Console.WriteLine("Step method: " + m.Name + " (" + string.Join(", ", Array.ConvertAll(m.GetParameters(), p => p.ParameterType.Name + " " + p.Name)) + ")");
            }
        }

        // Inspect CustomSegmentLight SetStates
        foreach (var m in lightType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            if (m.Name.Contains("SetStates") || m.Name.Contains("Mode"))
            {
                Console.WriteLine("Light method: " + m.Name + " (" + string.Join(", ", Array.ConvertAll(m.GetParameters(), p => p.ParameterType.Name + " " + p.Name)) + ")");
            }
        }
    }
}

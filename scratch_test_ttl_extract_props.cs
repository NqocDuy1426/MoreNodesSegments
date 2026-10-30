using System;
using System.IO;
using System.Reflection;

class Program
{
    static void Main()
    {
        string tmpeDll = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        string gameDir = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        
        Assembly.LoadFrom(Path.Combine(gameDir, "Assembly-CSharp.dll"));
        Assembly tmpe = Assembly.LoadFrom(tmpeDll);

        Type[] types = new Type[]
        {
            tmpe.GetType("TrafficManager.Configuration+TimedTrafficLights"),
            tmpe.GetType("TrafficManager.Configuration+TimedTrafficLightsStep"),
            tmpe.GetType("TrafficManager.Configuration+CustomSegmentLights"),
            tmpe.GetType("TrafficManager.Configuration+CustomSegmentLight")
        };

        foreach (var t in types)
        {
            Console.WriteLine("\nType: " + t.Name);
            foreach (var f in t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                Console.WriteLine("  F: " + f.FieldType.FullName + " " + f.Name);
            }
        }
    }
}

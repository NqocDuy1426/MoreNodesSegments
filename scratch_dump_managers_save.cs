using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string tmpePath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        Assembly asm = Assembly.LoadFrom(tmpePath);

        string[] managerNames = new string[] {
            "TrafficManager.Manager.Impl.TrafficLightSimulationManager",
            "TrafficManager.Manager.Impl.LaneConnection.LaneConnectionManager",
            "TrafficManager.Manager.Impl.JunctionRestrictionsManager",
            "TrafficManager.Manager.Impl.TrafficPriorityManager"
        };

        foreach (var name in managerNames)
        {
            Type t = asm.GetType(name);
            if (t == null) continue;
            MethodInfo m = t.GetMethod("SaveData", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (m != null)
            {
                Console.WriteLine($"{name}.SaveData return type: {m.ReturnType.Name}, params: {m.GetParameters().Length}");
                var body = m.GetMethodBody();
                if (body != null)
                {
                    Console.WriteLine($"  IL length: {body.GetILAsByteArray().Length}");
                }
            }
        }
    }
}

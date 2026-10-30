using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        Assembly asm = Assembly.LoadFile("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        Type tSim = asm.GetType("TrafficManager.TrafficLight.Impl.TrafficLightSimulation");
        foreach (FieldInfo fi in tSim.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            Console.WriteLine("Sim Field: " + fi.FieldType + " " + fi.Name);
        }
        foreach (MethodInfo mi in tSim.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            if (mi.Name.Contains("SimulationRunning"))
            {
                Console.WriteLine("Method: " + mi.Name);
                var body = mi.GetMethodBody();
                if (body != null)
                {
                    Console.WriteLine("IL bytes: " + body.GetILAsByteArray().Length);
                }
            }
        }
    }
}

using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        Assembly apiAsm = Assembly.LoadFile("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TMPE.API.dll");
        Assembly asm = Assembly.LoadFile("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        Type tSim = asm.GetType("TrafficManager.TrafficLight.Impl.TrafficLightSimulation");
        Console.WriteLine("TrafficLightSimulation isValueType: " + tSim.IsValueType);
        
        Type tMgr = asm.GetType("TrafficManager.Manager.Impl.TrafficLightSimulationManager");
        PropertyInfo prop = tMgr.GetProperty("TrafficLightSimulations");
        Console.WriteLine("TrafficLightSimulations prop: " + prop.PropertyType);
        
        foreach (FieldInfo fi in tMgr.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            Console.WriteLine("Field: " + fi.FieldType + " " + fi.Name);
        }
    }
}

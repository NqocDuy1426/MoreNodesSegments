using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        Assembly asm = Assembly.LoadFile("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        Type tSim = asm.GetType("TrafficManager.TrafficLight.Impl.TrafficLightSimulation");
        foreach (ConstructorInfo ctor in tSim.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            Console.WriteLine("Ctor: " + ctor);
            foreach (var p in ctor.GetParameters()) Console.WriteLine("  " + p.ParameterType + " " + p.Name);
        }
    }
}

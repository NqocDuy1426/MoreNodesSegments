using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string dir = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/";
        Assembly asm = Assembly.LoadFrom(dir + "TrafficManager.dll");
        Type t1 = asm.GetType("TrafficManager.Configuration+TimedTrafficLights");
        Type t2 = asm.GetType("TrafficManager.Configuration+SegmentNodeConf");
        Type t3 = asm.GetType("TrafficManager.Configuration+PrioritySegment");

        Console.WriteLine($"TimedTrafficLights.IsSerializable: {t1.IsSerializable}");
        Console.WriteLine($"SegmentNodeConf.IsSerializable: {t2.IsSerializable}");
        Console.WriteLine($"PrioritySegment.IsSerializable: {t3.IsSerializable}");
    }
}

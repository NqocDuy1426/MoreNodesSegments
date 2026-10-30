using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string tmpePath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        Assembly asm = Assembly.LoadFrom(tmpePath);
        Type cfgType = asm.GetType("TrafficManager.State.Configuration") ?? asm.GetType("TrafficManager.Configuration");
        Console.WriteLine($"Configuration type: {cfgType.FullName}");
        foreach (var f in cfgType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            Console.WriteLine($"Field: {f.Name} : {f.FieldType.Name}");
        }
    }
}

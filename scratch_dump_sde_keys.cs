using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string tmpePath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        Assembly asm = Assembly.LoadFrom(tmpePath);
        Type sde = asm.GetType("TrafficManager.Lifecycle.SerializableDataExtension");
        foreach (var f in sde.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
        {
            if (f.FieldType == typeof(string))
            {
                Console.WriteLine($"{f.Name} = {f.GetValue(null)}");
            }
        }
    }
}

using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string apiPath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TMPE.API.dll";
        Assembly asm = Assembly.LoadFrom(apiPath);
        Type t = asm.GetType("TrafficManager.API.Traffic.Data.ExtVehicle");
        Console.WriteLine("ExtVehicle IsValueType: " + t.IsValueType);
        foreach (FieldInfo f in t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            Console.WriteLine("Field: " + f.FieldType + " " + f.Name);
        }
        foreach (ConstructorInfo ctor in t.GetConstructors())
        {
            Console.WriteLine("Ctor: " + ctor);
        }
    }
}

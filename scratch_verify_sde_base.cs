using System;
using System.Reflection;
using ICities;

class Program
{
    static void Main()
    {
        Type t = typeof(SerializableDataExtensionBase);
        Console.WriteLine($"Base type: {t.FullName}");
        foreach (var m in t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            Console.WriteLine($"  {m.ReturnType.Name} {m.Name}");
        }
        foreach (var f in t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            Console.WriteLine($"  Field: {f.Name} : {f.FieldType.Name}");
        }
        foreach (var p in t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            Console.WriteLine($"  Prop: {p.Name} : {p.PropertyType.Name}");
        }
    }
}

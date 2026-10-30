using System;
using System.Reflection;

class Program {
    static void Main() {
        string managed = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        Assembly asm = Assembly.LoadFrom(managed + "/Assembly-CSharp.dll");
        
        Type sdType = asm.GetType("ShipDockAI");
        var m1 = sdType.GetMethod("FindFerryPath", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        Console.WriteLine($"FindFerryPath: {m1}");
        foreach (var p in m1.GetParameters()) Console.WriteLine($"   {p.ParameterType} {p.Name} (out: {p.IsOut})");

        var m2 = sdType.GetMethod("FindConnectionPath", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        Console.WriteLine($"\nFindConnectionPath: {m2}");
        foreach (var p in m2.GetParameters()) Console.WriteLine($"   {p.ParameterType} {p.Name} (out: {p.IsOut})");

        Type tsType = asm.GetType("TransportStationAI");
        var m3 = tsType.GetMethod("GetTransportLineInfo", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        Console.WriteLine($"\nGetTransportLineInfo: {m3}");

        var m4 = tsType.GetMethod("GetSecondaryTransportLineInfo", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        Console.WriteLine($"\nGetSecondaryTransportLineInfo: {m4}");
    }
}

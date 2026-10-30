using System;
using System.Reflection;
using ColossalFramework.Packaging;

class Program {
    static void Main() {
        foreach (var m in typeof(Package).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)) {
            Console.WriteLine("Package: " + m.Name + " (" + string.Join(",", Array.ConvertAll(m.GetParameters(), p => p.ParameterType.Name)) + ")");
        }
    }
}

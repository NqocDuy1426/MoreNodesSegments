using System;
using System.Reflection;

class Program {
    static void Main() {
        foreach (var m in typeof(ColossalFramework.Packaging.Package).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)) {
            if (m.Name.Contains("Deserialize") || m.Name.Contains("Header"))
                Console.WriteLine("Package: " + m.Name + " (" + string.Join(",", Array.ConvertAll(m.GetParameters(), p => p.ParameterType.Name)) + ")");
        }
    }
}

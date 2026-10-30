using System;
using System.Reflection;
using ColossalFramework.Packaging;

class Program {
    static void Main() {
        foreach (var f in typeof(Package.Asset).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
            Console.WriteLine("Asset field: " + f.Name + " (" + f.FieldType.Name + ")");
        }
        foreach (var m in typeof(Package.Asset).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)) {
            if (m.Name.Contains("Deserialize") || m.Name.Contains("Read"))
                Console.WriteLine("Asset method: " + m.Name);
        }
    }
}

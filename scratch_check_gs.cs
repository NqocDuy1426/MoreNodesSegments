using System;
using System.Reflection;
using ColossalFramework;

class Program {
    static void Main() {
        foreach (var f in typeof(GameSettings).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
            Console.WriteLine("GameSettings field: " + f.Name + " (" + f.FieldType.FullName + ")");
        }
    }
}

using System;
using System.Reflection;
using ColossalFramework;

class Program {
    static void Main() {
        var t = typeof(Singleton<>);
        var f = t.GetFields(BindingFlags.NonPublic | BindingFlags.Static);
        foreach (var x in f) {
            Console.WriteLine("Field: " + x.Name);
        }
    }
}

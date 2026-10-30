using System;
using System.Reflection;

class Program {
    static void Main() {
        var a = Assembly.LoadFrom("CitiesHarmony.Harmony.dll");
        Console.WriteLine("Types in CitiesHarmony.Harmony.dll:");
        foreach (var t in a.GetTypes()) {
            Console.WriteLine(t.FullName);
        }
    }
}

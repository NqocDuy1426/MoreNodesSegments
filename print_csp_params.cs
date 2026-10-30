using System;
using System.Reflection;

class P {
    static void Main() {
        var carAi = typeof(CarAI);
        foreach (var m in carAi.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)) {
            if (m.Name == "CalculateSegmentPosition") {
                Console.WriteLine(m.Name + " (" + m.GetParameters().Length + "): isPublic=" + m.IsPublic + ", isFamily=" + m.IsFamily + ", isFamilyOrAssembly=" + m.IsFamilyOrAssembly);
            }
        }
    }
}

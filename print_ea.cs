using System;
using System.Reflection;
using ColossalFramework.IO;

class Program {
    static void Main() {
        var t = typeof(EncodedArray.UShort);
        foreach (var m in t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
            Console.WriteLine("UShort method: " + m.Name);
        }
    }
}

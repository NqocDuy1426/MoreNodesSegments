using System;
using System.Reflection;
using ColossalFramework;

class Program {
    static void Main() {
        foreach (var c in typeof(SettingsFile).GetConstructors()) {
            Console.WriteLine("SettingsFile ctor: " + c);
        }
    }
}

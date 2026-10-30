using System;
using System.Reflection;
using ColossalFramework;

class Program {
    static void Main() {
        foreach (var c in typeof(SavedBool).GetConstructors()) {
            Console.WriteLine("SavedBool ctor: " + c);
        }
    }
}

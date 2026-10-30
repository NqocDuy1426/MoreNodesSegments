using System;
using System.Reflection;

class Program {
    static void Main() {
        var t = typeof(ColossalFramework.IO.EncodedArray.UShort);
        var m = t.GetMethod("BeginRead", BindingFlags.Public | BindingFlags.Static);
        Console.WriteLine("BeginRead: " + BitConverter.ToString(m.GetMethodBody().GetILAsByteArray()));
        
        var rm = t.GetMethod("Read", BindingFlags.Public | BindingFlags.Instance);
        Console.WriteLine("Read IL length: " + rm.GetMethodBody().GetILAsByteArray().Length);

        var erm = t.GetMethod("EndRead", BindingFlags.Public | BindingFlags.Instance);
        Console.WriteLine("EndRead IL: " + BitConverter.ToString(erm.GetMethodBody().GetILAsByteArray()));
    }
}

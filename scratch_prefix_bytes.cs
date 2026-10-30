using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        Assembly asm = Assembly.LoadFile("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        Type t = asm.GetType("TrafficManager.Patch._RoadBaseAI.GetTrafficLightNodeStatePatch");
        MethodInfo m = t.GetMethod("Prefix", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        byte[] il = m.GetMethodBody().GetILAsByteArray();
        var mRef = asm.ManifestModule.ResolveMethod(0x0A0007D3);
        Console.WriteLine("Method 0x0A0007D3: " + mRef.DeclaringType + "." + mRef.Name);
        foreach (var p in mRef.GetParameters()) Console.WriteLine("  " + p.ParameterType + " " + p.Name);
    }
}

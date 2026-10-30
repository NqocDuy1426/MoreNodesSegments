using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        Assembly asm = Assembly.LoadFile("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        Console.WriteLine("060006CA: " + asm.ManifestModule.ResolveMethod(0x060006CA).DeclaringType + "." + asm.ManifestModule.ResolveMethod(0x060006CA).Name);
        Console.WriteLine("060006CB: " + asm.ManifestModule.ResolveMethod(0x060006CB).DeclaringType + "." + asm.ManifestModule.ResolveMethod(0x060006CB).Name);
    }
}

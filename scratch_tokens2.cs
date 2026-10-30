using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        Assembly asm = Assembly.LoadFile("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        Module mod = asm.ManifestModule;
        Console.WriteLine("04000414: " + mod.ResolveField(0x04000414).DeclaringType + "." + mod.ResolveField(0x04000414).Name);
        Console.WriteLine("04000593: " + mod.ResolveField(0x04000593).DeclaringType + "." + mod.ResolveField(0x04000593).Name);
    }
}

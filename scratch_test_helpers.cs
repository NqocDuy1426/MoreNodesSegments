using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string dir = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/";
        Assembly.LoadFrom(dir + "CSUtil.Commons.dll");
        Assembly.LoadFrom(dir + "TMPE.API.dll");
        Assembly.LoadFrom(dir + "TrafficManager.dll");

        Assembly commons = null, api = null;
        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (asm.GetName().Name == "CSUtil.Commons") commons = asm;
            if (asm.GetName().Name == "TMPE.API") api = asm;
        }

        Console.WriteLine($"commons found: {commons != null}, api found: {api != null}");
    }
}

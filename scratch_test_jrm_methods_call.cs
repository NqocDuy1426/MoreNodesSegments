using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string dir = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/";
        Assembly asm = Assembly.LoadFrom(dir + "TrafficManager.dll");
        Type jrmType = asm.GetType("TrafficManager.Manager.Impl.JunctionRestrictionsManager");
        Type tbType = Assembly.LoadFrom(dir + "CSUtil.Commons.dll").GetType("CSUtil.Commons.TernaryBool");

        MethodInfo mSet = jrmType.GetMethod("SetUturnAllowed", new Type[] { typeof(ushort), typeof(bool), tbType });
        MethodInfo mGet = jrmType.GetMethod("GetUturnAllowed", new Type[] { typeof(ushort), typeof(bool) });

        Console.WriteLine($"mSet: {mSet != null}, mGet: {mGet != null}");
    }
}

using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        var asm = Assembly.LoadFrom("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var slmType = asm.GetType("TrafficManager.Manager.Impl.SpeedLimitManager");
        var m = slmType.GetMethod("GetGameSpeedLimit", BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(uint), typeof(NetInfo.Lane) }, null);
        Console.WriteLine("Found method: " + (m != null ? m.ToString() : "NULL"));
    }
}

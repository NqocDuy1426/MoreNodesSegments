using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        var asm = Assembly.LoadFrom("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var slmType = asm.GetType("TrafficManager.Manager.Impl.SpeedLimitManager");
        var m1 = slmType.GetMethod("GetGameSpeedLimit", BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(uint), typeof(NetInfo.Lane) }, null);
        var m2 = slmType.GetMethod("CalculateGameSpeedLimit", BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(uint) }, null);
        Console.WriteLine("m1 (GetGameSpeedLimit): " + (m1 != null ? m1.ToString() : "NULL"));
        Console.WriteLine("m2 (CalculateGameSpeedLimit): " + (m2 != null ? m2.ToString() : "NULL"));
    }
}

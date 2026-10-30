using System;
using System.Reflection;
using System.Text;

class Program {
    static void Main() {
        string managed = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        // Let's inspect NetSegment.GetClosestLanePosition and TransportTool.GetStopPosition
        Assembly asm = Assembly.LoadFrom(managed + "/Assembly-CSharp.dll");
        Type tt = asm.GetType("TransportTool");
        MethodInfo mGetStop = tt.GetMethod("GetStopPosition", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        Console.WriteLine("GetStopPosition params:");
        foreach (var p in mGetStop.GetParameters()) {
            Console.WriteLine($"  {p.ParameterType} {p.Name}");
        }
    }
}

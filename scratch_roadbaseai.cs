using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        Assembly asm = Assembly.LoadFile("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/Assembly-CSharp.dll");
        Type t = asm.GetType("RoadBaseAI");
        foreach (MethodInfo m in t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
        {
            if (m.Name == "GetTrafficLightState")
            {
                Console.WriteLine("Method: " + m);
                foreach (var p in m.GetParameters()) Console.WriteLine("  " + p.ParameterType + " " + p.Name);
            }
        }
    }
}

using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        Assembly asm = Assembly.LoadFrom("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/Assembly-CSharp.dll");
        Type sm = asm.GetType("SimulationManager");
        foreach (MethodInfo m in sm.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
        {
            if (m.Name.Contains("SimulationStep"))
            {
                Console.WriteLine("Method: " + m.Name);
                foreach (var p in m.GetParameters())
                {
                    Console.WriteLine("  param: " + p.ParameterType + " " + p.Name);
                }
            }
        }
    }
}

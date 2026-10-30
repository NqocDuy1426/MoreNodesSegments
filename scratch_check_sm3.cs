using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        Assembly asm = Assembly.LoadFrom("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/Assembly-CSharp.dll");
        Type sm = asm.GetType("SimulationManager");
        foreach (MethodInfo m in sm.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy))
        {
            if (m.Name == "SimulationStep")
            {
                Console.WriteLine("DeclaringType: " + m.DeclaringType.FullName);
                Console.WriteLine("Attributes: " + m.Attributes);
                Console.WriteLine("IsPublic: " + m.IsPublic);
                Console.WriteLine("IsFamily: " + m.IsFamily);
                Console.WriteLine("IsPrivate: " + m.IsPrivate);
            }
        }
    }
}

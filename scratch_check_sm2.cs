using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        Assembly asm = Assembly.LoadFrom("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/Assembly-CSharp.dll");
        Type sm = asm.GetType("SimulationManager");
        try
        {
            MethodInfo m = sm.GetMethod("SimulationStep", BindingFlags.Public | BindingFlags.Instance);
            Console.WriteLine("Result: " + (m != null ? m.ToString() : "NULL"));
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception: " + ex);
        }
    }
}

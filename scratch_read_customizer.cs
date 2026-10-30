using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        var asm = Assembly.LoadFrom("MoreNetworkCapacity.Core.dll");
        var c = asm.GetType("MoreNetworkCapacity.NetManagerCustomizer");
        foreach (var m in c.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
        {
            Console.WriteLine(m.Name);
        }
    }
}

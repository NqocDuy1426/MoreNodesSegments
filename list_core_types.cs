using System;
using System.Reflection;

public class ListCoreTypes
{
    public static void Main()
    {
        var asm = Assembly.LoadFrom("MoreNetworkCapacity.Core.dll");
        Console.WriteLine("Types in MoreNetworkCapacity.Core.dll: " + asm.GetTypes().Length);
        foreach (var t in asm.GetTypes())
        {
            Console.WriteLine("Type: " + t.FullName);
            foreach (var m in t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                Console.WriteLine("  " + m.Name);
            }
        }
    }
}

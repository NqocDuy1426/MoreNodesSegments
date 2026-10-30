using System;
using System.Reflection;

class Program {
    static void Main() {
        var asm = Assembly.LoadFrom("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/Assembly-CSharp.dll");
        var sp = asm.GetType("SavePanel");
        foreach (var t in sp.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic)) {
            Console.WriteLine("Nested: " + t.Name);
            foreach (var m in t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)) {
                if (m.DeclaringType == t)
                    Console.WriteLine("  " + m.Name);
            }
        }
    }
}

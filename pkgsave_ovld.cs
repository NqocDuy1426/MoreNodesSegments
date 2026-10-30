using System;
using System.Reflection;

class Program {
    static void Main() {
        var asm = Assembly.LoadFrom("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/ColossalManaged.dll");
        var pkg = asm.GetType("ColossalFramework.Packaging.Package");
        foreach (var m in pkg.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)) {
            if (m.Name == "Save") Console.WriteLine(m.ToString());
        }
    }
}

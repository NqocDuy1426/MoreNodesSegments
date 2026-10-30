using System;
using System.Reflection;

class Program {
    static void Main() {
        var asm = Assembly.LoadFrom("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/ColossalManaged.dll");
        var pm = asm.GetType("ColossalFramework.Packaging.PackageManager");
        foreach (var t in pm.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic)) {
            if (t.Name.Contains("Filter")) Console.WriteLine(t.FullName);
        }
    }
}

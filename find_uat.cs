using System;
using System.Reflection;

class Program {
    static void Main() {
        var asm = Assembly.LoadFrom("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/Assembly-CSharp.dll");
        foreach (var t in asm.GetTypes()) {
            if (t.Name.Contains("UserAsset")) Console.WriteLine("Found in Assembly-CSharp: " + t.FullName);
        }
    }
}

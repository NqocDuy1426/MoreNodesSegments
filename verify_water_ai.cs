using System;
using System.Reflection;

class Program {
    static void Main() {
        string managed = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        Assembly asm = Assembly.LoadFrom(managed + "/Assembly-CSharp.dll");
        Type pnai = asm.GetType("PlayerNetAI");
        foreach (var t in asm.GetTypes()) {
            if (pnai.IsAssignableFrom(t)) {
                var method = t.GetMethod("BuildOnWater", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (method != null) {
                    Console.WriteLine($"NetAI with BuildOnWater: {t.FullName}");
                }
            }
        }
    }
}

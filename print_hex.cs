using System;
using System.Reflection;

class Program {
    static void Main() {
        AppDomain.CurrentDomain.AssemblyResolve += (sender, args) => {
            string dir = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/";
            string name = args.Name.Split(',')[0] + ".dll";
            string path = System.IO.Path.Combine(dir, name);
            if (System.IO.File.Exists(path)) return Assembly.LoadFrom(path);
            return null;
        };
        var asm = Assembly.LoadFrom("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/Assembly-CSharp.dll");
        var sp = asm.GetType("SavePanel");
        var it = sp.GetNestedType("<SaveGame>c__Iterator1", BindingFlags.Public | BindingFlags.NonPublic);
        var s5 = it.GetNestedType("<SaveGame>c__AnonStorey5", BindingFlags.Public | BindingFlags.NonPublic);
        var m = s5.GetMethod("<>m__0", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        var mb = m.GetMethodBody();
        byte[] il = mb.GetILAsByteArray();
        for (int i = 0; i < 40; i++) {
            Console.Write(il[i].ToString("X2") + " ");
        }
        Console.WriteLine();
    }
}

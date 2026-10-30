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
        var it = sp.GetNestedType("<Snapshot>c__Iterator0", BindingFlags.Public | BindingFlags.NonPublic);
        var s2 = it.GetNestedType("<Snapshot>c__AnonStorey2", BindingFlags.Public | BindingFlags.NonPublic);
        foreach (var m in s2.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
            Console.WriteLine("Method in s2: " + m.Name);
        }
    }
}

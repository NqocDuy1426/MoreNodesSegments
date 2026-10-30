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
        int[] tokens = { 0x04002BE7, 0x04002BD9, 0x04002BB4, 0x0A000DD6 };
        foreach (var t in tokens) {
            Console.WriteLine(string.Format("0x{0:X8}: {1}", t, m.Module.ResolveMember(t)));
        }
    }
}

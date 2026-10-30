using System;
using System.IO;
using System.Reflection;

class Program {
    static void Main() {
        AppDomain.CurrentDomain.AssemblyResolve += (sender, args) => {
            string dir = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/";
            string name = args.Name.Split(',')[0] + ".dll";
            string path = Path.Combine(dir, name);
            if (File.Exists(path)) return Assembly.LoadFrom(path);
            return null;
        };
        var asm = Assembly.LoadFrom("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/Assembly-CSharp.dll");
        var sp = asm.GetType("SavePanel");
        var it = sp.GetNestedType("<Snapshot>c__Iterator0", BindingFlags.Public | BindingFlags.NonPublic);
        var s2 = it.GetNestedType("<Snapshot>c__AnonStorey2", BindingFlags.Public | BindingFlags.NonPublic);
        var m = s2.GetMethod("<>m__0", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        var mb = m.GetMethodBody();
        byte[] il = mb.GetILAsByteArray();
        for (int i = 0; i < il.Length; i++) {
            byte b = il[i];
            if (b == 0x28 || b == 0x6f) {
                int token = BitConverter.ToInt32(il, i + 1);
                try {
                    var member = m.Module.ResolveMember(token);
                    Console.WriteLine(string.Format("IL_{0:X4}: call {1}::{2}", i, member.DeclaringType.Name, member.Name));
                } catch {}
            }
        }
    }
}

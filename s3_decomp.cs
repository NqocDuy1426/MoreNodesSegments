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
        var s3 = s2.GetNestedType("<Snapshot>c__AnonStorey3", BindingFlags.Public | BindingFlags.NonPublic);
        if (s3 != null) {
            Console.WriteLine("Found nested s3 in s2!");
            foreach (var m in s3.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
                Console.WriteLine("  s3 method: " + m.Name);
                var mb = m.GetMethodBody();
                if (mb != null) {
                    byte[] il = mb.GetILAsByteArray();
                    for (int i = 0; i < il.Length; i++) {
                        if (il[i] == 0x28 || il[i] == 0x6f) {
                            int token = BitConverter.ToInt32(il, i + 1);
                            var member = m.Module.ResolveMember(token);
                            Console.WriteLine(string.Format("    IL_{0:X4}: call {1}::{2}", i, member.DeclaringType.Name, member.Name));
                        }
                    }
                }
            }
        }
    }
}

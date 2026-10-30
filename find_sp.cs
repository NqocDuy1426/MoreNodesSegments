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
        void CheckType(Type t) {
            foreach (var m in t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)) {
                try {
                    var mb = m.GetMethodBody();
                    if (mb == null) continue;
                    byte[] il = mb.GetILAsByteArray();
                    for (int i = 0; i < il.Length - 4; i++) {
                        if (il[i] == 0x28 || il[i] == 0x6f) {
                            int token = BitConverter.ToInt32(il, i + 1);
                            var member = m.Module.ResolveMember(token);
                            if (member.Name == "SaveSimulationData") {
                                Console.WriteLine(t.FullName + "::" + m.Name + " CALLS SaveSimulationData");
                            }
                        }
                    }
                } catch {}
            }
            foreach (var nested in t.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic)) {
                CheckType(nested);
            }
        }
        CheckType(sp);
    }
}

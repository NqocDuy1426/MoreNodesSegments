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
        var fLock = sp.GetField("m_ImagesLock", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        Console.WriteLine("m_ImagesLock: " + fLock);
        
        Type[] types;
        try { types = asm.GetTypes(); } catch (ReflectionTypeLoadException ex) { types = ex.Types; }
        foreach (var t in types) {
            if (t == null) continue;
            foreach (var m in t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)) {
                try {
                    var mb = m.GetMethodBody();
                    if (mb == null) continue;
                    byte[] il = mb.GetILAsByteArray();
                    for (int i = 0; i < il.Length - 4; i++) {
                        if (il[i] == 0x7B || il[i] == 0x7C || il[i] == 0x7D || il[i] == 0x7E || il[i] == 0x80) {
                            int token = BitConverter.ToInt32(il, i + 1);
                            if (token == fLock.MetadataToken) {
                                Console.WriteLine("Touched by: " + t.FullName + "::" + m.Name);
                            }
                        }
                    }
                } catch {}
            }
        }
    }
}

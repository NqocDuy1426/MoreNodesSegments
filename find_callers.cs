using System;
using System.Reflection;

class Program {
    static void Main() {
        var asm = Assembly.LoadFrom("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/Assembly-CSharp.dll");
        Type[] types;
        try {
            types = asm.GetTypes();
        } catch (ReflectionTypeLoadException ex) {
            types = ex.Types;
        }
        foreach (var t in types) {
            if (t == null) continue;
            foreach (var m in t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)) {
                var mb = m.GetMethodBody();
                if (mb == null) continue;
                byte[] il = mb.GetILAsByteArray();
                for (int i = 0; i < il.Length - 4; i++) {
                    if (il[i] == 0x28 || il[i] == 0x6f) {
                        int token = BitConverter.ToInt32(il, i + 1);
                        try {
                            var member = m.Module.ResolveMember(token);
                            if (member.Name == "SaveSimulationData") {
                                Console.WriteLine(t.FullName + "::" + m.Name + " calls SaveSimulationData");
                            }
                        } catch {}
                    }
                }
            }
        }
    }
}

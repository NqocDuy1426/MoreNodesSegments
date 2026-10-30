using System;
using System.Reflection;

class Program {
    static void Main() {
        var asm = Assembly.LoadFrom("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/Assembly-CSharp.dll");
        var sp = asm.GetType("SavePanel");
        var it = sp.GetNestedType("<SaveGame>c__Iterator1", BindingFlags.Public | BindingFlags.NonPublic);
        foreach (var nt in it.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic)) {
            Console.WriteLine("Nested in Iterator1: " + nt.Name);
            foreach (var m in nt.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
                Console.WriteLine("  Method: " + m.Name);
                var mb = m.GetMethodBody();
                if (mb != null) {
                    byte[] il = mb.GetILAsByteArray();
                    for (int i = 0; i < il.Length; i++) {
                        if (il[i] == 0x28 || il[i] == 0x6f) {
                            int token = BitConverter.ToInt32(il, i + 1);
                            try {
                                var member = m.Module.ResolveMember(token);
                                Console.WriteLine($"    call {member.DeclaringType.Name}::{member.Name}");
                            } catch {}
                        }
                    }
                }
            }
        }
    }
}

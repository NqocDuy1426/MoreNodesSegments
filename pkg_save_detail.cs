using System;
using System.Reflection;

class Program {
    static void Main() {
        var asm = Assembly.LoadFrom("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/ColossalManaged.dll");
        var pkg = asm.GetType("ColossalFramework.Packaging.Package");
        var m = pkg.GetMethod("Save", new Type[] { typeof(string), typeof(bool) });
        Console.WriteLine("Save: " + m);
        var mb = m.GetMethodBody();
        byte[] il = mb.GetILAsByteArray();
        for (int i = 0; i < il.Length; i++) {
            if (il[i] == 0x28 || il[i] == 0x6f) {
                int token = BitConverter.ToInt32(il, i + 1);
                try {
                    var member = m.Module.ResolveMember(token);
                    Console.WriteLine(string.Format("  IL_{0:X4}: call {1}::{2}", i, member.DeclaringType.Name, member.Name));
                } catch {}
            }
        }
    }
}

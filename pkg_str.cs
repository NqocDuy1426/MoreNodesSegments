using System;
using System.Reflection;

class Program {
    static void Main() {
        var asm = Assembly.LoadFrom("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/ColossalManaged.dll");
        var pkg = asm.GetType("ColossalFramework.Packaging.Package");
        var m = pkg.GetMethod("Save", new Type[] { typeof(string), typeof(bool) });
        var mb = m.GetMethodBody();
        byte[] il = mb.GetILAsByteArray();
        for (int i = 0; i < il.Length; i++) {
            if (il[i] == 0x72) {
                int token = BitConverter.ToInt32(il, i + 1);
                Console.WriteLine("String: " + m.Module.ResolveString(token));
            }
        }
    }
}

using System;
using System.Reflection;

class Program {
    static void Main() {
        var asm = Assembly.LoadFrom("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/ColossalManaged.dll");
        var t = asm.GetType("ColossalFramework.IO.DataSerializer");
        var m = t.GetMethod("ReadObjectArray", BindingFlags.Public | BindingFlags.Instance, null, new Type[] { t.GetNestedType("LegacyResolverHandler") }, null);
        var mb = m.GetMethodBody();
        byte[] il = mb.GetILAsByteArray();
        var module = m.Module;
        int i = 0;
        while (i < il.Length) {
            byte op = il[i++];
            if (op == 0x28 || op == 0x6F || op == 0x73) {
                int token = BitConverter.ToInt32(il, i);
                i += 4;
                try { Console.WriteLine("Call: " + module.ResolveMember(token)); } catch {}
            } else if (op == 0x20 || op == 0x38 || op == 0x39 || op == 0x3A || op == 0x3B || op == 0x3C || op == 0x3D || op == 0x3E || op == 0x3F || op == 0x40 || op == 0x41 || op == 0x42 || op == 0x7B || op == 0x7E) {
                i += 4;
            } else if (op == 0x72) {
                int token = BitConverter.ToInt32(il, i);
                i += 4;
                try { Console.WriteLine("Str: " + module.ResolveString(token)); } catch {}
            }
        }
    }
}

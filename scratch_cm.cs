using System;
using System.Reflection;

class Program {
    static void Main() {
        var asm = Assembly.LoadFrom("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/ColossalManaged.dll");
        var t = asm.GetType("ColossalFramework.Packaging.Package");
        var aType = t.GetNestedType("Asset", BindingFlags.Public | BindingFlags.NonPublic);
        var m = t.GetMethod("Instantiate", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[]{ aType }, null);
        var mb = m.GetMethodBody();
        byte[] il = mb.GetILAsByteArray();
        for (int i = 0; i < il.Length; i++) {
            if (il[i] == 0x28 || il[i] == 0x6f) {
                int token = BitConverter.ToInt32(il, i + 1);
                try {
                    var member = m.Module.ResolveMember(token);
                    Console.WriteLine($"call {member.DeclaringType.Name}::{member.Name}");
                } catch {}
            }
        }
    }
}

using System;
using System.Reflection;

class Program {
    static void Main() {
        var asm = Assembly.LoadFrom("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/Assembly-CSharp.dll");
        var baseType = asm.GetType("LoadSavePanelBase`1");
        var metaType = asm.GetType("SaveGameMetaData");
        var t = baseType.MakeGenericType(metaType);
        var m = t.GetMethod("EmbedAssetInfo", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        Console.WriteLine("EmbedAssetInfo: " + m);
        var mb = m.GetMethodBody();
        if (mb != null) {
            byte[] il = mb.GetILAsByteArray();
            Console.WriteLine("IL length: " + il.Length);
            for (int i = 0; i < il.Length; i++) {
                if (il[i] == 0x28 || il[i] == 0x6f) {
                    int token = BitConverter.ToInt32(il, i + 1);
                    try {
                        var member = m.Module.ResolveMember(token);
                        Console.WriteLine("  call " + member.DeclaringType.Name + "::" + member.Name);
                    } catch {}
                }
            }
        }
    }
}

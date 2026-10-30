using System;
using System.IO;
using System.Reflection;
using System.Text;

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
        var it = sp.GetNestedType("<SaveGame>c__Iterator1", BindingFlags.Public | BindingFlags.NonPublic);
        var s5 = it.GetNestedType("<SaveGame>c__AnonStorey5", BindingFlags.Public | BindingFlags.NonPublic);
        var m5 = s5.GetMethod("<>m__0", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        
        Console.WriteLine("=== SavePanel.<SaveGame>c__Iterator1::MoveNext ===");
        PrintInstructions(it.GetMethod("MoveNext", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));

        Console.WriteLine("\n=== <SaveGame>c__AnonStorey5::< >m__0 ===");
        PrintInstructions(m5);
    }

    static void PrintInstructions(MethodInfo m) {
        var mb = m.GetMethodBody();
        if (mb == null) { Console.WriteLine("No body"); return; }
        byte[] il = mb.GetILAsByteArray();
        int i = 0;
        while (i < il.Length) {
            int offset = i;
            byte b = il[i++];
            string op = "0x" + b.ToString("X2");
            string extra = "";
            if (b == 0xFE) {
                byte b2 = il[i++];
                op = "FE " + b2.ToString("X2");
            } else if (b == 0x28 || b == 0x6F) { // call, callvirt
                int token = BitConverter.ToInt32(il, i);
                i += 4;
                try {
                    var member = m.Module.ResolveMember(token);
                    extra = (b == 0x28 ? "call " : "callvirt ") + member.DeclaringType.Name + "::" + member.Name;
                } catch {
                    extra = "token 0x" + token.ToString("X8");
                }
            } else if (b >= 0x38 && b <= 0x44 || b >= 0x2B && b <= 0x37) {
                sbyte target = (sbyte)il[i++];
                extra = "-> IL_" + (i + target).ToString("X4");
            } else if (b == 0x7E || b == 0x7B || b == 0x7C || b == 0x80) { // ldfld, stfld, etc
                int token = BitConverter.ToInt32(il, i);
                i += 4;
                try {
                    var member = m.Module.ResolveMember(token);
                    extra = "field " + member.DeclaringType.Name + "::" + member.Name;
                } catch { extra = "field 0x" + token.ToString("X8"); }
            } else if (b == 0x20 || b == 0x72) { // ldc.i4, ldstr
                int token = BitConverter.ToInt32(il, i);
                i += 4;
                if (b == 0x72) {
                    try { extra = "\"" + m.Module.ResolveString(token) + "\""; } catch {}
                } else extra = token.ToString();
            } else if (b == 0x1F) { // ldc.i4.s
                sbyte val = (sbyte)il[i++];
                extra = val.ToString();
            } else if (b >= 0x14 && b <= 0x19) { // ldloc
                extra = "loc";
            } else if (b == 0x38 || b == 0x39 || b == 0x3A || b == 0x3B || b == 0x3C || b == 0x3D) {
                int target = BitConverter.ToInt32(il, i);
                i += 4;
                extra = "-> IL_" + (i + target).ToString("X4");
            }
            if (extra != "") {
                Console.WriteLine(string.Format("IL_{0:X4}: {1} {2}", offset, op, extra));
            }
        }
    }
}

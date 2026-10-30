using System;
using System.Reflection;
using System.Reflection.Emit;

class Program {
    static void Main() {
        var asm = Assembly.LoadFrom("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/Assembly-CSharp.dll");
        var sp = asm.GetType("SavePanel");
        var it = sp.GetNestedType("<SaveGame>c__Iterator1", BindingFlags.Public | BindingFlags.NonPublic);
        var storey = it.GetNestedType("<SaveGame>c__AnonStorey5", BindingFlags.Public | BindingFlags.NonPublic);
        var m = storey.GetMethod("<>m__0", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        
        var mb = m.GetMethodBody();
        byte[] il = mb.GetILAsByteArray();
        Console.WriteLine("Method <>m__0 IL length: " + il.Length);
        
        int offset = 0;
        while (offset < il.Length) {
            int cur = offset;
            byte b = il[offset++];
            OpCode op;
            if (b == 0xFE) {
                byte b2 = il[offset++];
                // two-byte opcode
                Console.WriteLine(string.Format("IL_{0:X4}: FE {1:X2}", cur, b2));
                continue;
            }
            if (b == 0x28 || b == 0x6f) { // call, callvirt
                int token = BitConverter.ToInt32(il, offset);
                offset += 4;
                string name = "";
                try {
                    var member = m.Module.ResolveMember(token);
                    name = member.DeclaringType.Name + "::" + member.Name;
                } catch { name = token.ToString("X"); }
                Console.WriteLine(string.Format("IL_{0:X4}: {1} {2}", cur, (b == 0x28 ? "call" : "callvirt"), name));
            } else if (b >= 0x38 && b <= 0x44) { // br, brtrue, brfalse, etc (short)
                sbyte target = (sbyte)il[offset++];
                Console.WriteLine(string.Format("IL_{0:X4}: br_s -> IL_{1:X4}", cur, offset + target));
            } else if (b >= 0x2B && b <= 0x37) { // br, etc (short)
                sbyte target = (sbyte)il[offset++];
                Console.WriteLine(string.Format("IL_{0:X4}: br_s -> IL_{1:X4}", cur, offset + target));
            } else {
                // simple skip
            }
        }
    }
}

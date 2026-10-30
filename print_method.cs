using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;

class Program {
    static void Main() {
        var m = typeof(LoadingProfiler).GetMethod("EndDeserialize", BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(ColossalFramework.IO.DataSerializer), typeof(string) }, null);
        var mb = m.GetMethodBody();
        byte[] il = mb.GetILAsByteArray();
        var module = m.Module;
        
        // Let's resolve tokens by looking at instructions
        int i = 0;
        while (i < il.Length) {
            int offset = i;
            byte op = il[i++];
            Console.Write(string.Format("IL_{0:X4}: {1:X2} ", offset, op));
            if (op == 0x28 || op == 0x6F || op == 0x73) { // call, callvirt, newobj
                int token = BitConverter.ToInt32(il, i);
                i += 4;
                try {
                    var member = module.ResolveMember(token);
                    Console.WriteLine("call " + member);
                } catch {
                    Console.WriteLine("token 0x" + token.ToString("X8"));
                }
            } else if (op == 0x7B || op == 0x7E) { // ldfld, ldsfld
                int token = BitConverter.ToInt32(il, i);
                i += 4;
                try {
                    var member = module.ResolveMember(token);
                    Console.WriteLine("ldfld " + member);
                } catch {
                    Console.WriteLine("token 0x" + token.ToString("X8"));
                }
            } else if (op == 0x72) { // ldstr
                int token = BitConverter.ToInt32(il, i);
                i += 4;
                try {
                    string s = module.ResolveString(token);
                    Console.WriteLine("ldstr \"" + s + "\"");
                } catch {
                    Console.WriteLine("str token 0x" + token.ToString("X8"));
                }
            } else if (op == 0x20) { // ldc.i4
                int val = BitConverter.ToInt32(il, i);
                i += 4;
                Console.WriteLine("ldc.i4 " + val);
            } else if (op == 0x38 || op == 0x39 || op == 0x3A || op == 0x3B || op == 0x3C || op == 0x3D || op == 0x3E || op == 0x3F || op == 0x40 || op == 0x41 || op == 0x42) { // br...
                int target = BitConverter.ToInt32(il, i);
                i += 4;
                Console.WriteLine("br offset " + target);
            } else {
                Console.WriteLine();
            }
        }
    }
}

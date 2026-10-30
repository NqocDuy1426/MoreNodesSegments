using System;
using System.Reflection;
using System.Reflection.Emit;

class Program {
    static void Main() {
        var m = typeof(ColossalFramework.Packaging.PackageDeserializer).GetMethod("Deserialize", BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { typeof(ColossalFramework.Packaging.Package), typeof(ColossalFramework.Packaging.PackageReader) }, null);
        var mb = m.GetMethodBody();
        byte[] il = mb.GetILAsByteArray();
        var module = m.Module;
        
        int i = 0;
        while (i < il.Length) {
            int offset = i;
            byte op = il[i++];
            Console.Write(string.Format("IL_{0:X4}: {1:X2} ", offset, op));
            if (op == 0x28 || op == 0x6F || op == 0x73) {
                int token = BitConverter.ToInt32(il, i);
                i += 4;
                try { Console.WriteLine("call " + module.ResolveMember(token)); } catch { Console.WriteLine("token 0x" + token.ToString("X8")); }
            } else if (op == 0x7B || op == 0x7E) {
                int token = BitConverter.ToInt32(il, i);
                i += 4;
                try { Console.WriteLine("ldfld " + module.ResolveMember(token)); } catch { Console.WriteLine("token 0x" + token.ToString("X8")); }
            } else if (op == 0x72) {
                int token = BitConverter.ToInt32(il, i);
                i += 4;
                try { Console.WriteLine("ldstr \"" + module.ResolveString(token) + "\""); } catch { Console.WriteLine("str 0x" + token.ToString("X8")); }
            } else if (op == 0x20) {
                int val = BitConverter.ToInt32(il, i);
                i += 4;
                Console.WriteLine("ldc.i4 " + val);
            } else {
                Console.WriteLine();
            }
        }
    }
}

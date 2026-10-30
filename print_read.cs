using System;
using System.Reflection;

class Program {
    static void Main() {
        var t = typeof(ColossalFramework.IO.EncodedArray.UShort);
        var rm = t.GetMethod("Read", BindingFlags.Public | BindingFlags.Instance);
        var mb = rm.GetMethodBody();
        byte[] il = mb.GetILAsByteArray();
        var module = t.Module;
        int i = 0;
        while (i < il.Length) {
            int offset = i;
            byte op = il[i++];
            Console.Write(string.Format("IL_{0:X4}: {1:X2} ", offset, op));
            if (op == 0x28 || op == 0x6F || op == 0x73) {
                int token = BitConverter.ToInt32(il, i);
                i += 4;
                try { Console.WriteLine("call " + module.ResolveMember(token)); } catch { Console.WriteLine("call token"); }
            } else if (op == 0x7B || op == 0x7E) {
                int token = BitConverter.ToInt32(il, i);
                i += 4;
                try { Console.WriteLine("ldfld " + module.ResolveMember(token)); } catch { Console.WriteLine("ldfld token"); }
            } else if (op == 0x7D || op == 0x80) {
                int token = BitConverter.ToInt32(il, i);
                i += 4;
                try { Console.WriteLine("stfld " + module.ResolveMember(token)); } catch { Console.WriteLine("stfld token"); }
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

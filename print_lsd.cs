using System;
using System.Reflection;
using System.Collections.Generic;

class Program {
    static void Main() {
        var asm = Assembly.LoadFrom("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        foreach (var type in asm.GetTypes()) {
            if (type.DeclaringType != null && type.DeclaringType.Name == "LoadingManager" && type.Name.Contains("LoadSimulationData")) {
                Console.WriteLine("Found iterator: " + type.Name);
                var m = type.GetMethod("MoveNext", BindingFlags.Public | BindingFlags.Instance);
                var mb = m.GetMethodBody();
                byte[] il = mb.GetILAsByteArray();
                var module = m.Module;
                int i = 0;
                while (i < il.Length) {
                    byte op = il[i++];
                    if (op == 0x28 || op == 0x6F) {
                        int token = BitConverter.ToInt32(il, i);
                        i += 4;
                        try {
                            var member = module.ResolveMember(token);
                            if (member.Name.Contains("Deserialize"))
                                Console.WriteLine("  Call: " + member);
                        } catch {}
                    } else if (op == 0x72) {
                        int token = BitConverter.ToInt32(il, i);
                        i += 4;
                        try {
                            string s = module.ResolveString(token);
                            if (s.Length > 2)
                                Console.WriteLine("  String: " + s);
                        } catch {}
                    } else if (op == 0x20 || op == 0x38 || op == 0x39 || op == 0x3A || op == 0x3B || op == 0x3C || op == 0x3D || op == 0x3E || op == 0x3F || op == 0x40 || op == 0x41 || op == 0x42 || op == 0x7B || op == 0x7E || op == 0x73) {
                        i += 4;
                    }
                }
            }
        }
    }
}

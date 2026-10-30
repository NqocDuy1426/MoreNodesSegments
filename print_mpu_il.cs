using System;
using System.Reflection;

class Program {
    static void Main() {
        var asm = Assembly.LoadFrom("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/2710657019/MorePathUnits.dll");
        var t = asm.GetType("MorePathUnits.PathDeserialize");
        var m = t.GetMethod("get_DeserializeSize", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        byte[] il = m.GetMethodBody().GetILAsByteArray();
        Console.WriteLine("get_DeserializeSize IL: " + BitConverter.ToString(il));
        
        var tMeta = asm.GetType("MorePathUnits.MetaData");
        if (tMeta != null) {
            foreach (var mm in tMeta.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)) {
                Console.WriteLine("MetaData." + mm.Name + ": " + BitConverter.ToString(mm.GetMethodBody().GetILAsByteArray()));
            }
        }
    }
}

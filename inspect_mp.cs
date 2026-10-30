using System;
using System.Reflection;

class Program {
    static void Main() {
        AppDomain.CurrentDomain.AssemblyResolve += (sender, args) => {
            string dir = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/";
            string name = args.Name.Split(',')[0] + ".dll";
            string path = System.IO.Path.Combine(dir, name);
            if (System.IO.File.Exists(path)) return Assembly.LoadFrom(path);
            return null;
        };
        var asm = Assembly.LoadFrom("/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Addons/Mods/MorePropsBETA/MorePropsBETA.dll");
        Type[] types;
        try { types = asm.GetTypes(); } catch (ReflectionTypeLoadException ex) { types = ex.Types; }
        foreach (var t in types) {
            if (t == null) continue;
            Console.WriteLine(t.FullName);
            foreach (var m in t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)) {
                if (m.Name.Contains("Save") || m.Name.Contains("Serialize") || m.Name.Contains("Package") || m.Name.Contains("Patch"))
                    Console.WriteLine("  " + m.Name);
            }
        }
    }
}

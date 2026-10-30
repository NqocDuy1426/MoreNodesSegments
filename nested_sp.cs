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
        var asm = Assembly.LoadFrom("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/Assembly-CSharp.dll");
        var sp = asm.GetType("SavePanel");
        void ListNested(Type t, int indent) {
            foreach (var nt in t.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic)) {
                Console.WriteLine(new string(' ', indent) + nt.Name);
                ListNested(nt, indent + 2);
            }
        }
        ListNested(sp, 0);
    }
}

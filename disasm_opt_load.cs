using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var type = asm.MainModule.GetType("TrafficManager.Manager.Impl.OptionsManager");
        var m = type.Methods.First(x => x.Name == "LoadData" && x.Parameters.Count == 1);
        int i = 0;
        foreach (var inst in m.Body.Instructions)
        {
            if (i < 80)
                Console.WriteLine(i + ": " + inst);
            i++;
        }
    }
}
static class Ext {
    public static T First<T>(this System.Collections.Generic.IEnumerable<T> e, Func<T, bool> p) {
        foreach (var x in e) if (p(x)) return x;
        return default(T);
    }
}

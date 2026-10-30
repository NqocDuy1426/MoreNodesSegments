using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var type = asm.MainModule.GetType("TrafficManager.Custom.PathFinding.CustomPathFind");
        var m = type.Methods.First(x => x.Name == "ProcessItemRouted");
        foreach (var inst in m.Body.Instructions)
        {
            if (inst.Offset >= 0x0240 && inst.Offset <= 0x0280)
            {
                Console.WriteLine(string.Format("IL_{0:x4}: {1} {2}", inst.Offset, inst.OpCode, inst.Operand));
            }
        }
    }
}
static class Ext {
    public static T First<T>(this System.Collections.Generic.IEnumerable<T> e, Func<T, bool> p) {
        foreach (var x in e) if (p(x)) return x;
        return default(T);
    }
}

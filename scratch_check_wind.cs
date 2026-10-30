using System;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var wm = asm.MainModule.GetType("WindManager");
        Console.WriteLine("WindManager exists: " + (wm != null));
    }
}


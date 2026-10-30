using System;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main(string[] args) {
        string dll = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll";
        var asm = AssemblyDefinition.ReadAssembly(dll);
        foreach (var type in asm.MainModule.GetTypes()) {
            if (type.Name.Contains("MessageManager")) {
                Console.WriteLine("Type: " + type.FullName);
                foreach (var nested in type.NestedTypes) {
                    Console.WriteLine("  Nested: " + nested.FullName);
                    foreach (var m in nested.Methods) {
                        Console.WriteLine("    Method: " + m.FullName);
                    }
                }
                foreach (var m in type.Methods) {
                    Console.WriteLine("  Method: " + m.FullName);
                }
            }
        }
    }
}

using System;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string p = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + 
            "/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(p);

        foreach (var t in asm.MainModule.Types)
        {
            SearchType(t);
        }
    }

    static void SearchType(TypeDefinition t)
    {
        foreach (var nested in t.NestedTypes) SearchType(nested);
        foreach (var m in t.Methods)
        {
            if (m.Body == null) continue;
            foreach (var inst in m.Body.Instructions)
            {
                var mr = inst.Operand as MethodReference;
                if (mr != null && mr.Name.Contains("OnBeforeSimulationStep"))
                {
                    Console.WriteLine($"{t.FullName}::{m.Name} -> {mr.DeclaringType.Name}::{mr.Name}");
                }
            }
        }
    }
}

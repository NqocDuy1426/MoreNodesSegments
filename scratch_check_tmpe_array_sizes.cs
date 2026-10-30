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
            if (t.Namespace != null && t.Namespace.Contains("TrafficManager.Manager.Impl"))
            {
                foreach (var m in t.Methods)
                {
                    if (m.Name == ".ctor" && m.Body != null)
                    {
                        for (int i = 0; i < m.Body.Instructions.Count; i++)
                        {
                            var inst = m.Body.Instructions[i];
                            if (inst.OpCode == OpCodes.Newarr)
                            {
                                var prev = m.Body.Instructions[i - 1];
                                Console.WriteLine($"{t.Name} creates array of {inst.Operand} with size {prev.OpCode} {prev.Operand}");
                            }
                        }
                    }
                }
            }
        }
    }
}

using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Addons/Mods/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        foreach (var t in asm.MainModule.Types)
        {
            if (t.Name.Contains("NetSegmentExtension") || t.Name.Contains("NetNodeExtension") || t.Name.Contains("NetLaneExtension"))
            {
                Console.WriteLine($"=== {t.FullName} ===");
                foreach (var f in t.Fields)
                {
                    Console.WriteLine($"  Field: {f.Name} : {f.FieldType.FullName} (static={f.IsStatic})");
                }
                foreach (var m in t.Methods)
                {
                    if (m.Name.Contains("To") || m.Name.Contains("Buffer") || m.Name.Contains("Init"))
                    {
                        Console.WriteLine($"  Method: {m.Name}");
                        foreach (var inst in m.Body.Instructions)
                        {
                            Console.WriteLine($"    {inst.OpCode} {inst.Operand}");
                        }
                    }
                }
            }
        }
    }
}

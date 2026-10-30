using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string tmpePath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var assembly = AssemblyDefinition.ReadAssembly(tmpePath);
        foreach (var type in assembly.MainModule.Types)
        {
            if (type.FullName.Contains("NetSegmentExtensions") || type.FullName.Contains("NetNodeExtensions"))
            {
                Console.WriteLine("=== Type: " + type.FullName + " ===");
                foreach (var f in type.Fields)
                {
                    Console.WriteLine(string.Format("  Field: {0} ({1}) isStatic={2}", f.Name, f.FieldType.FullName, f.IsStatic));
                }
                foreach (var m in type.Methods)
                {
                    if (m.Name == "ToSegment" || m.Name == "ToNode")
                    {
                        Console.WriteLine("  Method: " + m.FullName);
                        foreach (var inst in m.Body.Instructions)
                        {
                            Console.WriteLine("    " + inst.OpCode + " " + inst.Operand);
                        }
                    }
                }
            }
        }
    }
}

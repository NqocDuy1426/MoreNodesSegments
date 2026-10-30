using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string tmpePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var assembly = AssemblyDefinition.ReadAssembly(tmpePath);

        PrintMethod(assembly, "TrafficManager.Patch.NetManagerEvents", "ReleaseSegmentImplementationPostfix");
        PrintMethod(assembly, "TrafficManager.Patch.NetManagerEvents", "PreReleaseSegmentImplementationPrefix");

        Console.WriteLine("\n--- Searching for references to ReleasingSegment or ReleasedSegment ---");
        foreach (var t in assembly.Modules.SelectMany(m => m.Types))
        {
            foreach (var m in t.Methods)
            {
                if (!m.HasBody) continue;
                foreach (var i in m.Body.Instructions)
                {
                    if (i.Operand != null && i.Operand.ToString().Contains("Releas"))
                    {
                        Console.WriteLine(t.FullName + "::" + m.Name + " -> " + i.Operand);
                    }
                }
            }
        }
    }

    static void PrintMethod(AssemblyDefinition asm, string typeName, string methodName)
    {
        Console.WriteLine("================ " + typeName + " :: " + methodName + " ================");
        var type = asm.Modules.SelectMany(m => m.Types).FirstOrDefault(t => t.FullName == typeName);
        if (type == null) { Console.WriteLine("Type not found!"); return; }
        var method = type.Methods.FirstOrDefault(m => m.Name == methodName);
        if (method == null) { Console.WriteLine("Method not found!"); return; }

        if (!method.HasBody) { Console.WriteLine("No body!"); return; }
        foreach (var instr in method.Body.Instructions)
        {
            Console.WriteLine(string.Format("IL_{0:X4}: {1} {2}", instr.Offset, instr.OpCode, instr.Operand));
        }
    }
}

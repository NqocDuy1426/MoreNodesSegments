using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var gameDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + 
            "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var asm = AssemblyDefinition.ReadAssembly(gameDir + "/Assembly-CSharp.dll");

        DumpMethod(asm, "NetSegment", "PopulateGroupData");
        DumpMethod(asm, "NetNode", "PopulateGroupData");
    }

    static void DumpMethod(AssemblyDefinition asm, string typeName, string methodName)
    {
        var type = asm.MainModule.GetType(typeName);
        foreach (var m in type.Methods)
        {
            if (m.Name == methodName)
            {
                Console.WriteLine($"=== {typeName}.{m.Name} ===");
                for (int i = 0; i < m.Body.Instructions.Count; i++)
                {
                    var inst = m.Body.Instructions[i];
                    var mr = inst.Operand as MethodReference;
                    if (mr != null && mr.Name == "GetColorLocation")
                    {
                        for (int j = Math.Max(0, i - 6); j <= i + 1; j++)
                        {
                            var inst2 = m.Body.Instructions[j];
                            Console.WriteLine($"  IL_{inst2.Offset:X4}: {inst2.OpCode} {inst2.Operand}");
                        }
                    }
                }
            }
        }
    }
}

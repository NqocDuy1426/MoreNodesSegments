using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class PrintDesPatch
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("content/MoreNetworkCapacity.Core.dll");
        foreach (var type in asm.MainModule.Types)
        {
            if (type.FullName.Contains("NetManagerDeserializePatch") || type.FullName.Contains("NetManagerDeserializeState"))
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Type: " + type.FullName);
                foreach (var field in type.Fields)
                {
                    Console.WriteLine("  Field: " + field.Name + " : " + field.FieldType.FullName);
                }
                foreach (var m in type.Methods)
                {
                    Console.WriteLine("  Method: " + m.Name + " (" + string.Join(", ", m.Parameters.Select(p => p.ParameterType.Name + " " + p.Name).ToArray()) + ")");
                    if (m.HasBody)
                    {
                        foreach (var inst in m.Body.Instructions)
                        {
                            Console.WriteLine(string.Format("    {0}: {1} {2}", inst.Offset.ToString("X4"), inst.OpCode, inst.Operand));
                        }
                    }
                }
            }
        }
    }
}

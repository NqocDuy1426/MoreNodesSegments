using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/.gemini/antigravity-ide/brain/58f316be-e370-4c92-929c-6045614539fe/scratch/MoreNetworkCapacity.Core.dll");
        TypeDefinition td = asm.MainModule.GetType("MoreNetworkCapacity.Capacity");
        foreach (var m in td.Methods)
        {
            if (m.Name.Contains("BufferLength"))
            {
                Console.WriteLine("METHOD: " + m.FullName);
                foreach (var i in m.Body.Instructions)
                {
                    Console.WriteLine(string.Format("  {0} {1}", i.OpCode, i.Operand));
                }
            }
        }
    }
}

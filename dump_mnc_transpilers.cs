using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/.gemini/antigravity-ide/brain/58f316be-e370-4c92-929c-6045614539fe/scratch/MoreNetworkCapacity.Core.dll");
        foreach (var type in asm.MainModule.Types)
        {
            foreach (var m in type.Methods)
            {
                if (m.Name.ToLower().Contains("transpiler") || m.Name.ToLower().Contains("deserialize") || m.Name.ToLower().Contains("prefix") || m.Name.ToLower().Contains("postfix"))
                {
                    Console.WriteLine(type.FullName + " :: " + m.Name);
                }
            }
        }
    }
}

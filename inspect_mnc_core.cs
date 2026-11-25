using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/.gemini/antigravity-ide/brain/58f316be-e370-4c92-929c-6045614539fe/scratch/MoreNetworkCapacity.Core.dll");
        foreach (var type in asm.MainModule.Types)
        {
            if (type.Name.Contains("Net") || type.Name.Contains("Deserialize") || type.Name.Contains("Patch"))
            {
                Console.WriteLine("Type: " + type.FullName);
                foreach (var m in type.Methods)
                {
                    Console.WriteLine("  Method: " + m.Name);
                }
            }
        }
    }
}

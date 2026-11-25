using System;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/.gemini/antigravity-ide/brain/58f316be-e370-4c92-929c-6045614539fe/scratch/MoreNetworkCapacity.Core.dll");
        TypeDefinition td = asm.MainModule.GetType("MoreNetworkCapacity.NetManagerCustomizer");
        using (var sw = new StreamWriter("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/mnc_prep_dump.txt"))
        {
            foreach (var m in td.Methods)
            {
                sw.WriteLine("METHOD: " + m.FullName);
                if (m.HasBody)
                {
                    foreach (var i in m.Body.Instructions)
                    {
                        sw.WriteLine(string.Format("  {0} {1}", i.OpCode, i.Operand));
                    }
                }
            }
        }
        Console.WriteLine("Written to mnc_prep_dump.txt");
    }
}

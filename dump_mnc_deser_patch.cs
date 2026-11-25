using System;
using System.IO;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/.gemini/antigravity-ide/brain/58f316be-e370-4c92-929c-6045614539fe/scratch/MoreNetworkCapacity.Core.dll");
        TypeDefinition td = asm.MainModule.GetType("MoreNetworkCapacity.Patches.NetManagerDeserializePatch");
        using (var sw = new StreamWriter("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/mnc_deser_patch.txt"))
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
        Console.WriteLine("Done writing mnc_deser_patch.txt");
    }
}

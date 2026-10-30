using System;
using System.Linq;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var mi = asm.MainModule.GetType("MilestoneInfo/Data");
        var des = mi.Methods.FirstOrDefault(m => m.Name == "Deserialize");
        if (des != null)
        {
            Console.WriteLine("MilestoneInfo.Data.Deserialize:");
            foreach (var inst in des.Body.Instructions)
            {
                Console.WriteLine("  " + inst);
            }
        }
    }
}

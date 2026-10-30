using System;
using System.Linq;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var smData = asm.MainModule.GetType("SimulationManager/Data");
        var des = smData.Methods.First(m => m.Name == "Deserialize");
        for (int i = 0; i < 40; i++)
        {
            Console.WriteLine(des.Body.Instructions[i]);
        }
    }
}

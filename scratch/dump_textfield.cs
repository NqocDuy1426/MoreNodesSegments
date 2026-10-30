using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/ColossalManaged.dll";
        var assembly = AssemblyDefinition.ReadAssembly(path);
        var type = assembly.MainModule.GetType("ColossalFramework.UI.UITextField");
        foreach (var m in type.Methods)
        {
            if (m.Name == "set_text")
            {
                Console.WriteLine("--- " + m.ToString() + " ---");
                foreach (var ins in m.Body.Instructions)
                {
                    Console.WriteLine("  " + ins.ToString());
                }
            }
        }
    }
}

using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll";
        var assembly = AssemblyDefinition.ReadAssembly(path);
        var type = assembly.MainModule.GetType("TransportManager");
        foreach (var m in type.Methods)
        {
            if (m.Name.Contains("RenderLines"))
            {
                Console.WriteLine("--- " + m.ToString() + " ---");
                foreach (var ins in m.Body.Instructions)
                {
                    if (ins.ToString().Contains("m_lines") || ins.ToString().Contains("Render") || ins.ToString().Contains("CameraInfo"))
                    {
                        Console.WriteLine("  " + ins.ToString());
                    }
                }
            }
        }
    }
}

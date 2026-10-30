using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string lsmrPath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/2858591409/LoadingScreenModRevisited.dll";
        var assembly = AssemblyDefinition.ReadAssembly(lsmrPath);
        foreach (var type in assembly.MainModule.Types)
        {
            if (type.Name == "LevelLoader" || type.Name == "LoadingScreen")
            {
                foreach (var m in type.Methods)
                {
                    if (m.Name == "HasTaskFailed" || m.Name == "SimulationFailed")
                    {
                        Console.WriteLine("=== " + type.Name + "::" + m.Name + " ===");
                        foreach (var inst in m.Body.Instructions)
                        {
                            Console.WriteLine("  " + inst.OpCode + " " + inst.Operand);
                        }
                    }
                }
            }
        }
    }
}

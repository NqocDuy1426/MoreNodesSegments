using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll";
        var assembly = AssemblyDefinition.ReadAssembly(path);
        var type = assembly.MainModule.GetType("CarAI");
        foreach (var m in type.Methods)
        {
            if (m.Name == "CalculateSegmentPosition" && m.Parameters.Count >= 5)
            {
                Console.WriteLine("--- " + m.ToString() + " ---");
                foreach (var ins in m.Body.Instructions)
                {
                    if (ins.ToString().Contains("GetTrafficLightState") || ins.ToString().Contains("TrafficLight") || ins.ToString().Contains("m_maxSpeed"))
                    {
                        Console.WriteLine("  " + ins.ToString());
                    }
                }
            }
        }
    }
}

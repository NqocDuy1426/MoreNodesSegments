using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string tmpePath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(tmpePath);
        var type = asm.MainModule.GetType("TrafficManager.Manager.Impl.TrafficLightManager");
        Console.WriteLine("TrafficLightManager found: " + (type != null));
        if (type != null)
        {
            foreach (var f in type.Fields)
            {
                Console.WriteLine($"Field: {f.Name} : {f.FieldType.FullName}");
            }
            foreach (var m in type.Methods)
            {
                if (m.Name.Contains("TrafficLight") || m.Name.Contains("Step") || m.Name.Contains("Simulation"))
                {
                    Console.WriteLine($"Method: {m.FullName}");
                }
            }
        }
    }
}

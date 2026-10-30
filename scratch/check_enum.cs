using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll";
        var assembly = AssemblyDefinition.ReadAssembly(path);
        var type = assembly.MainModule.GetType("RoadBaseAI/TrafficLightState");
        foreach (var f in type.Fields)
        {
            if (f.IsStatic)
            {
                Console.WriteLine(f.Name + " = " + f.Constant);
            }
        }
    }
}

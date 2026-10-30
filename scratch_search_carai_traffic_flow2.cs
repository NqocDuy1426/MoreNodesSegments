using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string managed = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/";
        Assembly asm = Assembly.LoadFrom(managed + "Assembly-CSharp.dll");
        Type carAiType = asm.GetType("CarAI");
        foreach (var method in carAiType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            if (method.Name == "SimulationStep")
            {
                var body = method.GetMethodBody();
                if (body != null)
                {
                    Console.WriteLine($"Method: {method.Name}({string.Join(", ", Array.ConvertAll(method.GetParameters(), p => p.ParameterType.Name))}) length: {body.GetILAsByteArray().Length}");
                }
            }
        }
    }
}

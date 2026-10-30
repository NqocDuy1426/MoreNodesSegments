using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string managed = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/";
        Assembly asm = Assembly.LoadFrom(managed + "Assembly-CSharp.dll");
        Type roadAi = asm.GetType("RoadBaseAI");

        foreach (var m in roadAi.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            if (m.Name == "GetColor")
            {
                Console.WriteLine($"RoadBaseAI.{m.Name} ({string.Join(", ", Array.ConvertAll(m.GetParameters(), p => p.ParameterType.Name))})");
            }
        }
    }
}

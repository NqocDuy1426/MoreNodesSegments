using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string managed = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/";
        Assembly asm = Assembly.LoadFrom(managed + "Assembly-CSharp.dll");
        Type rmType = asm.GetType("RenderManager");
        foreach (var m in rmType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
        {
            if (m.Name == "GetColorLocation")
            {
                Console.WriteLine($"Method: {m.Name}({string.Join(", ", Array.ConvertAll(m.GetParameters(), p => p.ParameterType.Name))})");
                var body = m.GetMethodBody();
                if (body != null)
                {
                    byte[] il = body.GetILAsByteArray();
                    for (int i = 0; i < il.Length; i++)
                    {
                        Console.Write($"{il[i]:X2} ");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}

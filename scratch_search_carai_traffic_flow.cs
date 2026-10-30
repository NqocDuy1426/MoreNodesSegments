using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string managed = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/";
        Assembly asm = Assembly.LoadFrom(managed + "Assembly-CSharp.dll");
        Type carAiType = asm.GetType("CarAI");
        MethodInfo m = null;
        foreach (var method in carAiType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            if (method.Name == "SimulationStep" && method.GetParameters().Length == 3)
            {
                m = method;
                break;
            }
        }
        if (m == null)
        {
            Console.WriteLine("SimulationStep not found");
            return;
        }

        var body = m.GetMethodBody();
        byte[] il = body.GetILAsByteArray();
        Console.WriteLine($"CarAI.SimulationStep IL length: {il.Length}");

        // Let us look around offset 0x05F0 to 0x0660
        int start = Math.Max(0, 0x061E - 60);
        int end = Math.Min(il.Length, 0x063B + 60);
        Console.WriteLine($"Dumping IL from 0x{start:X4} to 0x{end:X4}");
    }
}

using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        var asm = Assembly.LoadFrom("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/MoreNetworkCapacity.Core.dll");
        foreach (var type in asm.GetTypes())
        {
            if (type.FullName.Contains("Signal") || type.FullName.Contains("CarAI") || type.FullName.Contains("FramePhase") || type.FullName.Contains("Patch"))
            {
                Console.WriteLine("Type: " + type.FullName);
                foreach (var m in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
                {
                    if (m.Name.Contains("Patch") || m.Name.Contains("Transpiler") || m.Name.Contains("Calculate") || m.Name.Contains("NeedStop"))
                    {
                        Console.WriteLine("  Method: " + m.Name);
                    }
                }
            }
        }
    }
}

using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string managed = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/";
        Assembly asm = Assembly.LoadFrom(managed + "Assembly-CSharp.dll");
        Type segType = asm.GetType("NetSegment");
        foreach (var m in segType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            if (m.Name == "PopulateGroupData")
            {
                var body = m.GetMethodBody();
                byte[] il = body.GetILAsByteArray();
                Console.WriteLine($"PopulateGroupData IL length: {il.Length}");
                // bytes around 0x04E0 to 0x0500
                for (int i = 0x04D0; i < 0x0500; i++)
                {
                    Console.Write($"{il[i]:X2} ");
                }
                Console.WriteLine();
                break;
            }
        }
    }
}

using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string managed = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/";
        Assembly asm = Assembly.LoadFrom(managed + "Assembly-CSharp.dll");
        Type idType = asm.GetType("InstanceID");
        foreach (var p in idType.GetProperties())
        {
            if (p.Name.Contains("Net") || p.Name.Contains("Raw"))
            {
                Console.WriteLine($"InstanceID.{p.Name} : {p.PropertyType.Name}");
            }
        }
        foreach (var m in idType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            if (m.Name.Contains("NetSegment") || m.Name.Contains("NetNode"))
            {
                Console.WriteLine($"Method: {m.Name}");
            }
        }
    }
}

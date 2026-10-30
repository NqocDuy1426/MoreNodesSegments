using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string managed = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/";
        Assembly asm = Assembly.LoadFrom(managed + "Assembly-CSharp.dll");
        Type nmType = asm.GetType("NetManager");

        foreach (var m in nmType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            if (m.Name.Contains("ColorMap") || m.Name.Contains("Colors"))
            {
                Console.WriteLine($"NetManager.{m.Name} ({string.Join(", ", Array.ConvertAll(m.GetParameters(), p => p.ParameterType.Name))})");
            }
        }
        foreach (var f in nmType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            if (f.Name.Contains("ColorMap") || f.Name.Contains("colorMap"))
            {
                Console.WriteLine($"NetManager Field: {f.Name} : {f.FieldType.Name}");
            }
        }
    }
}

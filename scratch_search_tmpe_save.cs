using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string tmpePath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        Assembly asm = Assembly.LoadFrom(tmpePath);

        foreach (Type t in asm.GetTypes())
        {
            if (t.Name.Contains("Serializable") || t.Name.Contains("SaveData") || t.Name.Contains("Save") || t.Name.Contains("LoadData"))
            {
                foreach (MethodInfo m in t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))
                {
                    if (m.Name.Contains("Save") || m.Name.Contains("Load") || m.Name.Contains("Serialize"))
                    {
                        Console.WriteLine($"{t.FullName}.{m.Name}");
                    }
                }
            }
        }
    }
}

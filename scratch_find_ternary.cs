using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string tmpePath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        Assembly asm = Assembly.LoadFrom(tmpePath);
        foreach (var t in asm.GetTypes())
        {
            if (t.Name == "TernaryBool")
            {
                Console.WriteLine($"Found {t.FullName}");
                foreach (var name in Enum.GetNames(t))
                {
                    Console.WriteLine($"  {name} = {(byte)Enum.Parse(t, name)}");
                }
            }
        }
    }
}

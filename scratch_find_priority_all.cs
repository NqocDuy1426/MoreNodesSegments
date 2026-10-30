using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string dir = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/";
        foreach (var file in System.IO.Directory.GetFiles(dir, "*.dll"))
        {
            try
            {
                Assembly asm = Assembly.LoadFrom(file);
                foreach (var t in asm.GetTypes())
                {
                    if (t.Name == "PriorityType")
                    {
                        Console.WriteLine($"Found {t.FullName} in {file}");
                        foreach (var name in Enum.GetNames(t))
                        {
                            Console.WriteLine($"  {name} = {Convert.ToInt32(Enum.Parse(t, name))}");
                        }
                    }
                }
            }
            catch {}
        }
    }
}

using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string dir = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/";
        Assembly asm = Assembly.LoadFrom(dir + "CSUtil.Commons.dll");
        Type t = asm.GetType("CSUtil.Commons.TernaryBool");
        foreach (var name in Enum.GetNames(t))
        {
            Console.WriteLine($"  {name} = {Convert.ToInt32(Enum.Parse(t, name))}");
        }
    }
}

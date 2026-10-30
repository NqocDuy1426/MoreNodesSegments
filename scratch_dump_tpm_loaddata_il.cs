using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string dir = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/";
        Assembly asm = Assembly.LoadFrom(dir + "TrafficManager.dll");
        Type t = asm.GetType("TrafficManager.Manager.Impl.TrafficPriorityManager");
        foreach (var m in t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            if (m.Name == "LoadData")
            {
                var body = m.GetMethodBody();
                if (body == null) continue;
                byte[] il = body.GetILAsByteArray();
                Console.WriteLine($"TPM LoadData({m.GetParameters()[0].ParameterType.Name}) length: {il.Length}");
                for (int i = 0; i < il.Length - 4; i++)
                {
                    if (il[i] == 0x20)
                    {
                        int val = BitConverter.ToInt32(il, i + 1);
                        if (val >= 10000) Console.WriteLine($"Constant {val} at IL_{i:X4}");
                    }
                }
            }
        }
    }
}

using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string tmpePath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        Assembly asm = Assembly.LoadFrom(tmpePath);
        Type t = asm.GetType("TrafficManager.Manager.Impl.ExtVehicleManager");
        foreach (MethodInfo m in t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            if (m.Name.Contains("Load") || m.Name.Contains("Init") || m.Name.Contains("Reset") || m.Name.Contains("Awake"))
            {
                Console.WriteLine("Method: " + m.Name);
                byte[] il = m.GetMethodBody().GetILAsByteArray();
                for (int i = 0; i < il.Length - 4; i++)
                {
                    if (il[i] == 0x20) // ldc.i4
                    {
                        int val = BitConverter.ToInt32(il, i + 1);
                        if (val > 1000 && val < 100000)
                        {
                            Console.WriteLine("  in " + m.Name + " found const: " + val);
                        }
                    }
                }
            }
        }
    }
}

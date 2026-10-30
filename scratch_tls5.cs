using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        Assembly asm = Assembly.LoadFile("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        Type tMgr = asm.GetType("TrafficManager.Manager.Impl.TrafficLightSimulationManager");
        foreach (ConstructorInfo ctor in tMgr.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            Console.WriteLine("Ctor: " + ctor);
            byte[] il = ctor.GetMethodBody().GetILAsByteArray();
            Console.WriteLine("Ctor IL length: " + il.Length);
            for (int i = 0; i < il.Length - 4; i++)
            {
                if (il[i] == 0x20 || il[i] == 0x8D) // ldc.i4 or newarr
                {
                    int val = BitConverter.ToInt32(il, i + 1);
                    if (val > 1000 && val < 100000)
                    {
                        Console.WriteLine("Found int in ctor: " + val);
                    }
                }
            }
        }
    }
}

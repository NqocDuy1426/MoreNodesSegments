using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        Assembly asm = Assembly.LoadFile("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        Type tSim = asm.GetType("TrafficManager.TrafficLight.Impl.TrafficLightSimulation");
        ConstructorInfo ctor = tSim.GetConstructor(new Type[] { typeof(ushort) });
        byte[] il = ctor.GetMethodBody().GetILAsByteArray();
        for (int i = 0; i < il.Length; i++)
        {
            Console.WriteLine(string.Format("{0:X2}: {1:X2}", i, il[i]));
        }
    }
}

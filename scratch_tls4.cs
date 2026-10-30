using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        Assembly asm = Assembly.LoadFile("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        MethodInfo m = (MethodInfo)asm.ManifestModule.ResolveMethod(0x060006CB);
        Console.WriteLine("Method: " + m.Name);
        byte[] il = m.GetMethodBody().GetILAsByteArray();
        for (int i = 0; i < il.Length; i++)
        {
            Console.WriteLine(string.Format("{0:X2}: {1:X2}", i, il[i]));
        }
    }
}

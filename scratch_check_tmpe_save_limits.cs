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
            MethodInfo m = t.GetMethod("SaveData", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
            if (m != null && m.GetMethodBody() != null)
            {
                byte[] il = m.GetMethodBody().GetILAsByteArray();
                for (int i = 0; i < il.Length - 4; i++)
                {
                    if (il[i] == 0x20) // ldc.i4
                    {
                        int val = BitConverter.ToInt32(il, i + 1);
                        if (val == 32768 || val == 32767 || val == 36864 || val == 36863)
                        {
                            Console.WriteLine($"{t.FullName}.SaveData contains hardcoded limit {val} at IL_{i:X4}");
                        }
                    }
                }
            }
        }
    }
}

using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string tmpePath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        Assembly asm = Assembly.LoadFrom(tmpePath);
        string apiPath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TMPE.API.dll";
        Assembly apiAsm = Assembly.LoadFrom(apiPath);
        
        Console.WriteLine("Searching for usages of firstVehicleId in TM:PE:");
        foreach (Type t in asm.GetTypes())
        {
            foreach (MethodInfo m in t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
            {
                var body = m.GetMethodBody();
                if (body != null)
                {
                    byte[] il = body.GetILAsByteArray();
                    for (int i = 0; i < il.Length - 4; i++)
                    {
                        try
                        {
                            int token = BitConverter.ToInt32(il, i);
                            FieldInfo fi = m.Module.ResolveField(token);
                            if (fi != null && fi.Name == "firstVehicleId")
                            {
                                Console.WriteLine("  " + t.Name + "." + m.Name);
                                break;
                            }
                        }
                        catch {}
                    }
                }
            }
        }
    }
}

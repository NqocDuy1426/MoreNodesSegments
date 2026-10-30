using System;
using System.Reflection;
using System.Reflection.Emit;

class Program
{
    static void Main()
    {
        Assembly asm = Assembly.LoadFile("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        Type t = asm.GetType("TrafficManager.Patch._RoadBaseAI.GetTrafficLightNodeStatePatch");
        MethodInfo m = t.GetMethod("Prefix", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        byte[] il = m.GetMethodBody().GetILAsByteArray();
        Module mod = m.Module;
        Console.WriteLine("Prefix IL bytes: " + il.Length);
        
        // Let us look at what methods/fields are referenced in tokens
        for (int i = 0; i < il.Length - 4; i++)
        {
            try
            {
                int token = BitConverter.ToInt32(il, i);
                MemberInfo member = null;
                try { member = mod.ResolveMember(token); } catch {}
                if (member != null)
                {
                    Console.WriteLine(string.Format("Offset {0:X2}: {1} ({2})", i, member.Name, member.DeclaringType));
                }
            }
            catch {}
        }
    }
}

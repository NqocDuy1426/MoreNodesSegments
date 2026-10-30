using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        Assembly asm = Assembly.LoadFrom("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/Assembly-CSharp.dll");
        Type nm = asm.GetType("NetManager");
        foreach (FieldInfo f in nm.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            if (f.Name.Contains("Name") || f.Name.Contains("segment") || f.Name.Contains("Segment"))
            {
                Console.WriteLine("Field: " + f.FieldType + " " + f.Name);
            }
        }
    }
}

using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string managed = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/";
        Assembly asm = Assembly.LoadFrom(managed + "Assembly-CSharp.dll");
        Type lmType = asm.GetType("LoadingManager");
        foreach (var m in lmType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            if (m.Name.Contains("Serializable") || m.Name.Contains("Extension"))
            {
                Console.WriteLine($"LoadingManager method: {m.Name}");
            }
        }
    }
}

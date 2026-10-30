using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string gameDir = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var assembly = AssemblyDefinition.ReadAssembly(gameDir + "/Assembly-CSharp.dll");
        foreach (var type in assembly.MainModule.Types)
        {
            if (type.FullName == "NetManager/Data" || type.FullName == "NetManager")
            {
                Console.WriteLine("Type: " + type.FullName);
                foreach (var m in type.Methods)
                {
                    if (m.Name.Contains("Deserialize") || m.Name.Contains("Awake") || m.Name.Contains("EarlyInit") || m.Name.Contains("Init"))
                    {
                        Console.WriteLine("  Method: " + m.FullName + " (IL len: " + (m.HasBody ? m.Body.Instructions.Count : 0) + ")");
                    }
                }
            }
        }
    }
}

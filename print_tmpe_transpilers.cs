using System;
using System.Linq;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string tmpePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var assembly = AssemblyDefinition.ReadAssembly(tmpePath);

        Console.WriteLine("Callers of GetModifiedBlockCounterInstructions:");
        foreach (var t in assembly.Modules.SelectMany(m => m.Types))
        {
            foreach (var m in t.Methods)
            {
                if (!m.HasBody) continue;
                foreach (var i in m.Body.Instructions)
                {
                    if (i.Operand != null && i.Operand.ToString().Contains("GetModifiedBlockCounterInstructions"))
                    {
                        Console.WriteLine(t.FullName + "::" + m.Name);
                    }
                }
            }
        }
    }
}

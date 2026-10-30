using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var type = asm.MainModule.GetType("TrafficManager.Lifecycle.SerializableDataExtension");
        foreach (var m in type.Methods)
        {
            if (m.Name == "Load")
            {
                Console.WriteLine("========================================");
                Console.WriteLine("METHOD: " + m.FullName);
                Console.WriteLine("========================================");
                if (m.Body != null)
                {
                    int i = 0;
                    foreach (var inst in m.Body.Instructions)
                    {
                        if (i++ < 50)
                            Console.WriteLine(inst);
                    }
                }
            }
        }
    }
}

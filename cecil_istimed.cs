using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string tmDll = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var readerParams = new ReaderParameters { ReadSymbols = false };
        var module = ModuleDefinition.ReadModule(tmDll, readerParams);

        TypeDefinition type = null;
        foreach (var t in module.GetTypes())
        {
            if (t.FullName == "TrafficManager.TrafficLight.Impl.TrafficLightSimulation") { type = t; break; }
        }
        foreach (var m in type.Methods)
        {
            if (m.Name == "IsTimedLight")
            {
                Console.WriteLine("\nMethod: " + m.FullName);
                foreach (var instr in m.Body.Instructions)
                {
                    Console.WriteLine(string.Format("  IL_{0:x4}: {1} {2}", instr.Offset, instr.OpCode, instr.Operand));
                }
            }
        }
    }
}

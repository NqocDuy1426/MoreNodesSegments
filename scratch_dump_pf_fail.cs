using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        string[] types = { "CarAI", "BusAI", "PassengerCarAI", "CargoTruckAI" };
        foreach (var name in types)
        {
            var t = asm.MainModule.GetType(name);
            foreach (var m in t.Methods)
            {
                if (m.Name == "PathfindFailure")
                {
                    Console.WriteLine("--- " + name + ".PathfindFailure ---");
                    foreach (var inst in m.Body.Instructions)
                    {
                        Console.WriteLine("  {0:X4}: {1} {2}", inst.Offset, inst.OpCode, inst.Operand);
                    }
                }
            }
        }
    }
}

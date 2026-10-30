using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string path = Environment.GetEnvironmentVariable("HOME") + "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var type = asm.MainModule.Types.FirstOrDefault(t => t.FullName == "Vehicle");
        var m = type.Methods.FirstOrDefault(meth => meth.Name == "Spawn");
        Console.WriteLine($"Method: {m.Name}");

        foreach (var ins in m.Body.Instructions)
        {
            Console.WriteLine($"  {ins.Offset:X4}: {ins}");
        }
    }
}

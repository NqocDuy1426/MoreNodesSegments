using System;
using System.Linq;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string gamePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll";
        var asm = AssemblyDefinition.ReadAssembly(gamePath);

        var a16 = asm.Modules.SelectMany(m => m.Types).FirstOrDefault(t => t.Name.StartsWith("Array16"));
        Console.WriteLine("Type: " + a16.FullName);
        foreach (var f in a16.Fields)
        {
            Console.WriteLine("  Field: " + f.FieldType.FullName + " " + f.Name);
        }
        foreach (var m in a16.Methods)
        {
            Console.WriteLine("  Method: " + m.ReturnType.Name + " " + m.Name + "(" + string.Join(", ", m.Parameters.Select(p => p.ParameterType.Name + " " + p.Name).ToArray()) + ")");
        }
    }
}

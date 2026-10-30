using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var t = asm.MainModule.GetType("Vehicle/Flags");
        foreach (var f in t.Fields)
        {
            if (f.IsStatic)
            {
                Console.WriteLine("  {0} = {1} (0x{1:X})", f.Name, f.Constant);
            }
        }
    }
}

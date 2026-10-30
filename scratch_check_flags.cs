using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var type = asm.MainModule.GetType("NetSegment/Flags");
        foreach (var f in type.Fields)
        {
            if (f.IsStatic && (f.Constant != null))
            {
                int val = Convert.ToInt32(f.Constant);
                if (val == 4096 || val == 8192 || val == 16384 || val == 32768 || f.Name.Contains("Vehicle") || f.Name.Contains("Pedestrian"))
                {
                    Console.WriteLine($"{f.Name} = {val} (0x{val:X})");
                }
            }
        }
    }
}

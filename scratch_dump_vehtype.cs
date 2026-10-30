using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var gameDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + 
            "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var asm = AssemblyDefinition.ReadAssembly(gameDir + "/Assembly-CSharp.dll");

        var t = asm.MainModule.GetType("VehicleInfo/VehicleType");
        foreach (var f in t.Fields)
        {
            if (f.IsStatic)
                Console.WriteLine($"  {f.Name} = {f.Constant} (0x{f.Constant:X})");
        }
    }
}

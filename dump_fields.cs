
using System;
using System.IO;
using Mono.Cecil;

class Dump
{
    static void Main()
    {
        string gameDir = Environment.GetEnvironmentVariable("HOME") + "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var asm = AssemblyDefinition.ReadAssembly(Path.Combine(gameDir, "Assembly-CSharp.dll"));
        var tLane = asm.MainModule.GetType("NetLane");
        Console.WriteLine("NetLane fields:");
        foreach (var f in tLane.Fields)
        {
            Console.WriteLine("  " + f.FieldType.Name + " " + f.Name);
        }
        var tVeh = asm.MainModule.GetType("Vehicle");
        Console.WriteLine("Vehicle fields:");
        foreach (var f in tVeh.Fields)
        {
            Console.WriteLine("  " + f.FieldType.Name + " " + f.Name);
        }
    }
}

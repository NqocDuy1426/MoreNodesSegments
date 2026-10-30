
using System;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Dump
{
    static void Main()
    {
        string gameDir = Environment.GetEnvironmentVariable("HOME") + "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var asm = AssemblyDefinition.ReadAssembly(Path.Combine(gameDir, "Assembly-CSharp.dll"));
        var t = asm.MainModule.GetType("VehicleAI");
        foreach (var method in t.Methods)
        {
            if (method.Name == "ReleaseVehicle" || method.Name == "Unspawn")
            {
                Console.WriteLine(method.Name + ":");
                foreach (var inst in method.Body.Instructions)
                {
                    Console.WriteLine("  " + inst);
                }
            }
        }
    }
}

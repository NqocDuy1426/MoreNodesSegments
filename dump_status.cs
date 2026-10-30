
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
        var t = asm.MainModule.GetType("PassengerCarAI");
        var m = t.Methods;
        foreach (var method in m)
        {
            if (method.Name == "GetLocalizedStatus")
            {
                Console.WriteLine("PassengerCarAI.GetLocalizedStatus:");
                foreach (var inst in method.Body.Instructions)
                {
                    Console.WriteLine("  " + inst);
                }
            }
        }
        var tCar = asm.MainModule.GetType("CarAI");
        foreach (var method in tCar.Methods)
        {
            if (method.Name == "GetLocalizedStatus")
            {
                Console.WriteLine("CarAI.GetLocalizedStatus:");
                foreach (var inst in method.Body.Instructions)
                {
                    Console.WriteLine("  " + inst);
                }
            }
        }
    }
}

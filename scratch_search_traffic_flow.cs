using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var gameDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + 
            "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var asm = AssemblyDefinition.ReadAssembly(gameDir + "/Assembly-CSharp.dll");

        Console.WriteLine("=== Searching NetManager ===");
        var nm = asm.MainModule.GetType("NetManager");
        foreach (var m in nm.Methods)
        {
            if (m.Name.IndexOf("traffic", StringComparison.OrdinalIgnoreCase) >= 0 ||
                m.Name.IndexOf("color", StringComparison.OrdinalIgnoreCase) >= 0 ||
                m.Name.IndexOf("flow", StringComparison.OrdinalIgnoreCase) >= 0 ||
                m.Name.IndexOf("congestion", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Console.WriteLine($"NetManager::{m.Name}");
            }
        }

        Console.WriteLine("\n=== Searching NetSegment ===");
        var ns = asm.MainModule.GetType("NetSegment");
        foreach (var m in ns.Methods)
        {
            if (m.Name.IndexOf("traffic", StringComparison.OrdinalIgnoreCase) >= 0 ||
                m.Name.IndexOf("color", StringComparison.OrdinalIgnoreCase) >= 0 ||
                m.Name.IndexOf("flow", StringComparison.OrdinalIgnoreCase) >= 0 ||
                m.Name.IndexOf("congestion", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Console.WriteLine($"NetSegment::{m.Name}");
            }
        }
    }
}

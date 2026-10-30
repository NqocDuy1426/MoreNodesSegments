using System;
using System.IO;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        foreach (var f in Directory.GetFiles(".", "*.dll"))
        {
            try
            {
                var a = AssemblyDefinition.ReadAssembly(f);
                foreach (var t in a.MainModule.Types)
                {
                    if (t.Name.StartsWith("Array16"))
                    {
                        Console.WriteLine(f + " has " + t.FullName);
                    }
                }
            }
            catch {}
        }
    }
}

using System;
using System.Reflection;
using HarmonyLib;

public class Target
{
    public void Work()
    {
        Console.WriteLine("Target.Work called");
        throw new IndexOutOfRangeException("Test IndexOutOfRange");
    }
}

public class PatchClass
{
    public static Exception Finalizer(Exception __exception)
    {
        Console.WriteLine("Finalizer executed! Exception: " + (__exception != null ? __exception.Message : "null"));
        return null; // Suppress exception
    }
}

class Program
{
    static void Main()
    {
        var h = new Harmony("test.h");
        var orig = typeof(Target).GetMethod("Work");
        var fin = typeof(PatchClass).GetMethod("Finalizer");
        h.Patch(orig, finalizer: new HarmonyMethod(fin));

        var t = new Target();
        try
        {
            t.Work();
            Console.WriteLine("SUCCESS! Exception suppressed by Harmony 2 Finalizer!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("FAILED! Exception NOT suppressed: " + ex);
        }
    }
}

using System;
using System.Reflection;
using HarmonyLib;

public class Target
{
    public void Work(string arg1, int arg2)
    {
        Console.WriteLine("Target.Work called with " + arg1 + ", " + arg2);
        throw new IndexOutOfRangeException("Test IndexOutOfRange");
    }
}

public class PatchClass
{
    public static Exception Finalizer(Exception __exception, object __instance, string arg1, int arg2)
    {
        Console.WriteLine("Finalizer executed! Exception: " + (__exception != null ? __exception.Message : "null") + ", arg1: " + arg1 + ", arg2: " + arg2);
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
            t.Work("hello", 42);
            Console.WriteLine("SUCCESS! Exception suppressed and arguments delivered to Finalizer!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("FAILED! " + ex);
        }
    }
}

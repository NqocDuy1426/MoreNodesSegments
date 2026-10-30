using System;
using System.Reflection;
using HarmonyLib;
using ColossalFramework.IO;

public class TestTarget
{
    public void TestMethod(DataSerializer s)
    {
        throw new IndexOutOfRangeException("Test exception");
    }
}

public class TestPatch
{
    public static Exception Finalizer(Exception __exception, object __instance, DataSerializer s)
    {
        Console.WriteLine("Finalizer called with exception: " + __exception.Message + ", s is null? " + (s == null));
        return null;
    }
}

class Program
{
    static void Main()
    {
        var harmony = new Harmony("test.finalizer");
        var original = typeof(TestTarget).GetMethod("TestMethod");
        var finalizer = typeof(TestPatch).GetMethod("Finalizer");
        harmony.Patch(original, finalizer: new HarmonyMethod(finalizer));

        var target = new TestTarget();
        try
        {
            target.TestMethod(null);
            Console.WriteLine("Success: Exception was suppressed!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed: Exception was NOT suppressed: " + ex);
        }
    }
}

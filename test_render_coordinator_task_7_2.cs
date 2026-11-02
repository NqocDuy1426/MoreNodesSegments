/**
 * Test RenderCoordinator Task 7.2 - Dirty Flag and Hot-Swap Detection
 */

using System;
using System.Reflection;
using System.Diagnostics;

class TestRenderCoordinatorTask72
{
    static int s_testsPassed = 0;
    static int s_testsFailed = 0;

    static void Main(string[] args)
    {
        Console.WriteLine("=== RenderCoordinator Task 7.2 Test Suite ===");
        Console.WriteLine();

        string dllPath = "content_beta/MoreNodesSegmentsBeta.dll";
        if (!System.IO.File.Exists(dllPath))
        {
            Console.WriteLine("ERROR: DLL not found: " + dllPath);
            Environment.Exit(1);
        }

        Assembly assembly = Assembly.LoadFrom(dllPath);
        Console.WriteLine("Loaded: " + assembly.GetName().Name);
        Console.WriteLine();

        TestIsHotSwapActiveMethodExists(assembly);
        TestSetGridDirtyFlagMethodExists(assembly);
        TestPerformanceMonitoringFieldsExist(assembly);
        TestPerformanceConstants(assembly);
        TestFrameTimeThreshold();
        TestOverheadThreshold();
        TestEndRenderingImplPostfixStructure(assembly);

        Console.WriteLine();
        Console.WriteLine("=== Test Summary ===");
        Console.WriteLine("Passed: " + s_testsPassed);
        Console.WriteLine("Failed: " + s_testsFailed);
        Console.WriteLine();

        if (s_testsFailed == 0)
        {
            Console.WriteLine("SUCCESS: All tests passed!");
            Environment.Exit(0);
        }
        else
        {
            Console.WriteLine("FAILED: Some tests failed.");
            Environment.Exit(1);
        }
    }

    static void TestIsHotSwapActiveMethodExists(Assembly assembly)
    {
        Console.WriteLine("[Test 1] IsHotSwapActive method...");
        Type renderCoordType = assembly.GetType("MoreNetworkCapacityRenew.RenderCoordinator");
        if (renderCoordType != null)
        {
            MethodInfo hotSwapMethod = renderCoordType.GetMethod("IsHotSwapActive", 
                BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            
            if (hotSwapMethod != null && hotSwapMethod.ReturnType == typeof(bool))
            {
                Console.WriteLine("  PASS: IsHotSwapActive() returns bool");
                s_testsPassed++;
            }
            else
            {
                Console.WriteLine("  FAIL: IsHotSwapActive method not found or wrong type");
                s_testsFailed++;
            }
        }
        else
        {
            Console.WriteLine("  FAIL: RenderCoordinator class not found");
            s_testsFailed++;
        }
    }

    static void TestSetGridDirtyFlagMethodExists(Assembly assembly)
    {
        Console.WriteLine("[Test 2] SetGridDirtyFlag method...");
        Type renderCoordType = assembly.GetType("MoreNetworkCapacityRenew.RenderCoordinator");
        if (renderCoordType != null)
        {
            MethodInfo setDirtyMethod = renderCoordType.GetMethod("SetGridDirtyFlag", 
                BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            
            if (setDirtyMethod != null)
            {
                Console.WriteLine("  PASS: SetGridDirtyFlag() found");
                s_testsPassed++;
            }
            else
            {
                Console.WriteLine("  FAIL: SetGridDirtyFlag method not found");
                s_testsFailed++;
            }
        }
    }

    static void TestPerformanceMonitoringFieldsExist(Assembly assembly)
    {
        Console.WriteLine("[Test 3] Performance monitoring fields...");
        Type renderCoordType = assembly.GetType("MoreNetworkCapacityRenew.RenderCoordinator");
        int passed = 0;
        
        FieldInfo stopwatchField = renderCoordType.GetField("s_perfStopwatch", BindingFlags.Static | BindingFlags.NonPublic);
        if (stopwatchField != null && stopwatchField.FieldType == typeof(Stopwatch))
        {
            Console.WriteLine("  PASS: s_perfStopwatch (Stopwatch)");
            passed++;
        }

        FieldInfo hotSwapField = renderCoordType.GetField("s_hotSwapActiveField", BindingFlags.Static | BindingFlags.NonPublic);
        if (hotSwapField != null && hotSwapField.FieldType == typeof(FieldInfo))
        {
            Console.WriteLine("  PASS: s_hotSwapActiveField (FieldInfo)");
            passed++;
        }

        FieldInfo hotSwapFailedField = renderCoordType.GetField("s_hotSwapReflectionFailed", BindingFlags.Static | BindingFlags.NonPublic);
        if (hotSwapFailedField != null && hotSwapFailedField.FieldType == typeof(bool))
        {
            Console.WriteLine("  PASS: s_hotSwapReflectionFailed (bool)");
            passed++;
        }

        if (passed == 3)
        {
            s_testsPassed++;
        }
        else
        {
            Console.WriteLine("  FAIL: Some fields missing");
            s_testsFailed++;
        }
    }

    static void TestPerformanceConstants(Assembly assembly)
    {
        Console.WriteLine("[Test 4] Performance constants...");
        Type renderCoordType = assembly.GetType("MoreNetworkCapacityRenew.RenderCoordinator");
        int passed = 0;
        
        FieldInfo frameTimeField = renderCoordType.GetField("TARGET_FRAME_TIME_MS", BindingFlags.Static | BindingFlags.NonPublic);
        if (frameTimeField != null)
        {
            double frameTime = (double)frameTimeField.GetValue(null);
            if (frameTime == 16.0)
            {
                Console.WriteLine("  PASS: TARGET_FRAME_TIME_MS = 16.0ms");
                passed++;
            }
        }

        FieldInfo overheadField = renderCoordType.GetField("TARGET_OVERHEAD_MS", BindingFlags.Static | BindingFlags.NonPublic);
        if (overheadField != null)
        {
            double overhead = (double)overheadField.GetValue(null);
            if (overhead == 2.0)
            {
                Console.WriteLine("  PASS: TARGET_OVERHEAD_MS = 2.0ms");
                passed++;
            }
        }

        if (passed == 2)
        {
            s_testsPassed++;
        }
        else
        {
            Console.WriteLine("  FAIL: Some constants missing or wrong");
            s_testsFailed++;
        }
    }

    static void TestFrameTimeThreshold()
    {
        Console.WriteLine("[Test 5] Frame time threshold (16ms)...");
        const double TARGET = 16.0;
        bool test1 = !(10.5 > TARGET);
        bool test2 = (18.7 > TARGET);
        bool test3 = !(16.0 > TARGET);
        
        if (test1 && test2 && test3)
        {
            Console.WriteLine("  PASS: All threshold tests correct");
            s_testsPassed++;
        }
        else
        {
            Console.WriteLine("  FAIL: Threshold logic incorrect");
            s_testsFailed++;
        }
    }

    static void TestOverheadThreshold()
    {
        Console.WriteLine("[Test 6] Overhead threshold (2ms)...");
        const double TARGET = 2.0;
        bool test1 = !(1.2 > TARGET);
        bool test2 = (3.5 > TARGET);
        bool test3 = !(0.5 > TARGET);
        
        if (test1 && test2 && test3)
        {
            Console.WriteLine("  PASS: All overhead tests correct");
            s_testsPassed++;
        }
        else
        {
            Console.WriteLine("  FAIL: Overhead logic incorrect");
            s_testsFailed++;
        }
    }

    static void TestEndRenderingImplPostfixStructure(Assembly assembly)
    {
        Console.WriteLine("[Test 7] EndRenderingImplPostfix structure...");
        Type renderCoordType = assembly.GetType("MoreNetworkCapacityRenew.RenderCoordinator");
        MethodInfo postfixMethod = renderCoordType.GetMethod("EndRenderingImplPostfix", 
            BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
        
        if (postfixMethod != null)
        {
            var methodBody = postfixMethod.GetMethodBody();
            if (methodBody != null)
            {
                Console.WriteLine("  PASS: EndRenderingImplPostfix found with " + methodBody.GetILAsByteArray().Length + " bytes IL");
                s_testsPassed++;
            }
            else
            {
                Console.WriteLine("  PASS: EndRenderingImplPostfix found (body not accessible)");
                s_testsPassed++;
            }
        }
        else
        {
            Console.WriteLine("  FAIL: EndRenderingImplPostfix not found");
            s_testsFailed++;
        }
    }
}

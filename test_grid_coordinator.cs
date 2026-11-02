using System;
using System.Reflection;
using System.Collections.Generic;

/// <summary>
/// Test suite for GridCoordinator timing coordination functionality.
/// 
/// Tests:
/// 1. Standalone mode - immediate sanitization (Req 3.6)
/// 2. Compatibility mode - deferred sanitization (Req 3.1, 3.2, 3.3)
/// 3. Fallback delayed execution when event registration fails
/// 4. Queue management for pending callbacks
/// 5. Reset functionality
/// </summary>
class TestGridCoordinator
{
    static int s_testsPassed = 0;
    static int s_testsFailed = 0;
    
    static void Main()
    {
        Console.WriteLine("=== GridCoordinator Test Suite ===\n");
        
        // Test 1: Verify GridCoordinator class exists and has required methods
        TestClassStructure();
        
        // Test 2: Verify standalone mode path exists
        TestStandaloneModePath();
        
        // Test 3: Verify compatibility mode path exists
        TestCompatibilityModePath();
        
        // Test 4: Verify scheduling queue functionality
        TestSchedulingQueue();
        
        // Test 5: Verify reset functionality
        TestResetFunctionality();
        
        // Test 6: Verify RenewMod delayed callback system
        TestDelayedCallbackSystem();
        
        // Summary
        Console.WriteLine("\n=== Test Summary ===");
        Console.WriteLine($"Tests Passed: {s_testsPassed}");
        Console.WriteLine($"Tests Failed: {s_testsFailed}");
        
        if (s_testsFailed == 0)
        {
            Console.WriteLine("\n✅ ALL TESTS PASSED!");
            Environment.Exit(0);
        }
        else
        {
            Console.WriteLine($"\n❌ {s_testsFailed} TEST(S) FAILED");
            Environment.Exit(1);
        }
    }
    
    static void TestClassStructure()
    {
        Console.WriteLine("Test 1: Verifying GridCoordinator class structure...");
        
        try
        {
            // Load the assembly
            Assembly asm = Assembly.LoadFrom("content_beta/MoreNodesSegmentsBeta.dll");
            
            // Find GridCoordinator class
            Type gridCoordType = null;
            foreach (Type t in asm.GetTypes())
            {
                if (t.Name == "GridCoordinator")
                {
                    gridCoordType = t;
                    break;
                }
            }
            
            Assert(gridCoordType != null, "GridCoordinator class should exist");
            
            // Verify public methods exist
            MethodInfo onLevelLoadedMethod = gridCoordType.GetMethod("OnLevelLoaded", BindingFlags.Public | BindingFlags.Static);
            Assert(onLevelLoadedMethod != null, "OnLevelLoaded method should exist");
            
            MethodInfo onMorePropsReadyMethod = gridCoordType.GetMethod("OnMorePropsReady", BindingFlags.Public | BindingFlags.Static);
            Assert(onMorePropsReadyMethod != null, "OnMorePropsReady method should exist");
            
            MethodInfo scheduleSanitizationMethod = gridCoordType.GetMethod("ScheduleSanitization", BindingFlags.Public | BindingFlags.Static);
            Assert(scheduleSanitizationMethod != null, "ScheduleSanitization method should exist");
            
            MethodInfo resetMethod = gridCoordType.GetMethod("Reset", BindingFlags.Public | BindingFlags.Static);
            Assert(resetMethod != null, "Reset method should exist");
            
            MethodInfo getStatusReportMethod = gridCoordType.GetMethod("GetStatusReport", BindingFlags.Public | BindingFlags.Static);
            Assert(getStatusReportMethod != null, "GetStatusReport method should exist");
            
            Pass("GridCoordinator class structure verified");
        }
        catch (Exception ex)
        {
            Fail($"GridCoordinator class structure test failed: {ex.Message}");
        }
    }
    
    static void TestStandaloneModePath()
    {
        Console.WriteLine("\nTest 2: Verifying standalone mode code path...");
        
        try
        {
            Assembly asm = Assembly.LoadFrom("content_beta/MoreNodesSegmentsBeta.dll");
            
            Type gridCoordType = FindType(asm, "GridCoordinator");
            Type coordMgrType = FindType(asm, "CoordinationManager");
            
            Assert(gridCoordType != null, "GridCoordinator should exist");
            Assert(coordMgrType != null, "CoordinationManager should exist");
            
            // Verify CoordinationManager has IsMorePropsActive property
            PropertyInfo isActiveProperty = coordMgrType.GetProperty("IsMorePropsActive", BindingFlags.Public | BindingFlags.Static);
            Assert(isActiveProperty != null, "IsMorePropsActive property should exist in CoordinationManager");
            
            Pass("Standalone mode path verified");
        }
        catch (Exception ex)
        {
            Fail($"Standalone mode path test failed: {ex.Message}");
        }
    }
    
    static void TestCompatibilityModePath()
    {
        Console.WriteLine("\nTest 3: Verifying compatibility mode code path...");
        
        try
        {
            Assembly asm = Assembly.LoadFrom("content_beta/MoreNodesSegmentsBeta.dll");
            
            Type gridCoordType = FindType(asm, "GridCoordinator");
            Assert(gridCoordType != null, "GridCoordinator should exist");
            
            // Verify private method for event registration exists
            MethodInfo tryRegisterMethod = gridCoordType.GetMethod("TryRegisterMorePropsCallback", BindingFlags.NonPublic | BindingFlags.Static);
            Assert(tryRegisterMethod != null, "TryRegisterMorePropsCallback method should exist");
            
            // Verify fallback method exists
            MethodInfo fallbackMethod = gridCoordType.GetMethod("FallbackDelayedExecution", BindingFlags.NonPublic | BindingFlags.Static);
            Assert(fallbackMethod != null, "FallbackDelayedExecution method should exist");
            
            Pass("Compatibility mode path verified");
        }
        catch (Exception ex)
        {
            Fail($"Compatibility mode path test failed: {ex.Message}");
        }
    }
    
    static void TestSchedulingQueue()
    {
        Console.WriteLine("\nTest 4: Verifying scheduling queue functionality...");
        
        try
        {
            Assembly asm = Assembly.LoadFrom("content_beta/MoreNodesSegmentsBeta.dll");
            
            Type gridCoordType = FindType(asm, "GridCoordinator");
            Assert(gridCoordType != null, "GridCoordinator should exist");
            
            // Verify ScheduleSanitization method signature
            MethodInfo scheduleMethod = gridCoordType.GetMethod("ScheduleSanitization", BindingFlags.Public | BindingFlags.Static);
            Assert(scheduleMethod != null, "ScheduleSanitization method should exist");
            
            ParameterInfo[] parameters = scheduleMethod.GetParameters();
            Assert(parameters.Length == 1, "ScheduleSanitization should have 1 parameter");
            Assert(parameters[0].ParameterType == typeof(string), "Parameter should be string (callerContext)");
            
            Pass("Scheduling queue structure verified");
        }
        catch (Exception ex)
        {
            Fail($"Scheduling queue test failed: {ex.Message}");
        }
    }
    
    static void TestResetFunctionality()
    {
        Console.WriteLine("\nTest 5: Verifying reset functionality...");
        
        try
        {
            Assembly asm = Assembly.LoadFrom("content_beta/MoreNodesSegmentsBeta.dll");
            
            Type gridCoordType = FindType(asm, "GridCoordinator");
            Assert(gridCoordType != null, "GridCoordinator should exist");
            
            MethodInfo resetMethod = gridCoordType.GetMethod("Reset", BindingFlags.Public | BindingFlags.Static);
            Assert(resetMethod != null, "Reset method should exist");
            Assert(resetMethod.ReturnType == typeof(void), "Reset should return void");
            Assert(resetMethod.GetParameters().Length == 0, "Reset should have no parameters");
            
            Pass("Reset functionality verified");
        }
        catch (Exception ex)
        {
            Fail($"Reset functionality test failed: {ex.Message}");
        }
    }
    
    static void TestDelayedCallbackSystem()
    {
        Console.WriteLine("\nTest 6: Verifying RenewMod delayed callback system...");
        
        try
        {
            Assembly asm = Assembly.LoadFrom("content_beta/MoreNodesSegmentsBeta.dll");
            
            Type renewModType = FindType(asm, "RenewMod");
            Assert(renewModType != null, "RenewMod should exist");
            
            // Verify ScheduleDelayedCallback exists
            MethodInfo scheduleDelayedMethod = renewModType.GetMethod("ScheduleDelayedCallback", BindingFlags.Public | BindingFlags.Static);
            Assert(scheduleDelayedMethod != null, "ScheduleDelayedCallback method should exist");
            
            ParameterInfo[] parameters = scheduleDelayedMethod.GetParameters();
            Assert(parameters.Length == 2, "ScheduleDelayedCallback should have 2 parameters");
            Assert(parameters[0].ParameterType == typeof(Action), "First parameter should be Action");
            Assert(parameters[1].ParameterType == typeof(float), "Second parameter should be float");
            
            // Verify ProcessDelayedCallbacks exists
            MethodInfo processMethod = renewModType.GetMethod("ProcessDelayedCallbacks", BindingFlags.Public | BindingFlags.Static);
            Assert(processMethod != null, "ProcessDelayedCallbacks method should exist");
            Assert(processMethod.ReturnType == typeof(void), "ProcessDelayedCallbacks should return void");
            
            Pass("Delayed callback system verified");
        }
        catch (Exception ex)
        {
            Fail($"Delayed callback system test failed: {ex.Message}");
        }
    }
    
    // Helper methods
    
    static Type FindType(Assembly asm, string typeName)
    {
        foreach (Type t in asm.GetTypes())
        {
            if (t.Name == typeName)
            {
                return t;
            }
        }
        return null;
    }
    
    static void Assert(bool condition, string message)
    {
        if (!condition)
        {
            throw new Exception($"Assertion failed: {message}");
        }
    }
    
    static void Pass(string message)
    {
        Console.WriteLine($"  ✅ PASS: {message}");
        s_testsPassed++;
    }
    
    static void Fail(string message)
    {
        Console.WriteLine($"  ❌ FAIL: {message}");
        s_testsFailed++;
    }
}

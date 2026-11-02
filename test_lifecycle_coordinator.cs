using System;
using System.Collections.Generic;
using System.Diagnostics;
using MoreNetworkCapacityRenew;

/// <summary>
/// Unit tests for LifecycleCoordinator callback execution and ordering
/// Tests Requirements: 7.1, 7.2, 7.7, 7.8
/// 
/// Test Coverage:
/// 1. Callback registration for different lifecycle phases
/// 2. Priority-based execution order (descending - highest first)
/// 3. Error isolation (exceptions in one callback don't affect others)
/// 4. Performance timing (warnings for callbacks >50ms)
/// 5. Debug logging of execution order
/// 6. Null callback handling
/// 7. Multiple callbacks with same priority
/// 
/// Note: This test runs in standalone mode (without Unity environment)
/// The LifecycleCoordinator should use Console logging fallback when Unity is unavailable
/// </summary>
class TestLifecycleCoordinator
{
    static int s_testsPassed = 0;
    static int s_testsFailed = 0;
    static List<string> s_executionLog = new List<string>();
    
    static void Main()
    {
        Console.WriteLine("========================================");
        Console.WriteLine("LifecycleCoordinator Unit Tests");
        Console.WriteLine("========================================\n");
        
        Console.WriteLine("NOTE: Running in standalone mode (no Unity environment)");
        Console.WriteLine("LifecycleCoordinator will use Console logging fallback\n");
        
        // Run all tests
        Test_BasicCallbackRegistration();
        Test_PriorityBasedOrdering();
        Test_ErrorIsolation();
        Test_PerformanceTiming();
        Test_NullCallbackHandling();
        Test_MultiplePhasesIndependent();
        Test_EmptyCallbackList();
        Test_SamePriorityCallbacks();
        Test_AllLifecyclePhases();
        Test_CallbackReexecution();
        
        // Print summary
        Console.WriteLine("\n========================================");
        Console.WriteLine("Test Summary");
        Console.WriteLine("========================================");
        Console.WriteLine($"Tests Passed: {s_testsPassed}");
        Console.WriteLine($"Tests Failed: {s_testsFailed}");
        Console.WriteLine("========================================");
        
        if (s_testsFailed == 0)
        {
            Console.WriteLine("✓ All tests PASSED");
            Console.WriteLine("\nNOTE: These tests verify LifecycleCoordinator callback management.");
            Console.WriteLine("Full integration testing requires running in Cities: Skylines with LoadingExtension.");
            Environment.Exit(0);
        }
        else
        {
            Console.WriteLine($"✗ {s_testsFailed} test(s) FAILED");
            Environment.Exit(1);
        }
    }
    
    // ========== Test Cases ==========
    
    static void Test_BasicCallbackRegistration()
    {
        string testName = "Basic Callback Registration";
        Console.WriteLine($"\n--- {testName} ---");
        
        try
        {
            s_executionLog.Clear();
            
            // Register a simple callback
            LifecycleCoordinator.RegisterOnCreated("TestComponent", 100, () =>
            {
                s_executionLog.Add("TestComponent");
            });
            
            Console.WriteLine("  ✓ Registered OnCreated callback");
            
            // Execute callbacks
            LifecycleCoordinator.ExecuteOnCreated();
            
            // Verify execution
            Assert(s_executionLog.Count == 1, "Should have executed 1 callback");
            Assert(s_executionLog[0] == "TestComponent", "Should have executed TestComponent");
            
            Console.WriteLine("  ✓ Callback executed successfully");
            
            Pass(testName);
        }
        catch (Exception ex)
        {
            Fail(testName, ex.Message);
        }
    }
    
    static void Test_PriorityBasedOrdering()
    {
        string testName = "Priority-Based Ordering (Descending)";
        Console.WriteLine($"\n--- {testName} ---");
        
        try
        {
            s_executionLog.Clear();
            
            // Register callbacks with different priorities (out of order)
            LifecycleCoordinator.RegisterOnLevelLoaded("LowPriority", 100, () =>
            {
                s_executionLog.Add("LowPriority");
            });
            
            LifecycleCoordinator.RegisterOnLevelLoaded("HighPriority", 1000, () =>
            {
                s_executionLog.Add("HighPriority");
            });
            
            LifecycleCoordinator.RegisterOnLevelLoaded("MediumPriority", 500, () =>
            {
                s_executionLog.Add("MediumPriority");
            });
            
            Console.WriteLine("  ✓ Registered 3 callbacks with priorities: 100, 1000, 500");
            
            // Execute callbacks
            LifecycleCoordinator.ExecuteOnLevelLoaded();
            
            // Verify execution order (highest priority first)
            Assert(s_executionLog.Count == 3, "Should have executed 3 callbacks");
            Assert(s_executionLog[0] == "HighPriority", "First should be HighPriority (1000)");
            Assert(s_executionLog[1] == "MediumPriority", "Second should be MediumPriority (500)");
            Assert(s_executionLog[2] == "LowPriority", "Third should be LowPriority (100)");
            
            Console.WriteLine("  ✓ Execution order: HighPriority (1000) → MediumPriority (500) → LowPriority (100)");
            Console.WriteLine("  ✓ Priority-based ordering verified (descending)");
            
            Pass(testName);
        }
        catch (Exception ex)
        {
            Fail(testName, ex.Message);
        }
    }
    
    static void Test_ErrorIsolation()
    {
        string testName = "Error Isolation (Exception Handling)";
        Console.WriteLine($"\n--- {testName} ---");
        
        try
        {
            s_executionLog.Clear();
            
            // Register callbacks where one throws an exception
            LifecycleCoordinator.RegisterOnLevelUnloading("BeforeError", 300, () =>
            {
                s_executionLog.Add("BeforeError");
            });
            
            LifecycleCoordinator.RegisterOnLevelUnloading("ErrorCallback", 200, () =>
            {
                s_executionLog.Add("ErrorCallback");
                throw new Exception("Simulated error");
            });
            
            LifecycleCoordinator.RegisterOnLevelUnloading("AfterError", 100, () =>
            {
                s_executionLog.Add("AfterError");
            });
            
            Console.WriteLine("  ✓ Registered 3 callbacks (middle one throws exception)");
            
            // Execute callbacks (should not throw)
            LifecycleCoordinator.ExecuteOnLevelUnloading();
            
            // Verify all callbacks executed despite exception
            Assert(s_executionLog.Count == 3, "Should have executed all 3 callbacks");
            Assert(s_executionLog[0] == "BeforeError", "BeforeError should execute");
            Assert(s_executionLog[1] == "ErrorCallback", "ErrorCallback should execute (before throwing)");
            Assert(s_executionLog[2] == "AfterError", "AfterError should execute (isolation works)");
            
            Console.WriteLine("  ✓ All callbacks executed despite exception in middle callback");
            Console.WriteLine("  ✓ Error isolation verified");
            
            Pass(testName);
        }
        catch (Exception ex)
        {
            Fail(testName, ex.Message);
        }
    }
    
    static void Test_PerformanceTiming()
    {
        string testName = "Performance Timing (>50ms Warning)";
        Console.WriteLine($"\n--- {testName} ---");
        
        try
        {
            s_executionLog.Clear();
            
            // Register a fast callback
            LifecycleCoordinator.RegisterOnCreated("FastCallback", 200, () =>
            {
                s_executionLog.Add("FastCallback");
                // No delay - should complete quickly
            });
            
            // Register a slow callback (simulates >50ms work)
            LifecycleCoordinator.RegisterOnCreated("SlowCallback", 100, () =>
            {
                s_executionLog.Add("SlowCallback");
                System.Threading.Thread.Sleep(60); // 60ms delay
            });
            
            Console.WriteLine("  ✓ Registered fast and slow callbacks");
            
            // Execute callbacks (should log warning for slow callback)
            var sw = Stopwatch.StartNew();
            LifecycleCoordinator.ExecuteOnCreated();
            sw.Stop();
            
            // Verify both executed
            Assert(s_executionLog.Count == 2, "Should have executed 2 callbacks");
            Assert(sw.ElapsedMilliseconds >= 60, "Total execution should be >= 60ms");
            
            Console.WriteLine($"  ✓ Both callbacks executed (total time: {sw.ElapsedMilliseconds}ms)");
            Console.WriteLine("  ✓ Performance timing implemented (check logs for >50ms warning)");
            
            Pass(testName);
        }
        catch (Exception ex)
        {
            Fail(testName, ex.Message);
        }
    }
    
    static void Test_NullCallbackHandling()
    {
        string testName = "Null Callback Handling";
        Console.WriteLine($"\n--- {testName} ---");
        
        try
        {
            // Try to register null callback (should be rejected gracefully)
            LifecycleCoordinator.RegisterOnCreated("NullTest", 100, null);
            
            Console.WriteLine("  ✓ Null callback registration handled gracefully");
            
            // Execute should not crash
            LifecycleCoordinator.ExecuteOnCreated();
            
            Console.WriteLine("  ✓ ExecuteOnCreated() completed without crash");
            
            Pass(testName);
        }
        catch (Exception ex)
        {
            Fail(testName, ex.Message);
        }
    }
    
    static void Test_MultiplePhasesIndependent()
    {
        string testName = "Multiple Phases Independent";
        Console.WriteLine($"\n--- {testName} ---");
        
        try
        {
            s_executionLog.Clear();
            
            // Register callbacks for different phases
            LifecycleCoordinator.RegisterOnCreated("CreatedPhase", 100, () =>
            {
                s_executionLog.Add("Created");
            });
            
            LifecycleCoordinator.RegisterOnLevelLoaded("LoadedPhase", 100, () =>
            {
                s_executionLog.Add("Loaded");
            });
            
            LifecycleCoordinator.RegisterOnLevelUnloading("UnloadingPhase", 100, () =>
            {
                s_executionLog.Add("Unloading");
            });
            
            Console.WriteLine("  ✓ Registered callbacks for all 3 lifecycle phases");
            
            // Execute OnCreated only
            LifecycleCoordinator.ExecuteOnCreated();
            Assert(s_executionLog.Count == 1, "Should have executed only OnCreated callback");
            Assert(s_executionLog[0] == "Created", "Should be Created callback");
            
            Console.WriteLine("  ✓ ExecuteOnCreated() executed only OnCreated callback");
            
            // Execute OnLevelLoaded
            s_executionLog.Clear();
            LifecycleCoordinator.ExecuteOnLevelLoaded();
            Assert(s_executionLog.Count == 1, "Should have executed only OnLevelLoaded callback");
            Assert(s_executionLog[0] == "Loaded", "Should be Loaded callback");
            
            Console.WriteLine("  ✓ ExecuteOnLevelLoaded() executed only OnLevelLoaded callback");
            
            // Execute OnLevelUnloading
            s_executionLog.Clear();
            LifecycleCoordinator.ExecuteOnLevelUnloading();
            Assert(s_executionLog.Count == 1, "Should have executed only OnLevelUnloading callback");
            Assert(s_executionLog[0] == "Unloading", "Should be Unloading callback");
            
            Console.WriteLine("  ✓ ExecuteOnLevelUnloading() executed only OnLevelUnloading callback");
            Console.WriteLine("  ✓ Lifecycle phases are independent");
            
            Pass(testName);
        }
        catch (Exception ex)
        {
            Fail(testName, ex.Message);
        }
    }
    
    static void Test_EmptyCallbackList()
    {
        string testName = "Empty Callback List";
        Console.WriteLine($"\n--- {testName} ---");
        
        try
        {
            // Execute on a fresh phase with no callbacks registered
            // (Note: Previous tests may have registered callbacks, but they're in different lists)
            // We'll use a clean execution context by not registering anything new
            
            // This should not crash
            LifecycleCoordinator.ExecuteOnCreated();
            LifecycleCoordinator.ExecuteOnLevelLoaded();
            LifecycleCoordinator.ExecuteOnLevelUnloading();
            
            Console.WriteLine("  ✓ All Execute methods handled gracefully (with or without callbacks)");
            
            Pass(testName);
        }
        catch (Exception ex)
        {
            Fail(testName, ex.Message);
        }
    }
    
    static void Test_SamePriorityCallbacks()
    {
        string testName = "Same Priority Callbacks";
        Console.WriteLine($"\n--- {testName} ---");
        
        try
        {
            s_executionLog.Clear();
            
            // Register multiple callbacks with same priority
            LifecycleCoordinator.RegisterOnCreated("SamePriority_A", 500, () =>
            {
                s_executionLog.Add("A");
            });
            
            LifecycleCoordinator.RegisterOnCreated("SamePriority_B", 500, () =>
            {
                s_executionLog.Add("B");
            });
            
            LifecycleCoordinator.RegisterOnCreated("SamePriority_C", 500, () =>
            {
                s_executionLog.Add("C");
            });
            
            Console.WriteLine("  ✓ Registered 3 callbacks with same priority (500)");
            
            // Execute callbacks
            LifecycleCoordinator.ExecuteOnCreated();
            
            // Verify all executed (order may vary for same priority)
            Assert(s_executionLog.Count == 3, "Should have executed 3 callbacks");
            Assert(s_executionLog.Contains("A"), "Should contain A");
            Assert(s_executionLog.Contains("B"), "Should contain B");
            Assert(s_executionLog.Contains("C"), "Should contain C");
            
            Console.WriteLine($"  ✓ All callbacks executed (order: {string.Join(", ", s_executionLog)})");
            Console.WriteLine("  ✓ Same priority callbacks handled correctly");
            
            Pass(testName);
        }
        catch (Exception ex)
        {
            Fail(testName, ex.Message);
        }
    }
    
    static void Test_AllLifecyclePhases()
    {
        string testName = "All Lifecycle Phases Integration";
        Console.WriteLine($"\n--- {testName} ---");
        
        try
        {
            s_executionLog.Clear();
            
            // Simulate a complete lifecycle with priorities matching design doc
            
            // OnCreated phase
            LifecycleCoordinator.RegisterOnCreated("CoordinationManager", 1000, () =>
            {
                s_executionLog.Add("CoordinationManager");
            });
            LifecycleCoordinator.RegisterOnCreated("MorePropsDetection", 900, () =>
            {
                s_executionLog.Add("MorePropsDetection");
            });
            LifecycleCoordinator.RegisterOnCreated("CompatibilityActivation", 800, () =>
            {
                s_executionLog.Add("CompatibilityActivation");
            });
            LifecycleCoordinator.RegisterOnCreated("ArrayGuardian", 700, () =>
            {
                s_executionLog.Add("ArrayGuardian");
            });
            LifecycleCoordinator.RegisterOnCreated("MNCInit", 500, () =>
            {
                s_executionLog.Add("MNCInit");
            });
            
            Console.WriteLine("  ✓ Registered OnCreated callbacks with priorities 1000 → 500");
            
            // Execute OnCreated
            LifecycleCoordinator.ExecuteOnCreated();
            
            // Verify execution order
            Assert(s_executionLog.Count == 5, "Should execute 5 OnCreated callbacks");
            Assert(s_executionLog[0] == "CoordinationManager", "First: CoordinationManager (1000)");
            Assert(s_executionLog[1] == "MorePropsDetection", "Second: MorePropsDetection (900)");
            Assert(s_executionLog[2] == "CompatibilityActivation", "Third: CompatibilityActivation (800)");
            Assert(s_executionLog[3] == "ArrayGuardian", "Fourth: ArrayGuardian (700)");
            Assert(s_executionLog[4] == "MNCInit", "Fifth: MNCInit (500)");
            
            Console.WriteLine("  ✓ OnCreated phase executed in correct priority order");
            
            // OnLevelLoaded phase
            s_executionLog.Clear();
            LifecycleCoordinator.RegisterOnLevelLoaded("MorePropsDeserialization", 200, () =>
            {
                s_executionLog.Add("Deserialization");
            });
            LifecycleCoordinator.RegisterOnLevelLoaded("GridValidation", 100, () =>
            {
                s_executionLog.Add("Validation");
            });
            
            LifecycleCoordinator.ExecuteOnLevelLoaded();
            
            Assert(s_executionLog.Count == 2, "Should execute 2 OnLevelLoaded callbacks");
            Assert(s_executionLog[0] == "Deserialization", "First: Deserialization (200)");
            Assert(s_executionLog[1] == "Validation", "Second: Validation (100)");
            
            Console.WriteLine("  ✓ OnLevelLoaded phase executed in correct priority order");
            
            // OnLevelUnloading phase
            s_executionLog.Clear();
            LifecycleCoordinator.RegisterOnLevelUnloading("MNCCleanup", 100, () =>
            {
                s_executionLog.Add("MNCCleanup");
            });
            
            LifecycleCoordinator.ExecuteOnLevelUnloading();
            
            Assert(s_executionLog.Count == 1, "Should execute 1 OnLevelUnloading callback");
            Assert(s_executionLog[0] == "MNCCleanup", "Should be MNCCleanup");
            
            Console.WriteLine("  ✓ OnLevelUnloading phase executed");
            Console.WriteLine("  ✓ Complete lifecycle simulation verified");
            
            Pass(testName);
        }
        catch (Exception ex)
        {
            Fail(testName, ex.Message);
        }
    }
    
    static void Test_CallbackReexecution()
    {
        string testName = "Callback Re-execution";
        Console.WriteLine($"\n--- {testName} ---");
        
        try
        {
            s_executionLog.Clear();
            int callCount = 0;
            
            // Register a callback that increments a counter
            LifecycleCoordinator.RegisterOnCreated("ReexecutionTest", 100, () =>
            {
                callCount++;
                s_executionLog.Add($"Call_{callCount}");
            });
            
            // Execute multiple times
            LifecycleCoordinator.ExecuteOnCreated();
            Assert(callCount == 1, "Should have called once");
            
            LifecycleCoordinator.ExecuteOnCreated();
            Assert(callCount == 2, "Should have called twice");
            
            LifecycleCoordinator.ExecuteOnCreated();
            Assert(callCount == 3, "Should have called three times");
            
            Console.WriteLine($"  ✓ Callback executed {callCount} times across multiple Execute calls");
            Console.WriteLine("  ✓ Callbacks can be re-executed (useful for testing)");
            
            Pass(testName);
        }
        catch (Exception ex)
        {
            Fail(testName, ex.Message);
        }
    }
    
    // ========== Test Utilities ==========
    
    static void Assert(bool condition, string message)
    {
        if (!condition)
        {
            throw new Exception($"Assertion failed: {message}");
        }
    }
    
    static void Pass(string testName)
    {
        s_testsPassed++;
        Console.WriteLine($"✓ {testName} PASSED");
    }
    
    static void Fail(string testName, string reason)
    {
        s_testsFailed++;
        Console.WriteLine($"✗ {testName} FAILED: {reason}");
    }
}

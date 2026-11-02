using System;
using System.Collections.Generic;
using System.Threading;
using MoreNetworkCapacityRenew;

/// <summary>
/// Test suite for PerformanceMonitor class.
/// 
/// Tests:
/// 1. TrackCoordinationTiming tracks operation duration
/// 2. GetPerformanceMetrics returns accurate statistics
/// 3. Warning thresholds trigger logging for slow operations
/// 4. Multiple operations tracked independently
/// 5. Performance report generation
/// 
/// Requirements tested: 10.5, 10.6, 10.7, 10.8, 11.1, 11.2
/// </summary>
public static class PerformanceMonitorTests
{
    private static int s_passCount = 0;
    private static int s_failCount = 0;
    
    public static void Main(string[] args)
    {
        Console.WriteLine("===========================================");
        Console.WriteLine("  PerformanceMonitor Test Suite");
        Console.WriteLine("===========================================");
        Console.WriteLine();
        
        try
        {
            // Reset metrics before tests
            PerformanceMonitor.ResetMetrics();
            
            // Run all tests
            Test1_BasicTiming();
            Test2_MultipleOperations();
            Test3_WarningThreshold();
            Test4_GetPerformanceMetrics();
            Test5_PerformanceReport();
            Test6_ThreadSafety();
            Test7_CustomThresholds();
            Test8_DefaultThresholds();
            
            // Print summary
            Console.WriteLine();
            Console.WriteLine("===========================================");
            Console.WriteLine($"  Tests Passed: {s_passCount}");
            Console.WriteLine($"  Tests Failed: {s_failCount}");
            Console.WriteLine("===========================================");
            
            Environment.Exit(s_failCount > 0 ? 1 : 0);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ FATAL ERROR: {ex}");
            Environment.Exit(1);
        }
    }
    
    // ========== Test Cases ==========
    
    /// <summary>
    /// Test: TrackCoordinationTiming tracks operation duration (Req 10.5)
    /// </summary>
    private static void Test1_BasicTiming()
    {
        Console.WriteLine("Test 1: Basic timing tracking");
        PerformanceMonitor.ResetMetrics();
        
        try
        {
            // Simulate an operation that takes ~100ms
            using (PerformanceMonitor.TrackCoordinationTiming("TestOperation"))
            {
                Thread.Sleep(100);
            }
            
            // Get metrics
            var metrics = PerformanceMonitor.GetPerformanceMetrics();
            Assert(metrics.ContainsKey("TestOperation"), "Should track TestOperation");
            
            var opMetrics = metrics["TestOperation"];
            Assert(opMetrics.Count == 1, "Should record 1 execution");
            Assert(opMetrics.TotalTimeMs >= 90 && opMetrics.TotalTimeMs <= 150, 
                   $"Duration should be ~100ms, got {opMetrics.TotalTimeMs:F2}ms");
            Assert(opMetrics.MaxTimeMs >= 90 && opMetrics.MaxTimeMs <= 150,
                   $"Max duration should be ~100ms, got {opMetrics.MaxTimeMs:F2}ms");
            
            Console.WriteLine($"  ✓ Operation tracked correctly ({opMetrics.TotalTimeMs:F2}ms)");
            s_passCount++;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  ❌ FAILED: {ex.Message}");
            s_failCount++;
        }
    }
    
    /// <summary>
    /// Test: Multiple operations tracked independently (Req 10.5, 10.8)
    /// </summary>
    private static void Test2_MultipleOperations()
    {
        Console.WriteLine("\nTest 2: Multiple operations tracking");
        PerformanceMonitor.ResetMetrics();
        
        try
        {
            // Simulate different operations
            using (PerformanceMonitor.TrackCoordinationTiming("SaveOperation"))
            {
                Thread.Sleep(50);
            }
            
            using (PerformanceMonitor.TrackCoordinationTiming("LoadOperation"))
            {
                Thread.Sleep(75);
            }
            
            using (PerformanceMonitor.TrackCoordinationTiming("SaveOperation")) // Second save
            {
                Thread.Sleep(60);
            }
            
            // Get metrics
            var metrics = PerformanceMonitor.GetPerformanceMetrics();
            Assert(metrics.Count == 2, $"Should track 2 distinct operations, got {metrics.Count}");
            Assert(metrics.ContainsKey("SaveOperation"), "Should track SaveOperation");
            Assert(metrics.ContainsKey("LoadOperation"), "Should track LoadOperation");
            
            var saveMetrics = metrics["SaveOperation"];
            Assert(saveMetrics.Count == 2, $"SaveOperation should have 2 executions, got {saveMetrics.Count}");
            Assert(saveMetrics.TotalTimeMs >= 100 && saveMetrics.TotalTimeMs <= 150,
                   $"SaveOperation total should be ~110ms, got {saveMetrics.TotalTimeMs:F2}ms");
            
            var loadMetrics = metrics["LoadOperation"];
            Assert(loadMetrics.Count == 1, $"LoadOperation should have 1 execution, got {loadMetrics.Count}");
            Assert(loadMetrics.TotalTimeMs >= 70 && loadMetrics.TotalTimeMs <= 100,
                   $"LoadOperation total should be ~75ms, got {loadMetrics.TotalTimeMs:F2}ms");
            
            Console.WriteLine($"  ✓ SaveOperation: {saveMetrics.Count} executions, {saveMetrics.TotalTimeMs:F2}ms total");
            Console.WriteLine($"  ✓ LoadOperation: {loadMetrics.Count} executions, {loadMetrics.TotalTimeMs:F2}ms total");
            s_passCount++;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  ❌ FAILED: {ex.Message}");
            s_failCount++;
        }
    }
    
    /// <summary>
    /// Test: Warning threshold triggers logging (Req 10.6, 10.7, 11.2)
    /// </summary>
    private static void Test3_WarningThreshold()
    {
        Console.WriteLine("\nTest 3: Warning threshold triggering");
        PerformanceMonitor.ResetMetrics();
        
        try
        {
            // Simulate a slow operation (>50ms default threshold)
            Console.WriteLine("  Triggering slow operation (expecting warning log)...");
            using (PerformanceMonitor.TrackCoordinationTiming("SlowOperation"))
            {
                Thread.Sleep(75); // Exceeds 50ms coordination threshold
            }
            
            var metrics = PerformanceMonitor.GetPerformanceMetrics();
            Assert(metrics.ContainsKey("SlowOperation"), "Should track SlowOperation");
            
            var opMetrics = metrics["SlowOperation"];
            Assert(opMetrics.TotalTimeMs >= 70, 
                   $"Operation should take >= 70ms to trigger warning, got {opMetrics.TotalTimeMs:F2}ms");
            
            Console.WriteLine($"  ✓ Slow operation tracked ({opMetrics.TotalTimeMs:F2}ms, should have logged warning)");
            s_passCount++;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  ❌ FAILED: {ex.Message}");
            s_failCount++;
        }
    }
    
    /// <summary>
    /// Test: GetPerformanceMetrics returns accurate statistics (Req 10.8)
    /// </summary>
    private static void Test4_GetPerformanceMetrics()
    {
        Console.WriteLine("\nTest 4: GetPerformanceMetrics accuracy");
        PerformanceMonitor.ResetMetrics();
        
        try
        {
            // Record several operations with known durations
            using (PerformanceMonitor.TrackCoordinationTiming("TestOp")) { Thread.Sleep(10); }
            using (PerformanceMonitor.TrackCoordinationTiming("TestOp")) { Thread.Sleep(20); }
            using (PerformanceMonitor.TrackCoordinationTiming("TestOp")) { Thread.Sleep(30); }
            
            var metrics = PerformanceMonitor.GetPerformanceMetrics();
            Assert(metrics.ContainsKey("TestOp"), "Should track TestOp");
            
            var opMetrics = metrics["TestOp"];
            Assert(opMetrics.Count == 3, $"Should have 3 executions, got {opMetrics.Count}");
            
            // Check average is reasonable (~20ms average)
            double expectedAvg = 20.0;
            double avgDiff = Math.Abs(opMetrics.AverageTimeMs - expectedAvg);
            Assert(avgDiff < 10.0, 
                   $"Average should be ~{expectedAvg}ms, got {opMetrics.AverageTimeMs:F2}ms (diff {avgDiff:F2}ms)");
            
            // Check max is reasonable (~30ms)
            Assert(opMetrics.MaxTimeMs >= 25 && opMetrics.MaxTimeMs <= 45,
                   $"Max should be ~30ms, got {opMetrics.MaxTimeMs:F2}ms");
            
            // Check total is reasonable (~60ms total)
            Assert(opMetrics.TotalTimeMs >= 50 && opMetrics.TotalTimeMs <= 80,
                   $"Total should be ~60ms, got {opMetrics.TotalTimeMs:F2}ms");
            
            Console.WriteLine($"  ✓ Metrics: Count={opMetrics.Count}, Total={opMetrics.TotalTimeMs:F2}ms, " +
                            $"Avg={opMetrics.AverageTimeMs:F2}ms, Max={opMetrics.MaxTimeMs:F2}ms");
            s_passCount++;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  ❌ FAILED: {ex.Message}");
            s_failCount++;
        }
    }
    
    /// <summary>
    /// Test: Performance report generation (Req 10.8, 11.1)
    /// </summary>
    private static void Test5_PerformanceReport()
    {
        Console.WriteLine("\nTest 5: Performance report generation");
        PerformanceMonitor.ResetMetrics();
        
        try
        {
            // Record some operations
            using (PerformanceMonitor.TrackCoordinationTiming("LockAcquisition")) { Thread.Sleep(45); }
            using (PerformanceMonitor.TrackCoordinationTiming("PatchApplication")) { Thread.Sleep(95); }
            using (PerformanceMonitor.TrackCoordinationTiming("EventDispatch")) { Thread.Sleep(30); }
            
            string report = PerformanceMonitor.GetPerformanceReport();
            Assert(report != null && report.Length > 0, "Report should not be empty");
            Assert(report.Contains("LockAcquisition"), "Report should mention LockAcquisition");
            Assert(report.Contains("PatchApplication"), "Report should mention PatchApplication");
            Assert(report.Contains("EventDispatch"), "Report should mention EventDispatch");
            Assert(report.Contains("Warning Thresholds"), "Report should include threshold section");
            
            Console.WriteLine("  ✓ Report generated successfully:");
            Console.WriteLine(report);
            s_passCount++;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  ❌ FAILED: {ex.Message}");
            s_failCount++;
        }
    }
    
    /// <summary>
    /// Test: Thread safety of metrics tracking (Req 10.5)
    /// </summary>
    private static void Test6_ThreadSafety()
    {
        Console.WriteLine("\nTest 6: Thread safety");
        PerformanceMonitor.ResetMetrics();
        
        try
        {
            const int threadCount = 10;
            const int operationsPerThread = 5;
            var threads = new Thread[threadCount];
            
            // Start multiple threads recording operations
            for (int i = 0; i < threadCount; i++)
            {
                int threadId = i;
                threads[i] = new Thread(() =>
                {
                    for (int j = 0; j < operationsPerThread; j++)
                    {
                        using (PerformanceMonitor.TrackCoordinationTiming("ThreadSafeOp"))
                        {
                            Thread.Sleep(5);
                        }
                    }
                });
                threads[i].Start();
            }
            
            // Wait for all threads
            foreach (var thread in threads)
            {
                thread.Join();
            }
            
            // Verify metrics
            var metrics = PerformanceMonitor.GetPerformanceMetrics();
            Assert(metrics.ContainsKey("ThreadSafeOp"), "Should track ThreadSafeOp");
            
            var opMetrics = metrics["ThreadSafeOp"];
            int expectedCount = threadCount * operationsPerThread;
            Assert(opMetrics.Count == expectedCount,
                   $"Should record {expectedCount} executions, got {opMetrics.Count}");
            
            Console.WriteLine($"  ✓ {expectedCount} concurrent operations tracked correctly");
            s_passCount++;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  ❌ FAILED: {ex.Message}");
            s_failCount++;
        }
    }
    
    /// <summary>
    /// Test: Custom warning thresholds (Req 10.6)
    /// </summary>
    private static void Test7_CustomThresholds()
    {
        Console.WriteLine("\nTest 7: Custom warning thresholds");
        PerformanceMonitor.ResetMetrics();
        
        try
        {
            // Use custom threshold
            Console.WriteLine("  Testing custom threshold (30ms)...");
            using (PerformanceMonitor.TrackCoordinationTiming("CustomThresholdOp", warningThresholdMs: 30.0))
            {
                Thread.Sleep(40); // Should trigger warning with 30ms threshold
            }
            
            var metrics = PerformanceMonitor.GetPerformanceMetrics();
            Assert(metrics.ContainsKey("CustomThresholdOp"), "Should track CustomThresholdOp");
            
            var opMetrics = metrics["CustomThresholdOp"];
            Assert(opMetrics.TotalTimeMs >= 35,
                   $"Operation should exceed 30ms threshold, got {opMetrics.TotalTimeMs:F2}ms");
            
            Console.WriteLine($"  ✓ Custom threshold operation tracked ({opMetrics.TotalTimeMs:F2}ms, should have logged warning)");
            s_passCount++;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  ❌ FAILED: {ex.Message}");
            s_failCount++;
        }
    }
    
    /// <summary>
    /// Test: Default thresholds based on operation name (Req 10.6)
    /// </summary>
    private static void Test8_DefaultThresholds()
    {
        Console.WriteLine("\nTest 8: Default threshold detection");
        PerformanceMonitor.ResetMetrics();
        
        try
        {
            // Test lock operation threshold (50ms)
            Console.WriteLine("  Testing lock operation threshold (should be 50ms)...");
            using (PerformanceMonitor.TrackCoordinationTiming("LockAcquisitionTest"))
            {
                Thread.Sleep(10); // Fast, should not warn
            }
            
            // Test patch operation threshold (100ms)
            Console.WriteLine("  Testing patch operation threshold (should be 100ms)...");
            using (PerformanceMonitor.TrackCoordinationTiming("HarmonyPatchTest"))
            {
                Thread.Sleep(10); // Fast, should not warn
            }
            
            // Test event operation threshold (50ms)
            Console.WriteLine("  Testing event dispatch threshold (should be 50ms)...");
            using (PerformanceMonitor.TrackCoordinationTiming("EventDispatchTest"))
            {
                Thread.Sleep(10); // Fast, should not warn
            }
            
            var metrics = PerformanceMonitor.GetPerformanceMetrics();
            Assert(metrics.Count == 3, $"Should track 3 operations, got {metrics.Count}");
            
            Console.WriteLine("  ✓ All default threshold operations tracked");
            Console.WriteLine("  ✓ None should have triggered warnings (all <10ms)");
            s_passCount++;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  ❌ FAILED: {ex.Message}");
            s_failCount++;
        }
    }
    
    // ========== Helper Methods ==========
    
    private static void Assert(bool condition, string message)
    {
        if (!condition)
        {
            throw new Exception($"Assertion failed: {message}");
        }
    }
}

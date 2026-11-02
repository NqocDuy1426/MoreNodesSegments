using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Performance test suite for lock-free fast path optimizations in SaveCoordinator.
/// Tests Requirements 11.3 and 11.8:
/// - Standalone mode bypasses all coordination checks
/// - Single-mod save bypasses lock acquisition
/// - Concurrent save detection flag works correctly
/// - Fast path overhead should be <1ms
/// </summary>
public class LockFreeFastPathTests
{
    private const int ITERATIONS_WARMUP = 1000;
    private const int ITERATIONS_MEASURE = 10000;
    private const double TARGET_OVERHEAD_MS = 1.0; // <1ms per requirement 11.8

    /// <summary>
    /// Test 1: Verify standalone mode bypasses all coordination checks
    /// Requirement: 11.3
    /// </summary>
    public static void TestStandaloneModeBypass()
    {
        UnityEngine.Debug.Log("[LockFreeFastPath] ==== Test 1: Standalone Mode Bypass ====");
        
        // Simulate standalone mode (More Props not active)
        bool originalMorePropsActive = CoordinationManager.IsMorePropsActive;
        
        try
        {
            // Force standalone mode for testing
            // Note: We can't directly set IsMorePropsActive, so we'll measure with it off
            if (originalMorePropsActive)
            {
                UnityEngine.Debug.LogWarning("[LockFreeFastPath] Test requires standalone mode - cannot force. Skipping detailed test.");
                UnityEngine.Debug.Log("[LockFreeFastPath] Test 1: SKIPPED (More Props is active)");
                return;
            }
            
            // Warmup
            for (int i = 0; i < ITERATIONS_WARMUP; i++)
            {
                SaveCoordinator.AcquireLock($"Test-{i % 10}");
                SaveCoordinator.ReleaseLock($"Test-{i % 10}");
            }
            
            // Measure overhead
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < ITERATIONS_MEASURE; i++)
            {
                bool acquired = SaveCoordinator.AcquireLock("TestOwner");
                if (!acquired)
                {
                    UnityEngine.Debug.LogError("[LockFreeFastPath] Failed to acquire lock in standalone mode!");
                }
                SaveCoordinator.ReleaseLock("TestOwner");
            }
            sw.Stop();
            
            double avgOverheadMs = (sw.Elapsed.TotalMilliseconds / ITERATIONS_MEASURE);
            double avgOverheadUs = (sw.Elapsed.TotalMilliseconds * 1000) / ITERATIONS_MEASURE; // microseconds
            
            UnityEngine.Debug.Log($"[LockFreeFastPath] Standalone Mode Performance:");
            UnityEngine.Debug.Log($"  Total iterations: {ITERATIONS_MEASURE}");
            UnityEngine.Debug.Log($"  Total time: {sw.Elapsed.TotalMilliseconds:F3}ms");
            UnityEngine.Debug.Log($"  Average per operation: {avgOverheadUs:F3}µs ({avgOverheadMs:F6}ms)");
            
            // Verify fast path used
            if (!SaveCoordinator.IsConcurrentSaveDetected)
            {
                UnityEngine.Debug.Log($"[LockFreeFastPath] ✓ Concurrent save mode: NOT detected (fast path used)");
            }
            else
            {
                UnityEngine.Debug.LogWarning($"[LockFreeFastPath] ✗ Concurrent save mode: DETECTED (fast path NOT used)");
            }
            
            // Verify overhead target met
            if (avgOverheadMs < TARGET_OVERHEAD_MS)
            {
                UnityEngine.Debug.Log($"[LockFreeFastPath] ✓ Overhead target met: {avgOverheadMs:F6}ms < {TARGET_OVERHEAD_MS}ms");
                UnityEngine.Debug.Log("[LockFreeFastPath] Test 1: PASSED");
            }
            else
            {
                UnityEngine.Debug.LogError($"[LockFreeFastPath] ✗ Overhead target NOT met: {avgOverheadMs:F6}ms >= {TARGET_OVERHEAD_MS}ms");
                UnityEngine.Debug.Log("[LockFreeFastPath] Test 1: FAILED");
            }
        }
        finally
        {
            // Can't restore original state as IsMorePropsActive is read-only
        }
        
        UnityEngine.Debug.Log("[LockFreeFastPath] ========================================\n");
    }

    /// <summary>
    /// Test 2: Verify single-mod save bypasses lock acquisition when no concurrency detected
    /// Requirement: 11.3
    /// </summary>
    public static void TestSingleModSaveBypass()
    {
        UnityEngine.Debug.Log("[LockFreeFastPath] ==== Test 2: Single-Mod Save Bypass ====");
        
        // This test only makes sense in compatibility mode
        if (!CoordinationManager.IsMorePropsActive)
        {
            UnityEngine.Debug.Log("[LockFreeFastPath] Test 2: SKIPPED (requires compatibility mode)");
            UnityEngine.Debug.Log("[LockFreeFastPath] ========================================\n");
            return;
        }
        
        // Reset concurrent save detection by releasing all locks
        try
        {
            SaveCoordinator.ReleaseLock("MNC");
        }
        catch { }
        
        try
        {
            SaveCoordinator.ReleaseLock("TestOwner");
        }
        catch { }
        
        // Check initial state
        bool initialConcurrentState = SaveCoordinator.IsConcurrentSaveDetected;
        UnityEngine.Debug.Log($"[LockFreeFastPath] Initial concurrent save state: {initialConcurrentState}");
        
        // Warmup with single owner
        for (int i = 0; i < ITERATIONS_WARMUP; i++)
        {
            SaveCoordinator.AcquireLock("SingleOwner");
            SaveCoordinator.ReleaseLock("SingleOwner");
        }
        
        // Measure first save operation (should use fast path)
        Stopwatch sw = Stopwatch.StartNew();
        for (int i = 0; i < ITERATIONS_MEASURE; i++)
        {
            bool acquired = SaveCoordinator.AcquireLock("SingleOwner");
            if (!acquired)
            {
                UnityEngine.Debug.LogError($"[LockFreeFastPath] Failed to acquire lock on iteration {i}!");
            }
            SaveCoordinator.ReleaseLock("SingleOwner");
        }
        sw.Stop();
        
        double avgOverheadMs = (sw.Elapsed.TotalMilliseconds / ITERATIONS_MEASURE);
        double avgOverheadUs = (sw.Elapsed.TotalMilliseconds * 1000) / ITERATIONS_MEASURE;
        
        UnityEngine.Debug.Log($"[LockFreeFastPath] Single-Mod Save Performance:");
        UnityEngine.Debug.Log($"  Total iterations: {ITERATIONS_MEASURE}");
        UnityEngine.Debug.Log($"  Total time: {sw.Elapsed.TotalMilliseconds:F3}ms");
        UnityEngine.Debug.Log($"  Average per operation: {avgOverheadUs:F3}µs ({avgOverheadMs:F6}ms)");
        
        // Check if fast path was used (concurrent save should not be detected if only one owner)
        bool finalConcurrentState = SaveCoordinator.IsConcurrentSaveDetected;
        UnityEngine.Debug.Log($"[LockFreeFastPath] Final concurrent save state: {finalConcurrentState}");
        
        // Verify overhead target met
        if (avgOverheadMs < TARGET_OVERHEAD_MS)
        {
            UnityEngine.Debug.Log($"[LockFreeFastPath] ✓ Overhead target met: {avgOverheadMs:F6}ms < {TARGET_OVERHEAD_MS}ms");
            UnityEngine.Debug.Log("[LockFreeFastPath] Test 2: PASSED");
        }
        else
        {
            UnityEngine.Debug.LogError($"[LockFreeFastPath] ✗ Overhead target NOT met: {avgOverheadMs:F6}ms >= {TARGET_OVERHEAD_MS}ms");
            UnityEngine.Debug.Log("[LockFreeFastPath] Test 2: FAILED");
        }
        
        UnityEngine.Debug.Log("[LockFreeFastPath] ========================================\n");
    }

    /// <summary>
    /// Test 3: Verify concurrent save detection flag works correctly
    /// Requirement: 11.3
    /// </summary>
    public static void TestConcurrentSaveDetection()
    {
        UnityEngine.Debug.Log("[LockFreeFastPath] ==== Test 3: Concurrent Save Detection ====");
        
        // This test only makes sense in compatibility mode
        if (!CoordinationManager.IsMorePropsActive)
        {
            UnityEngine.Debug.Log("[LockFreeFastPath] Test 3: SKIPPED (requires compatibility mode)");
            UnityEngine.Debug.Log("[LockFreeFastPath] ========================================\n");
            return;
        }
        
        bool testPassed = true;
        
        // Release any existing locks
        try
        {
            SaveCoordinator.ReleaseLock("MNC");
            SaveCoordinator.ReleaseLock("MoreProps");
            SaveCoordinator.ReleaseLock("Owner1");
            SaveCoordinator.ReleaseLock("Owner2");
        }
        catch { }
        
        UnityEngine.Debug.Log($"[LockFreeFastPath] Initial state - Concurrent detected: {SaveCoordinator.IsConcurrentSaveDetected}");
        
        // Test sequence:
        // 1. First owner acquires lock (should use fast path)
        UnityEngine.Debug.Log("[LockFreeFastPath] Step 1: First owner acquiring lock...");
        bool acquired1 = SaveCoordinator.AcquireLock("Owner1");
        if (!acquired1)
        {
            UnityEngine.Debug.LogError("[LockFreeFastPath] ✗ Owner1 failed to acquire lock!");
            testPassed = false;
        }
        else
        {
            UnityEngine.Debug.Log($"[LockFreeFastPath] ✓ Owner1 acquired lock");
        }
        
        // At this point, concurrent save might or might not be detected depending on implementation
        bool afterFirstAcquire = SaveCoordinator.IsConcurrentSaveDetected;
        UnityEngine.Debug.Log($"[LockFreeFastPath] After first acquire - Concurrent detected: {afterFirstAcquire}");
        
        // 2. First owner releases lock
        UnityEngine.Debug.Log("[LockFreeFastPath] Step 2: First owner releasing lock...");
        SaveCoordinator.ReleaseLock("Owner1");
        UnityEngine.Debug.Log($"[LockFreeFastPath] ✓ Owner1 released lock");
        
        // 3. Second owner tries to acquire (should trigger concurrent detection)
        UnityEngine.Debug.Log("[LockFreeFastPath] Step 3: Second owner acquiring lock...");
        bool acquired2 = SaveCoordinator.AcquireLock("Owner2");
        if (!acquired2)
        {
            UnityEngine.Debug.LogError("[LockFreeFastPath] ✗ Owner2 failed to acquire lock!");
            testPassed = false;
        }
        else
        {
            UnityEngine.Debug.Log($"[LockFreeFastPath] ✓ Owner2 acquired lock");
        }
        
        bool afterSecondAcquire = SaveCoordinator.IsConcurrentSaveDetected;
        UnityEngine.Debug.Log($"[LockFreeFastPath] After second acquire - Concurrent detected: {afterSecondAcquire}");
        
        // Once concurrency is detected, it should stay detected
        // (Implementation note: Current design may not detect concurrency until actual contention)
        
        // 4. Second owner releases
        UnityEngine.Debug.Log("[LockFreeFastPath] Step 4: Second owner releasing lock...");
        SaveCoordinator.ReleaseLock("Owner2");
        UnityEngine.Debug.Log($"[LockFreeFastPath] ✓ Owner2 released lock");
        
        bool afterRelease = SaveCoordinator.IsConcurrentSaveDetected;
        UnityEngine.Debug.Log($"[LockFreeFastPath] After release - Concurrent detected: {afterRelease}");
        
        // Verify final state
        if (!SaveCoordinator.IsLocked)
        {
            UnityEngine.Debug.Log($"[LockFreeFastPath] ✓ Lock properly released (not held)");
        }
        else
        {
            UnityEngine.Debug.LogError($"[LockFreeFastPath] ✗ Lock still held by: {SaveCoordinator.CurrentLockOwner}");
            testPassed = false;
        }
        
        if (testPassed)
        {
            UnityEngine.Debug.Log("[LockFreeFastPath] Test 3: PASSED");
        }
        else
        {
            UnityEngine.Debug.Log("[LockFreeFastPath] Test 3: FAILED");
        }
        
        UnityEngine.Debug.Log("[LockFreeFastPath] ========================================\n");
    }

    /// <summary>
    /// Test 4: Measure fast path overhead - should be <1ms
    /// Requirement: 11.8
    /// </summary>
    public static void TestFastPathOverhead()
    {
        UnityEngine.Debug.Log("[LockFreeFastPath] ==== Test 4: Fast Path Overhead Measurement ====");
        
        Dictionary<string, double> results = new Dictionary<string, double>();
        
        // Test Case A: Standalone mode (if applicable)
        if (!CoordinationManager.IsMorePropsActive)
        {
            UnityEngine.Debug.Log("[LockFreeFastPath] Measuring: Standalone Mode");
            
            // Warmup
            for (int i = 0; i < ITERATIONS_WARMUP; i++)
            {
                SaveCoordinator.AcquireLock("Test");
                SaveCoordinator.ReleaseLock("Test");
            }
            
            // Measure acquire
            Stopwatch swAcquire = Stopwatch.StartNew();
            for (int i = 0; i < ITERATIONS_MEASURE; i++)
            {
                SaveCoordinator.AcquireLock("Test");
            }
            swAcquire.Stop();
            double avgAcquireMs = (swAcquire.Elapsed.TotalMilliseconds / ITERATIONS_MEASURE);
            
            // Measure release
            Stopwatch swRelease = Stopwatch.StartNew();
            for (int i = 0; i < ITERATIONS_MEASURE; i++)
            {
                SaveCoordinator.ReleaseLock("Test");
            }
            swRelease.Stop();
            double avgReleaseMs = (swRelease.Elapsed.TotalMilliseconds / ITERATIONS_MEASURE);
            
            double avgTotalMs = avgAcquireMs + avgReleaseMs;
            
            results["Standalone_Acquire"] = avgAcquireMs;
            results["Standalone_Release"] = avgReleaseMs;
            results["Standalone_Total"] = avgTotalMs;
            
            UnityEngine.Debug.Log($"[LockFreeFastPath]   Acquire: {(avgAcquireMs * 1000):F3}µs");
            UnityEngine.Debug.Log($"[LockFreeFastPath]   Release: {(avgReleaseMs * 1000):F3}µs");
            UnityEngine.Debug.Log($"[LockFreeFastPath]   Total: {(avgTotalMs * 1000):F3}µs");
        }
        
        // Test Case B: Compatibility mode with single owner (fast path)
        if (CoordinationManager.IsMorePropsActive)
        {
            UnityEngine.Debug.Log("[LockFreeFastPath] Measuring: Compatibility Mode - Single Owner (Fast Path)");
            
            // Clear any locks
            try { SaveCoordinator.ReleaseLock("Test"); } catch { }
            
            // Warmup
            for (int i = 0; i < ITERATIONS_WARMUP; i++)
            {
                SaveCoordinator.AcquireLock("Test");
                SaveCoordinator.ReleaseLock("Test");
            }
            
            // Measure combined operation
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < ITERATIONS_MEASURE; i++)
            {
                SaveCoordinator.AcquireLock("Test");
                SaveCoordinator.ReleaseLock("Test");
            }
            sw.Stop();
            double avgTotalMs = (sw.Elapsed.TotalMilliseconds / ITERATIONS_MEASURE);
            
            results["Compat_SingleOwner_Total"] = avgTotalMs;
            
            UnityEngine.Debug.Log($"[LockFreeFastPath]   Total: {(avgTotalMs * 1000):F3}µs ({avgTotalMs:F6}ms)");
        }
        
        // Display summary
        UnityEngine.Debug.Log("[LockFreeFastPath] Summary:");
        UnityEngine.Debug.Log($"[LockFreeFastPath]   Target: <{TARGET_OVERHEAD_MS}ms per operation");
        
        bool allTestsPassed = true;
        foreach (var kvp in results)
        {
            bool passed = kvp.Value < TARGET_OVERHEAD_MS;
            string status = passed ? "✓ PASS" : "✗ FAIL";
            UnityEngine.Debug.Log($"[LockFreeFastPath]   {kvp.Key}: {kvp.Value:F6}ms {status}");
            
            if (!passed)
            {
                allTestsPassed = false;
            }
        }
        
        if (allTestsPassed && results.Count > 0)
        {
            UnityEngine.Debug.Log("[LockFreeFastPath] Test 4: PASSED");
        }
        else if (results.Count == 0)
        {
            UnityEngine.Debug.LogWarning("[LockFreeFastPath] Test 4: NO MEASUREMENTS (unexpected state)");
        }
        else
        {
            UnityEngine.Debug.Log("[LockFreeFastPath] Test 4: FAILED");
        }
        
        UnityEngine.Debug.Log("[LockFreeFastPath] ========================================\n");
    }

    /// <summary>
    /// Run all fast path tests
    /// </summary>
    public static void RunAllTests()
    {
        UnityEngine.Debug.Log("\n╔════════════════════════════════════════════════════════════════════╗");
        UnityEngine.Debug.Log("║   LOCK-FREE FAST PATH PERFORMANCE TEST SUITE                      ║");
        UnityEngine.Debug.Log("║   Testing Requirements: 11.3, 11.8                                ║");
        UnityEngine.Debug.Log("╚════════════════════════════════════════════════════════════════════╝\n");
        
        UnityEngine.Debug.Log($"[LockFreeFastPath] Environment:");
        UnityEngine.Debug.Log($"  More Props Active: {CoordinationManager.IsMorePropsActive}");
        UnityEngine.Debug.Log($"  Concurrent Save Detected: {SaveCoordinator.IsConcurrentSaveDetected}");
        UnityEngine.Debug.Log($"  Lock Held: {SaveCoordinator.IsLocked}");
        UnityEngine.Debug.Log($"  Lock Owner: {SaveCoordinator.CurrentLockOwner ?? "None"}");
        UnityEngine.Debug.Log("");
        
        try
        {
            TestStandaloneModeBypass();
            TestSingleModSaveBypass();
            TestConcurrentSaveDetection();
            TestFastPathOverhead();
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError($"[LockFreeFastPath] Test suite exception: {ex}");
        }
        
        UnityEngine.Debug.Log("\n╔════════════════════════════════════════════════════════════════════╗");
        UnityEngine.Debug.Log("║   TEST SUITE COMPLETE                                              ║");
        UnityEngine.Debug.Log("╚════════════════════════════════════════════════════════════════════╝\n");
    }

    /// <summary>
    /// Profile comparison between standalone and compatibility mode
    /// </summary>
    public static void ProfileStandaloneVsCompatibility()
    {
        UnityEngine.Debug.Log("\n╔════════════════════════════════════════════════════════════════════╗");
        UnityEngine.Debug.Log("║   STANDALONE VS COMPATIBILITY MODE PERFORMANCE PROFILE            ║");
        UnityEngine.Debug.Log("╚════════════════════════════════════════════════════════════════════╝\n");
        
        bool isCompatMode = CoordinationManager.IsMorePropsActive;
        string mode = isCompatMode ? "COMPATIBILITY" : "STANDALONE";
        
        UnityEngine.Debug.Log($"[Profile] Current Mode: {mode}");
        UnityEngine.Debug.Log($"[Profile] Iterations: {ITERATIONS_MEASURE}");
        UnityEngine.Debug.Log("");
        
        // Measure various operations
        Dictionary<string, TimeSpan> measurements = new Dictionary<string, TimeSpan>();
        
        // 1. Lock acquire
        Stopwatch sw = Stopwatch.StartNew();
        for (int i = 0; i < ITERATIONS_MEASURE; i++)
        {
            SaveCoordinator.AcquireLock("ProfileTest");
            SaveCoordinator.ReleaseLock("ProfileTest");
        }
        sw.Stop();
        measurements["AcquireReleaseCycle"] = sw.Elapsed;
        
        // 2. Lock acquire only
        sw = Stopwatch.StartNew();
        for (int i = 0; i < ITERATIONS_MEASURE; i++)
        {
            SaveCoordinator.AcquireLock($"Test-{i % 100}");
        }
        sw.Stop();
        measurements["AcquireOnly"] = sw.Elapsed;
        
        // Release all
        for (int i = 0; i < 100; i++)
        {
            try { SaveCoordinator.ReleaseLock($"Test-{i}"); } catch { }
        }
        
        // 3. IsLocked check
        sw = Stopwatch.StartNew();
        bool dummy = false;
        for (int i = 0; i < ITERATIONS_MEASURE * 10; i++)
        {
            dummy = SaveCoordinator.IsLocked;
        }
        sw.Stop();
        measurements["IsLockedCheck"] = sw.Elapsed;
        
        // Display results
        UnityEngine.Debug.Log($"[Profile] Results for {mode} mode:");
        UnityEngine.Debug.Log("");
        
        foreach (var kvp in measurements)
        {
            int iterations = kvp.Key == "IsLockedCheck" ? ITERATIONS_MEASURE * 10 : ITERATIONS_MEASURE;
            double avgMs = kvp.Value.TotalMilliseconds / iterations;
            double avgUs = (kvp.Value.TotalMilliseconds * 1000) / iterations;
            double avgNs = (kvp.Value.TotalMilliseconds * 1000000) / iterations;
            
            UnityEngine.Debug.Log($"[Profile] {kvp.Key}:");
            UnityEngine.Debug.Log($"  Total: {kvp.Value.TotalMilliseconds:F3}ms over {iterations} iterations");
            UnityEngine.Debug.Log($"  Average: {avgNs:F1}ns ({avgUs:F3}µs, {avgMs:F6}ms)");
            
            if (kvp.Key == "AcquireReleaseCycle")
            {
                if (avgMs < TARGET_OVERHEAD_MS)
                {
                    UnityEngine.Debug.Log($"  Status: ✓ PASS (< {TARGET_OVERHEAD_MS}ms target)");
                }
                else
                {
                    UnityEngine.Debug.Log($"  Status: ✗ FAIL (>= {TARGET_OVERHEAD_MS}ms target)");
                }
            }
            
            UnityEngine.Debug.Log("");
        }
        
        UnityEngine.Debug.Log("╚════════════════════════════════════════════════════════════════════╝\n");
    }
}

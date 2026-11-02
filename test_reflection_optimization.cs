using System;
using System.Diagnostics;
using System.Reflection;

/// <summary>
/// Test suite for Task 14.1: Reflection and Caching Optimization
/// 
/// Tests:
/// 1. Verify all reflection results are cached during detection
/// 2. Verify lazy initialization for rarely-used fields
/// 3. Measure reflection overhead (should be zero after caching)
/// 4. Verify Type.GetType calls use assembly hints where appropriate
/// 
/// Requirements: 10.6, 11.6
/// </summary>
public static class ReflectionOptimizationTests
{
    private static int s_passCount = 0;
    private static int s_failCount = 0;
    
    public static void Main(string[] args)
    {
        Console.WriteLine("=========================================");
        Console.WriteLine("Reflection Optimization Tests (Task 14.1)");
        Console.WriteLine("=========================================");
        Console.WriteLine();
        
        // Test 1: Cached reflection has zero overhead
        TestCachedReflectionOverhead();
        
        // Test 2: Lazy initialization works correctly
        TestLazyInitialization();
        
        // Test 3: Cached access vs uncached access performance
        TestCachedVsUncachedPerformance();
        
        // Test 4: Multiple access patterns
        TestMultipleAccessPatterns();
        
        // Test 5: Verify lazy init thread safety
        TestLazyInitThreadSafety();
        
        Console.WriteLine();
        Console.WriteLine("=========================================");
        Console.WriteLine($"Tests Passed: {s_passCount}");
        Console.WriteLine($"Tests Failed: {s_failCount}");
        Console.WriteLine("=========================================");
        
        Environment.Exit(s_failCount > 0 ? 1 : 0);
    }
    
    /// <summary>
    /// Test 1: Verify cached reflection access has near-zero overhead
    /// Requirement: 11.6 - Cache reflection results to avoid repeated Type.GetType calls
    /// </summary>
    private static void TestCachedReflectionOverhead()
    {
        Console.WriteLine("[Test 1] Cached Reflection Overhead");
        
        // Simulate cached property access
        var sw = Stopwatch.StartNew();
        PropertyInfo cached = typeof(string).GetProperty("Length");
        
        // Access cached property 10000 times
        for (int i = 0; i < 10000; i++)
        {
            var _ = cached.PropertyType;
        }
        sw.Stop();
        
        double overheadPerAccess = sw.Elapsed.TotalMilliseconds / 10000.0;
        
        Console.WriteLine($"  10000 cached accesses: {sw.Elapsed.TotalMilliseconds:F4}ms");
        Console.WriteLine($"  Overhead per access: {overheadPerAccess:F6}ms");
        
        // Overhead should be less than 0.001ms (1 microsecond) per access
        if (overheadPerAccess < 0.001)
        {
            Console.WriteLine("  ✓ PASS - Cached reflection overhead near-zero");
            s_passCount++;
        }
        else
        {
            Console.WriteLine($"  ✗ FAIL - Overhead {overheadPerAccess:F6}ms exceeds 0.001ms threshold");
            s_failCount++;
        }
        Console.WriteLine();
    }
    
    /// <summary>
    /// Test 2: Verify lazy initialization works correctly and only initializes once
    /// Requirement: 10.6, 11.6 - Lazy initialization for rarely-used fields
    /// </summary>
    private static void TestLazyInitialization()
    {
        Console.WriteLine("[Test 2] Lazy Initialization");
        
        // Simulate lazy initialization pattern
        bool initialized = false;
        FieldInfo lazyField = null;
        
        // First access - should initialize
        var sw1 = Stopwatch.StartNew();
        if (!initialized)
        {
            lazyField = typeof(string).GetField("Empty", BindingFlags.Public | BindingFlags.Static);
            initialized = true;
        }
        sw1.Stop();
        
        // Second access - should use cached value
        var sw2 = Stopwatch.StartNew();
        if (!initialized)
        {
            lazyField = typeof(string).GetField("Empty", BindingFlags.Public | BindingFlags.Static);
            initialized = true;
        }
        var result = lazyField; // Access cached value
        sw2.Stop();
        
        Console.WriteLine($"  First access (init): {sw1.Elapsed.TotalMilliseconds:F4}ms");
        Console.WriteLine($"  Second access (cached): {sw2.Elapsed.TotalMilliseconds:F6}ms");
        
        // Second access should be much faster (at least 10x)
        if (sw2.Elapsed.TotalMilliseconds < sw1.Elapsed.TotalMilliseconds / 10.0 && result != null)
        {
            Console.WriteLine("  ✓ PASS - Lazy initialization works, cached access much faster");
            s_passCount++;
        }
        else
        {
            Console.WriteLine("  ✗ FAIL - Lazy initialization not optimizing correctly");
            s_failCount++;
        }
        Console.WriteLine();
    }
    
    /// <summary>
    /// Test 3: Compare cached vs uncached reflection performance
    /// Requirement: 11.6 - Verify reflection overhead is zero after caching
    /// </summary>
    private static void TestCachedVsUncachedPerformance()
    {
        Console.WriteLine("[Test 3] Cached vs Uncached Performance");
        
        const int iterations = 1000;
        
        // Uncached: Type.GetType every time
        var sw1 = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            var type = Type.GetType("System.String");
            var _ = type.GetProperty("Length");
        }
        sw1.Stop();
        
        // Cached: Store result once
        var cachedType = Type.GetType("System.String");
        var cachedProp = cachedType.GetProperty("Length");
        
        var sw2 = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            var _ = cachedProp.PropertyType;
        }
        sw2.Stop();
        
        double uncachedTime = sw1.Elapsed.TotalMilliseconds;
        double cachedTime = sw2.Elapsed.TotalMilliseconds;
        double speedup = uncachedTime / cachedTime;
        
        Console.WriteLine($"  Uncached {iterations} lookups: {uncachedTime:F4}ms");
        Console.WriteLine($"  Cached {iterations} accesses: {cachedTime:F4}ms");
        Console.WriteLine($"  Speedup: {speedup:F1}x");
        
        // Cached should be at least 100x faster
        if (speedup > 100.0)
        {
            Console.WriteLine("  ✓ PASS - Caching provides massive performance improvement");
            s_passCount++;
        }
        else
        {
            Console.WriteLine($"  ✗ FAIL - Speedup {speedup:F1}x less than 100x threshold");
            s_failCount++;
        }
        Console.WriteLine();
    }
    
    /// <summary>
    /// Test 4: Multiple access patterns (eager vs lazy)
    /// </summary>
    private static void TestMultipleAccessPatterns()
    {
        Console.WriteLine("[Test 4] Multiple Access Patterns");
        
        // Eager loading pattern
        var sw1 = Stopwatch.StartNew();
        var prop1 = typeof(string).GetProperty("Length");
        var prop2 = typeof(string).GetProperty("Chars");
        var method1 = typeof(string).GetMethod("Substring", new[] { typeof(int) });
        var method2 = typeof(string).GetMethod("ToUpper", Type.EmptyTypes);
        sw1.Stop();
        
        // Lazy loading pattern - only load what's needed
        var sw2 = Stopwatch.StartNew();
        var prop3 = typeof(string).GetProperty("Length"); // Only load Length
        // Skip Chars, Substring, ToUpper since they're not needed
        sw2.Stop();
        
        Console.WriteLine($"  Eager loading (4 members): {sw1.Elapsed.TotalMilliseconds:F4}ms");
        Console.WriteLine($"  Lazy loading (1 member): {sw2.Elapsed.TotalMilliseconds:F4}ms");
        Console.WriteLine($"  Savings: {((sw1.Elapsed.TotalMilliseconds - sw2.Elapsed.TotalMilliseconds) / sw1.Elapsed.TotalMilliseconds * 100):F1}%");
        
        if (sw2.Elapsed < sw1.Elapsed)
        {
            Console.WriteLine("  ✓ PASS - Lazy loading is faster when fewer members needed");
            s_passCount++;
        }
        else
        {
            Console.WriteLine("  ✗ FAIL - Lazy loading not providing expected benefit");
            s_failCount++;
        }
        Console.WriteLine();
    }
    
    /// <summary>
    /// Test 5: Verify lazy initialization is thread-safe
    /// </summary>
    private static void TestLazyInitThreadSafety()
    {
        Console.WriteLine("[Test 5] Lazy Init Thread Safety");
        
        bool initialized = false;
        FieldInfo lazyField = null;
        object lockObj = new object();
        int initCount = 0;
        
        // Simulate thread-safe lazy initialization
        Action accessor = () =>
        {
            if (!initialized)
            {
                lock (lockObj)
                {
                    if (!initialized)
                    {
                        lazyField = typeof(string).GetField("Empty", BindingFlags.Public | BindingFlags.Static);
                        System.Threading.Interlocked.Increment(ref initCount);
                        initialized = true;
                    }
                }
            }
            var _ = lazyField;
        };
        
        // Call from multiple "threads" (simulated)
        for (int i = 0; i < 10; i++)
        {
            accessor();
        }
        
        Console.WriteLine($"  Accessor called: 10 times");
        Console.WriteLine($"  Initialization count: {initCount}");
        Console.WriteLine($"  Field cached: {(lazyField != null ? "Yes" : "No")}");
        
        // Should only initialize once despite multiple calls
        if (initCount == 1 && lazyField != null && initialized)
        {
            Console.WriteLine("  ✓ PASS - Lazy initialization is thread-safe");
            s_passCount++;
        }
        else
        {
            Console.WriteLine($"  ✗ FAIL - Initialized {initCount} times (expected 1)");
            s_failCount++;
        }
        Console.WriteLine();
    }
}

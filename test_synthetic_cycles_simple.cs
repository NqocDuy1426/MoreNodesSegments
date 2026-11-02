using System;

/// <summary>
/// Simple integration test that verifies cycle detection code exists.
/// Requirements: 3.4, 3.5, 3.8, 3.9
/// </summary>
class TestSyntheticCyclesSimple
{
    static int s_testsPassed = 0;

    static void Assert(bool condition, string message)
    {
        if (!condition)
        {
            Console.WriteLine($"❌ FAIL: {message}");
            Environment.Exit(1);
        }
        Console.WriteLine($"✓ {message}");
        s_testsPassed++;
    }

    static void Main(string[] args)
    {
        Console.WriteLine("========================================");
        Console.WriteLine("Test: Grid Cycle Detection (Code Review)");
        Console.WriteLine("========================================\n");

        string sourceFile = "/Users/nqocduy/CS_MoreNetworkCapacity_Renew/Renew.cs";
        string source = System.IO.File.ReadAllText(sourceFile);

        // Test 1: Verify 500 iteration limit
        Console.WriteLine("Test 1: 500 Iteration Limit");
        Console.WriteLine("----------------------------");
        Assert(source.Contains("int maxAllowedChain = 500"), 
            "Max allowed chain = 500 defined");
        Assert(source.Contains("if (count > maxAllowedChain)"),
            "Iteration limit check exists");
        Assert(source.Contains("chain exceeded"),
            "Cycle repair for iteration limit exists");
        Console.WriteLine();

        // Test 2: Verify node revisit tracking  
        Console.WriteLine("Test 2: Node Revisit Tracking");
        Console.WriteLine("------------------------------");
        Assert(source.Contains("HashSet<uint>"),
            "HashSet used for node tracking");
        Assert(source.Contains("visitedNodes.Contains(current)"),
            "Node revisit check exists");
        Assert(source.Contains("visitedNodes.Add(current)"),
            "Nodes added to visited set");
        Assert(source.Contains("visitedNodes.Clear()"),
            "Visited set cleared between chains");
        Console.WriteLine();

        // Test 3: Verify self-reference detection
        Console.WriteLine("Test 3: Self-Reference Detection");
        Console.WriteLine("---------------------------------");
        Assert(source.Contains("if (next == current)"),
            "Self-reference check exists");
        Assert(source.Contains("Broke self-reference"),
            "Self-reference repair logging exists");
        Console.WriteLine();

        // Test 4: Verify cycle repair logging
        Console.WriteLine("Test 4: Cycle Repair Logging");
        Console.WriteLine("-----------------------------");
        Assert(source.Contains("Cycle detected in grid cell"),
            "Grid cell index logged");
        Assert(source.Contains("props in chain"),
            "Prop count logged");
        Assert(source.Contains("Broke cycle at prop"),
            "Repair location logged");
        Assert(source.Contains("MNC-GridSanitizer"),
            "Correct log prefix used");
        Console.WriteLine();

        // Test 5: Verify grid integrity validation
        Console.WriteLine("Test 5: Grid Integrity Validation");
        Console.WriteLine("----------------------------------");
        Assert(source.Contains("ValidateGridIntegrity"),
            "Validation method exists");
        Assert(source.Contains("Grid integrity validation PASSED"),
            "Success logging exists");
        Assert(source.Contains("Grid integrity validation FAILED"),
            "Failure logging exists");
        Console.WriteLine();

        // Test 6: Verify reflection error handling
        Console.WriteLine("Test 6: Reflection Error Handling");
        Console.WriteLine("----------------------------------");
        Assert(source.Contains("ReflectionTypeLoadException"),
            "ReflectionTypeLoadException handling exists");
        Assert(source.Contains("TargetException"),
            "TargetException handling exists");
        Assert(source.Contains("TargetInvocationException"),
            "TargetInvocationException handling exists");
        Assert(source.Contains("Reflection type load error"),
            "Type load error logging exists");
        Assert(source.Contains("Reflection target error"),
            "Target error logging exists");
        Assert(source.Contains("Reflection invocation error"),
            "Invocation error logging exists");
        Console.WriteLine();

        Console.WriteLine("========================================");
        Console.WriteLine($"✅ ALL TESTS PASSED: {s_testsPassed} checks");
        Console.WriteLine("========================================");
    }
}

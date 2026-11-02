using System;
using UnityEngine;

/// <summary>
/// Simple test runner for adapter API tests.
/// Can be invoked from game console or mod initialization.
/// </summary>
public class AdapterTestRunner
{
    public static void Main()
    {
        try
        {
            Debug.Log("===========================================");
            Debug.Log("STARTING ADAPTER API TEST SUITE");
            Debug.Log("===========================================");
            
            // Initialize coordination system components
            ConfigManager.Initialize();
            CoordinationManager.Initialize();
            
            // Run the adapter API tests
            AdapterAPITests.RunAllTests();
            
            Debug.Log("\n===========================================");
            Debug.Log("ADAPTER API TESTS FINISHED");
            Debug.Log("===========================================");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Test runner failed: {ex}");
        }
    }
}

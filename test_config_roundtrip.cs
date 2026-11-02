using System;
using System.IO;
using System.Collections.Generic;
using MoreNetworkCapacityRenew;

/// <summary>
/// Test round-trip property: parse → serialize → parse produces equivalent object
/// This validates Requirement 9.1, 9.2, 9.3, 9.4, 9.5, 9.6
/// </summary>
class TestConfigRoundTrip
{
    static void Main()
    {
        Console.WriteLine("========================================");
        Console.WriteLine("ConfigManager Round-Trip Property Test");
        Console.WriteLine("========================================\n");
        
        // Create test config with non-default values
        var originalConfig = new CompatibilityConfig();
        originalConfig.EnableCoordination = false;
        originalConfig.EnableSaveCoordination = true;
        originalConfig.EnableGridCoordination = false;
        originalConfig.EnableRenderCoordination = true;
        originalConfig.EnableDebugLogging = true;
        originalConfig.EnableSelfTest = true;
        originalConfig.SerializationLockTimeoutSeconds = 45;
        originalConfig.GridSanitizationDelayMs = 250;
        originalConfig.OrphanedSaveRecoveryDelaySeconds = 20;
        
        Console.WriteLine("Original Config:");
        PrintConfig(originalConfig);
        
        // Step 1: Serialize to JSON string
        var dict1 = originalConfig.ToDictionary();
        string json = SimpleJsonParser.Stringify(dict1, 2);
        Console.WriteLine("\nSerialized JSON:");
        Console.WriteLine(json);
        
        // Step 2: Parse JSON string
        var dict2 = SimpleJsonParser.Parse(json);
        
        // Step 3: Reconstruct config from parsed dictionary
        var parsedConfig = CompatibilityConfig.FromDictionary(dict2);
        
        Console.WriteLine("\nParsed Config:");
        PrintConfig(parsedConfig);
        
        // Verify all fields match
        bool allMatch = true;
        
        allMatch &= Check(originalConfig.EnableCoordination == parsedConfig.EnableCoordination, "EnableCoordination");
        allMatch &= Check(originalConfig.EnableSaveCoordination == parsedConfig.EnableSaveCoordination, "EnableSaveCoordination");
        allMatch &= Check(originalConfig.EnableGridCoordination == parsedConfig.EnableGridCoordination, "EnableGridCoordination");
        allMatch &= Check(originalConfig.EnableRenderCoordination == parsedConfig.EnableRenderCoordination, "EnableRenderCoordination");
        allMatch &= Check(originalConfig.EnableDebugLogging == parsedConfig.EnableDebugLogging, "EnableDebugLogging");
        allMatch &= Check(originalConfig.EnableSelfTest == parsedConfig.EnableSelfTest, "EnableSelfTest");
        allMatch &= Check(originalConfig.SerializationLockTimeoutSeconds == parsedConfig.SerializationLockTimeoutSeconds, "SerializationLockTimeoutSeconds");
        allMatch &= Check(originalConfig.GridSanitizationDelayMs == parsedConfig.GridSanitizationDelayMs, "GridSanitizationDelayMs");
        allMatch &= Check(originalConfig.OrphanedSaveRecoveryDelaySeconds == parsedConfig.OrphanedSaveRecoveryDelaySeconds, "OrphanedSaveRecoveryDelaySeconds");
        
        // Test with file I/O
        Console.WriteLine("\n--- Testing with File I/O ---");
        string testFile = "test_config.json";
        
        try
        {
            // Write to file
            File.WriteAllText(testFile, json);
            Console.WriteLine($"Wrote config to {testFile}");
            
            // Read from file
            string jsonFromFile = File.ReadAllText(testFile);
            var dict3 = SimpleJsonParser.Parse(jsonFromFile);
            var configFromFile = CompatibilityConfig.FromDictionary(dict3);
            
            Console.WriteLine("\nConfig from file:");
            PrintConfig(configFromFile);
            
            // Verify file round-trip
            allMatch &= Check(originalConfig.EnableCoordination == configFromFile.EnableCoordination, "File: EnableCoordination");
            allMatch &= Check(originalConfig.SerializationLockTimeoutSeconds == configFromFile.SerializationLockTimeoutSeconds, "File: SerializationLockTimeoutSeconds");
            
            // Clean up
            File.Delete(testFile);
            Console.WriteLine($"\nCleaned up {testFile}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError during file I/O test: {ex.Message}");
            allMatch = false;
        }
        
        Console.WriteLine("\n========================================");
        if (allMatch)
        {
            Console.WriteLine("✓ Round-trip property VERIFIED");
            Console.WriteLine("  Parse → Serialize → Parse produces equivalent object");
            Console.WriteLine("========================================");
            Environment.Exit(0);
        }
        else
        {
            Console.WriteLine("✗ Round-trip property FAILED");
            Console.WriteLine("========================================");
            Environment.Exit(1);
        }
    }
    
    static bool Check(bool condition, string fieldName)
    {
        if (condition)
        {
            Console.WriteLine($"  ✓ {fieldName} matches");
            return true;
        }
        else
        {
            Console.WriteLine($"  ✗ {fieldName} MISMATCH");
            return false;
        }
    }
    
    static void PrintConfig(CompatibilityConfig config)
    {
        Console.WriteLine($"  EnableCoordination: {config.EnableCoordination}");
        Console.WriteLine($"  EnableSaveCoordination: {config.EnableSaveCoordination}");
        Console.WriteLine($"  EnableGridCoordination: {config.EnableGridCoordination}");
        Console.WriteLine($"  EnableRenderCoordination: {config.EnableRenderCoordination}");
        Console.WriteLine($"  EnableDebugLogging: {config.EnableDebugLogging}");
        Console.WriteLine($"  EnableSelfTest: {config.EnableSelfTest}");
        Console.WriteLine($"  SerializationLockTimeoutSeconds: {config.SerializationLockTimeoutSeconds}");
        Console.WriteLine($"  GridSanitizationDelayMs: {config.GridSanitizationDelayMs}");
        Console.WriteLine($"  OrphanedSaveRecoveryDelaySeconds: {config.OrphanedSaveRecoveryDelaySeconds}");
    }
}

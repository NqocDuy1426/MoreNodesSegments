using System;
using System.IO;
using System.Collections.Generic;
using MoreNetworkCapacityRenew;

/// <summary>
/// Test suite for ConfigManager - validates JSON parsing, serialization, and round-trip property
/// </summary>
class TestConfigManager
{
    static int TestsPassed = 0;
    static int TestsFailed = 0;
    
    static void Main()
    {
        Console.WriteLine("========================================");
        Console.WriteLine("ConfigManager Test Suite");
        Console.WriteLine("========================================");
        
        TestSimpleJsonParser();
        TestConfigRoundTrip();
        TestConfigValidation();
        TestConfigDefaults();
        TestConfigFileOperations();
        
        Console.WriteLine("========================================");
        Console.WriteLine($"Tests Passed: {TestsPassed}");
        Console.WriteLine($"Tests Failed: {TestsFailed}");
        Console.WriteLine("========================================");
        
        Environment.Exit(TestsFailed > 0 ? 1 : 0);
    }
    
    static void Assert(bool condition, string testName)
    {
        if (condition)
        {
            Console.WriteLine($"✓ {testName}");
            TestsPassed++;
        }
        else
        {
            Console.WriteLine($"✗ {testName}");
            TestsFailed++;
        }
    }
    
    static void TestSimpleJsonParser()
    {
        Console.WriteLine("\n--- Testing SimpleJsonParser ---");
        
        // Test 1: Parse boolean values
        string json1 = "{\"EnableCoordination\": true, \"EnableDebugLogging\": false}";
        var dict1 = SimpleJsonParser.Parse(json1);
        Assert(dict1.ContainsKey("EnableCoordination"), "Parse: Contains EnableCoordination key");
        Assert((bool)dict1["EnableCoordination"] == true, "Parse: EnableCoordination is true");
        Assert((bool)dict1["EnableDebugLogging"] == false, "Parse: EnableDebugLogging is false");
        
        // Test 2: Parse integer values
        string json2 = "{\"SerializationLockTimeoutSeconds\": 60, \"GridSanitizationDelayMs\": 100}";
        var dict2 = SimpleJsonParser.Parse(json2);
        Assert((int)dict2["SerializationLockTimeoutSeconds"] == 60, "Parse: Integer value 60");
        Assert((int)dict2["GridSanitizationDelayMs"] == 100, "Parse: Integer value 100");
        
        // Test 3: Stringify
        var dict3 = new Dictionary<string, object>
        {
            { "EnableCoordination", true },
            { "SerializationLockTimeoutSeconds", 60 }
        };
        string json3 = SimpleJsonParser.Stringify(dict3);
        Assert(json3.Contains("\"EnableCoordination\": true"), "Stringify: Boolean true");
        Assert(json3.Contains("\"SerializationLockTimeoutSeconds\": 60"), "Stringify: Integer 60");
    }
    
    static void TestConfigRoundTrip()
    {
        Console.WriteLine("\n--- Testing Config Round-Trip Property ---");
        
        // Create a config with specific values
        var config1 = new CompatibilityConfig();
        config1.EnableCoordination = true;
        config1.EnableSaveCoordination = false;
        config1.EnableGridCoordination = true;
        config1.EnableRenderCoordination = false;
        config1.EnableDebugLogging = true;
        config1.EnableSelfTest = false;
        config1.SerializationLockTimeoutSeconds = 45;
        config1.GridSanitizationDelayMs = 200;
        config1.OrphanedSaveRecoveryDelaySeconds = 15;
        
        // Convert to dictionary
        var dict = config1.ToDictionary();
        
        // Convert back from dictionary
        var config2 = CompatibilityConfig.FromDictionary(dict);
        
        // Verify all fields match
        Assert(config1.EnableCoordination == config2.EnableCoordination, "Round-trip: EnableCoordination");
        Assert(config1.EnableSaveCoordination == config2.EnableSaveCoordination, "Round-trip: EnableSaveCoordination");
        Assert(config1.EnableGridCoordination == config2.EnableGridCoordination, "Round-trip: EnableGridCoordination");
        Assert(config1.EnableRenderCoordination == config2.EnableRenderCoordination, "Round-trip: EnableRenderCoordination");
        Assert(config1.EnableDebugLogging == config2.EnableDebugLogging, "Round-trip: EnableDebugLogging");
        Assert(config1.EnableSelfTest == config2.EnableSelfTest, "Round-trip: EnableSelfTest");
        Assert(config1.SerializationLockTimeoutSeconds == config2.SerializationLockTimeoutSeconds, "Round-trip: SerializationLockTimeoutSeconds");
        Assert(config1.GridSanitizationDelayMs == config2.GridSanitizationDelayMs, "Round-trip: GridSanitizationDelayMs");
        Assert(config1.OrphanedSaveRecoveryDelaySeconds == config2.OrphanedSaveRecoveryDelaySeconds, "Round-trip: OrphanedSaveRecoveryDelaySeconds");
        
        // Test full round-trip: Config -> JSON -> Config
        var dict1 = config1.ToDictionary();
        string json = SimpleJsonParser.Stringify(dict1);
        var dict2 = SimpleJsonParser.Parse(json);
        var config3 = CompatibilityConfig.FromDictionary(dict2);
        
        Assert(config1.EnableCoordination == config3.EnableCoordination, "Full round-trip: EnableCoordination");
        Assert(config1.SerializationLockTimeoutSeconds == config3.SerializationLockTimeoutSeconds, "Full round-trip: SerializationLockTimeoutSeconds");
    }
    
    static void TestConfigValidation()
    {
        Console.WriteLine("\n--- Testing Config Validation ---");
        
        // Valid config
        var config1 = new CompatibilityConfig();
        Assert(config1.Validate(), "Validation: Default config is valid");
        
        // Invalid timeout (too low)
        var config2 = new CompatibilityConfig();
        config2.SerializationLockTimeoutSeconds = 0;
        Assert(!config2.Validate(), "Validation: Timeout 0 is invalid");
        
        // Invalid timeout (too high)
        var config3 = new CompatibilityConfig();
        config3.SerializationLockTimeoutSeconds = 301;
        Assert(!config3.Validate(), "Validation: Timeout 301 is invalid");
        
        // Invalid delay (negative)
        var config4 = new CompatibilityConfig();
        config4.GridSanitizationDelayMs = -1;
        Assert(!config4.Validate(), "Validation: Negative delay is invalid");
        
        // Invalid delay (too high)
        var config5 = new CompatibilityConfig();
        config5.GridSanitizationDelayMs = 6000;
        Assert(!config5.Validate(), "Validation: Delay 6000ms is invalid");
        
        // Valid edge cases
        var config6 = new CompatibilityConfig();
        config6.SerializationLockTimeoutSeconds = 1;
        Assert(config6.Validate(), "Validation: Timeout 1 is valid");
        
        var config7 = new CompatibilityConfig();
        config7.SerializationLockTimeoutSeconds = 300;
        Assert(config7.Validate(), "Validation: Timeout 300 is valid");
    }
    
    static void TestConfigDefaults()
    {
        Console.WriteLine("\n--- Testing Config Defaults ---");
        
        var config = new CompatibilityConfig();
        
        Assert(config.EnableCoordination == true, "Default: EnableCoordination is true");
        Assert(config.EnableSaveCoordination == true, "Default: EnableSaveCoordination is true");
        Assert(config.EnableGridCoordination == true, "Default: EnableGridCoordination is true");
        Assert(config.EnableRenderCoordination == true, "Default: EnableRenderCoordination is true");
        Assert(config.EnableDebugLogging == false, "Default: EnableDebugLogging is false");
        Assert(config.EnableSelfTest == false, "Default: EnableSelfTest is false");
        Assert(config.SerializationLockTimeoutSeconds == 60, "Default: SerializationLockTimeoutSeconds is 60");
        Assert(config.GridSanitizationDelayMs == 0, "Default: GridSanitizationDelayMs is 0");
        Assert(config.OrphanedSaveRecoveryDelaySeconds == 10, "Default: OrphanedSaveRecoveryDelaySeconds is 10");
    }
    
    static void TestConfigFileOperations()
    {
        Console.WriteLine("\n--- Testing Config File Operations ---");
        
        // This test would require initializing ConfigManager which depends on Unity and Cities: Skylines
        // For now, we'll test the JSON round-trip which is the core requirement
        
        var config1 = new CompatibilityConfig();
        config1.EnableDebugLogging = true;
        config1.SerializationLockTimeoutSeconds = 75;
        
        // Serialize
        var dict1 = config1.ToDictionary();
        string json = SimpleJsonParser.Stringify(dict1, 2);
        
        // Deserialize
        var dict2 = SimpleJsonParser.Parse(json);
        var config2 = CompatibilityConfig.FromDictionary(dict2);
        
        Assert(config1.EnableDebugLogging == config2.EnableDebugLogging, "File round-trip: EnableDebugLogging matches");
        Assert(config1.SerializationLockTimeoutSeconds == config2.SerializationLockTimeoutSeconds, "File round-trip: Timeout matches");
        
        // Verify pretty printing
        Assert(json.Contains("\n"), "Pretty print: Contains newlines");
        Assert(json.Contains("  "), "Pretty print: Contains indentation");
    }
}

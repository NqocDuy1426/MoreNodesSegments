using System;
using System.Collections.Generic;
using MoreNetworkCapacityRenew;

/// <summary>
/// Unit tests for CoordinationMetadata serialization and deserialization
/// Tests Requirements: 11.1, 11.2, 11.3, 11.4 (Implementation Notes - Save Metadata Serializer)
/// 
/// Test Coverage:
/// 1. Metadata creation from current state
/// 2. Serialization to compact string format
/// 3. Deserialization from string format
/// 4. Round-trip property validation
/// 5. Metadata with various configurations (standalone, compatibility mode, with third-party mods)
/// 
/// Note: This test runs in standalone mode (without Unity environment)
/// </summary>
class TestCoordinationMetadata
{
    static int s_testsPassed = 0;
    static int s_testsFailed = 0;
    
    static void Main()
    {
        Console.WriteLine("========================================");
        Console.WriteLine("CoordinationMetadata Unit Tests");
        Console.WriteLine("========================================\n");
        
        Console.WriteLine("NOTE: Running in standalone mode (no Unity environment)");
        Console.WriteLine("Testing metadata serialization/deserialization functionality\n");
        
        // Run all tests
        Test_DefaultConstruction();
        Test_CreateFromCurrentState();
        Test_SerializeBasicMetadata();
        Test_SerializeWithThirdPartyMods();
        Test_DeserializeBasicMetadata();
        Test_DeserializeWithThirdPartyMods();
        Test_RoundTripProperty_Standalone();
        Test_RoundTripProperty_CompatibilityMode();
        Test_RoundTripProperty_WithThirdPartyMods();
        Test_DeserializeInvalidData();
        Test_DeserializeEmptyData();
        Test_ToStringMethod();
        
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
            Console.WriteLine("\nNOTE: These tests verify CoordinationMetadata functionality.");
            Console.WriteLine("Full integration testing with save/load requires running in Cities: Skylines.");
            Environment.Exit(0);
        }
        else
        {
            Console.WriteLine($"✗ {s_testsFailed} test(s) FAILED");
            Environment.Exit(1);
        }
    }
    
    // ========== Test Cases ==========
    
    static void Test_DefaultConstruction()
    {
        string testName = "Default Construction";
        Console.WriteLine($"\n--- {testName} ---");
        
        try
        {
            var metadata = new CoordinationMetadata();
            
            Assert(metadata.MetadataVersion == 1, "MetadataVersion should be 1");
            Assert(metadata.CoordinationVersion == "4.0.0", "CoordinationVersion should be 4.0.0");
            Assert(metadata.MNCVersion == "4.0.0", "MNCVersion should be 4.0.0");
            Assert(metadata.MorePropsVersion == null, "MorePropsVersion should be null");
            Assert(metadata.CompatibilityModeActive == false, "CompatibilityModeActive should be false");
            Assert(metadata.CoordinationEventsCount == 0, "CoordinationEventsCount should be 0");
            Assert(metadata.RegisteredThirdPartyMods != null, "RegisteredThirdPartyMods should not be null");
            Assert(metadata.RegisteredThirdPartyMods.Count == 0, "RegisteredThirdPartyMods should be empty");
            
            Console.WriteLine("  ✓ Default metadata constructed with correct values");
            Pass(testName);
        }
        catch (Exception ex)
        {
            Fail(testName, ex.Message);
        }
    }
    
    static void Test_CreateFromCurrentState()
    {
        string testName = "Create From Current State";
        Console.WriteLine($"\n--- {testName} ---");
        
        try
        {
            var metadata = CoordinationMetadata.CreateFromCurrentState();
            
            Assert(metadata != null, "Metadata should not be null");
            Assert(metadata.MetadataVersion == 1, "MetadataVersion should be 1");
            Assert(metadata.CoordinationVersion == "4.0.0", "CoordinationVersion should be 4.0.0");
            Assert(metadata.MNCVersion == "4.0.0", "MNCVersion should be 4.0.0");
            
            // In standalone mode, CoordinationManager.IsMorePropsActive should be false
            Assert(metadata.CompatibilityModeActive == false, "CompatibilityModeActive should be false in standalone mode");
            Assert(metadata.MorePropsVersion == null, "MorePropsVersion should be null in standalone mode");
            
            Console.WriteLine($"  ✓ Metadata created from current state: {metadata}");
            Pass(testName);
        }
        catch (Exception ex)
        {
            Fail(testName, ex.Message);
        }
    }
    
    static void Test_SerializeBasicMetadata()
    {
        string testName = "Serialize Basic Metadata";
        Console.WriteLine($"\n--- {testName} ---");
        
        try
        {
            var metadata = new CoordinationMetadata
            {
                MetadataVersion = 1,
                CoordinationVersion = "4.0.0",
                MNCVersion = "4.0.0",
                MorePropsVersion = null,
                CompatibilityModeActive = false,
                LastSaveTimestamp = new DateTime(2024, 1, 15, 12, 30, 0, DateTimeKind.Utc),
                CoordinationEventsCount = 0,
                RegisteredThirdPartyMods = new List<string>()
            };
            
            string serialized = metadata.SerializeMetadata();
            
            Assert(!string.IsNullOrEmpty(serialized), "Serialized string should not be empty");
            Assert(serialized.Contains("v=1"), "Should contain version");
            Assert(serialized.Contains("cv=4.0.0"), "Should contain coordination version");
            Assert(serialized.Contains("mnc=4.0.0"), "Should contain MNC version");
            Assert(serialized.Contains("mp=none"), "Should contain MoreProps=none");
            Assert(serialized.Contains("cm=false"), "Should contain CompatibilityMode=false");
            Assert(serialized.Contains("ec=0"), "Should contain events count");
            
            Console.WriteLine($"  ✓ Serialized: {serialized}");
            Pass(testName);
        }
        catch (Exception ex)
        {
            Fail(testName, ex.Message);
        }
    }
    
    static void Test_SerializeWithThirdPartyMods()
    {
        string testName = "Serialize With Third-Party Mods";
        Console.WriteLine($"\n--- {testName} ---");
        
        try
        {
            var metadata = new CoordinationMetadata
            {
                MetadataVersion = 1,
                CoordinationVersion = "4.0.0",
                MNCVersion = "4.0.0",
                MorePropsVersion = "1.0.7",
                CompatibilityModeActive = true,
                LastSaveTimestamp = new DateTime(2024, 1, 15, 12, 30, 0, DateTimeKind.Utc),
                CoordinationEventsCount = 42,
                RegisteredThirdPartyMods = new List<string> { "TMPE", "MoveIt", "NetworkExtensions" }
            };
            
            string serialized = metadata.SerializeMetadata();
            
            Assert(!string.IsNullOrEmpty(serialized), "Serialized string should not be empty");
            Assert(serialized.Contains("mp=1.0.7"), "Should contain MoreProps version");
            Assert(serialized.Contains("cm=true"), "Should contain CompatibilityMode=true");
            Assert(serialized.Contains("ec=42"), "Should contain events count");
            Assert(serialized.Contains("tm=TMPE,MoveIt,NetworkExtensions"), "Should contain third-party mods");
            
            Console.WriteLine($"  ✓ Serialized with mods: {serialized}");
            Pass(testName);
        }
        catch (Exception ex)
        {
            Fail(testName, ex.Message);
        }
    }
    
    static void Test_DeserializeBasicMetadata()
    {
        string testName = "Deserialize Basic Metadata";
        Console.WriteLine($"\n--- {testName} ---");
        
        try
        {
            string serialized = "v=1;cv=4.0.0;mnc=4.0.0;mp=none;cm=false;ts=2024-01-15T12:30:00.0000000Z;ec=0";
            
            var metadata = CoordinationMetadata.DeserializeMetadata(serialized);
            
            Assert(metadata != null, "Metadata should not be null");
            Assert(metadata.MetadataVersion == 1, "MetadataVersion should be 1");
            Assert(metadata.CoordinationVersion == "4.0.0", "CoordinationVersion should be 4.0.0");
            Assert(metadata.MNCVersion == "4.0.0", "MNCVersion should be 4.0.0");
            Assert(metadata.MorePropsVersion == null, "MorePropsVersion should be null");
            Assert(metadata.CompatibilityModeActive == false, "CompatibilityModeActive should be false");
            Assert(metadata.CoordinationEventsCount == 0, "CoordinationEventsCount should be 0");
            
            Console.WriteLine($"  ✓ Deserialized: {metadata}");
            Pass(testName);
        }
        catch (Exception ex)
        {
            Fail(testName, ex.Message);
        }
    }
    
    static void Test_DeserializeWithThirdPartyMods()
    {
        string testName = "Deserialize With Third-Party Mods";
        Console.WriteLine($"\n--- {testName} ---");
        
        try
        {
            string serialized = "v=1;cv=4.0.0;mnc=4.0.0;mp=1.0.7;cm=true;ts=2024-01-15T12:30:00.0000000Z;ec=42;tm=TMPE,MoveIt,NetworkExtensions";
            
            var metadata = CoordinationMetadata.DeserializeMetadata(serialized);
            
            Assert(metadata != null, "Metadata should not be null");
            Assert(metadata.MetadataVersion == 1, "MetadataVersion should be 1");
            Assert(metadata.MorePropsVersion == "1.0.7", "MorePropsVersion should be 1.0.7");
            Assert(metadata.CompatibilityModeActive == true, "CompatibilityModeActive should be true");
            Assert(metadata.CoordinationEventsCount == 42, "CoordinationEventsCount should be 42");
            Assert(metadata.RegisteredThirdPartyMods != null, "RegisteredThirdPartyMods should not be null");
            Assert(metadata.RegisteredThirdPartyMods.Count == 3, "Should have 3 third-party mods");
            Assert(metadata.RegisteredThirdPartyMods[0] == "TMPE", "First mod should be TMPE");
            Assert(metadata.RegisteredThirdPartyMods[1] == "MoveIt", "Second mod should be MoveIt");
            Assert(metadata.RegisteredThirdPartyMods[2] == "NetworkExtensions", "Third mod should be NetworkExtensions");
            
            Console.WriteLine($"  ✓ Deserialized with mods: {metadata}");
            Pass(testName);
        }
        catch (Exception ex)
        {
            Fail(testName, ex.Message);
        }
    }
    
    static void Test_RoundTripProperty_Standalone()
    {
        string testName = "Round-Trip Property (Standalone Mode)";
        Console.WriteLine($"\n--- {testName} ---");
        
        try
        {
            var original = new CoordinationMetadata
            {
                MetadataVersion = 1,
                CoordinationVersion = "4.0.0",
                MNCVersion = "4.0.0",
                MorePropsVersion = null,
                CompatibilityModeActive = false,
                LastSaveTimestamp = new DateTime(2024, 1, 15, 12, 30, 0, DateTimeKind.Utc),
                CoordinationEventsCount = 0,
                RegisteredThirdPartyMods = new List<string>()
            };
            
            bool valid = original.ValidateRoundTrip();
            Assert(valid, "Round-trip validation should pass for standalone metadata");
            
            // Manual verification
            string serialized = original.SerializeMetadata();
            var deserialized = CoordinationMetadata.DeserializeMetadata(serialized);
            
            Assert(deserialized.MetadataVersion == original.MetadataVersion, "MetadataVersion should match");
            Assert(deserialized.CoordinationVersion == original.CoordinationVersion, "CoordinationVersion should match");
            Assert(deserialized.MNCVersion == original.MNCVersion, "MNCVersion should match");
            Assert(deserialized.MorePropsVersion == original.MorePropsVersion, "MorePropsVersion should match");
            Assert(deserialized.CompatibilityModeActive == original.CompatibilityModeActive, "CompatibilityModeActive should match");
            Assert(deserialized.CoordinationEventsCount == original.CoordinationEventsCount, "CoordinationEventsCount should match");
            
            Console.WriteLine("  ✓ Round-trip validation passed for standalone mode");
            Pass(testName);
        }
        catch (Exception ex)
        {
            Fail(testName, ex.Message);
        }
    }
    
    static void Test_RoundTripProperty_CompatibilityMode()
    {
        string testName = "Round-Trip Property (Compatibility Mode)";
        Console.WriteLine($"\n--- {testName} ---");
        
        try
        {
            var original = new CoordinationMetadata
            {
                MetadataVersion = 1,
                CoordinationVersion = "4.0.0",
                MNCVersion = "4.0.0",
                MorePropsVersion = "1.0.7",
                CompatibilityModeActive = true,
                LastSaveTimestamp = new DateTime(2024, 1, 15, 12, 30, 0, DateTimeKind.Utc),
                CoordinationEventsCount = 25,
                RegisteredThirdPartyMods = new List<string>()
            };
            
            bool valid = original.ValidateRoundTrip();
            Assert(valid, "Round-trip validation should pass for compatibility mode metadata");
            
            Console.WriteLine("  ✓ Round-trip validation passed for compatibility mode");
            Pass(testName);
        }
        catch (Exception ex)
        {
            Fail(testName, ex.Message);
        }
    }
    
    static void Test_RoundTripProperty_WithThirdPartyMods()
    {
        string testName = "Round-Trip Property (With Third-Party Mods)";
        Console.WriteLine($"\n--- {testName} ---");
        
        try
        {
            var original = new CoordinationMetadata
            {
                MetadataVersion = 1,
                CoordinationVersion = "4.0.0",
                MNCVersion = "4.0.0",
                MorePropsVersion = "1.0.7",
                CompatibilityModeActive = true,
                LastSaveTimestamp = new DateTime(2024, 1, 15, 12, 30, 0, DateTimeKind.Utc),
                CoordinationEventsCount = 100,
                RegisteredThirdPartyMods = new List<string> { "TMPE", "MoveIt", "NetworkExtensions", "PlanningTools" }
            };
            
            bool valid = original.ValidateRoundTrip();
            Assert(valid, "Round-trip validation should pass with third-party mods");
            
            // Verify list preservation
            string serialized = original.SerializeMetadata();
            var deserialized = CoordinationMetadata.DeserializeMetadata(serialized);
            
            Assert(deserialized.RegisteredThirdPartyMods.Count == original.RegisteredThirdPartyMods.Count, 
                   "Third-party mod count should match");
            
            for (int i = 0; i < original.RegisteredThirdPartyMods.Count; i++)
            {
                Assert(deserialized.RegisteredThirdPartyMods[i] == original.RegisteredThirdPartyMods[i],
                       $"Mod at index {i} should match");
            }
            
            Console.WriteLine("  ✓ Round-trip validation passed with third-party mods");
            Pass(testName);
        }
        catch (Exception ex)
        {
            Fail(testName, ex.Message);
        }
    }
    
    static void Test_DeserializeInvalidData()
    {
        string testName = "Deserialize Invalid Data";
        Console.WriteLine($"\n--- {testName} ---");
        
        try
        {
            // Test with malformed data
            string invalid1 = "invalid=data;format";
            var metadata1 = CoordinationMetadata.DeserializeMetadata(invalid1);
            Assert(metadata1 != null, "Should return metadata object even with invalid format");
            Assert(metadata1.MetadataVersion == 1, "Should use default version");
            
            // Test with partial data
            string invalid2 = "v=1;cv=4.0.0";
            var metadata2 = CoordinationMetadata.DeserializeMetadata(invalid2);
            Assert(metadata2 != null, "Should handle partial data");
            Assert(metadata2.MetadataVersion == 1, "Should parse version");
            Assert(metadata2.CoordinationVersion == "4.0.0", "Should parse coordination version");
            
            Console.WriteLine("  ✓ Handled invalid data gracefully");
            Pass(testName);
        }
        catch (Exception ex)
        {
            Fail(testName, ex.Message);
        }
    }
    
    static void Test_DeserializeEmptyData()
    {
        string testName = "Deserialize Empty/Null Data";
        Console.WriteLine($"\n--- {testName} ---");
        
        try
        {
            var metadata1 = CoordinationMetadata.DeserializeMetadata(null);
            Assert(metadata1 == null, "Should return null for null input");
            
            var metadata2 = CoordinationMetadata.DeserializeMetadata("");
            Assert(metadata2 == null, "Should return null for empty input");
            
            var metadata3 = CoordinationMetadata.DeserializeMetadata("   ");
            Assert(metadata3 == null, "Should return null for whitespace input");
            
            Console.WriteLine("  ✓ Handled empty/null data correctly");
            Pass(testName);
        }
        catch (Exception ex)
        {
            Fail(testName, ex.Message);
        }
    }
    
    static void Test_ToStringMethod()
    {
        string testName = "ToString Method";
        Console.WriteLine($"\n--- {testName} ---");
        
        try
        {
            var metadata = new CoordinationMetadata
            {
                MetadataVersion = 1,
                MNCVersion = "4.0.0",
                MorePropsVersion = "1.0.7",
                CompatibilityModeActive = true,
                CoordinationEventsCount = 50,
                RegisteredThirdPartyMods = new List<string> { "TMPE", "MoveIt" }
            };
            
            string str = metadata.ToString();
            
            Assert(!string.IsNullOrEmpty(str), "ToString should not return empty string");
            Assert(str.Contains("CoordinationMetadata"), "Should contain class name");
            Assert(str.Contains("v=1"), "Should contain version");
            Assert(str.Contains("MNC=4.0.0"), "Should contain MNC version");
            Assert(str.Contains("MoreProps=1.0.7"), "Should contain MoreProps version");
            Assert(str.Contains("CompatMode=True"), "Should contain CompatMode flag");
            Assert(str.Contains("Events=50"), "Should contain events count");
            Assert(str.Contains("ThirdPartyMods=2"), "Should contain mod count");
            
            Console.WriteLine($"  ✓ ToString output: {str}");
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

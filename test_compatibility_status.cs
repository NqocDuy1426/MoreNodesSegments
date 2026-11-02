using System;
using System.Reflection;

namespace MoreNetworkCapacityRenew
{
    /// <summary>
    /// Test for CompatibilityStatus data structure and GetCompatibilityStatus API.
    /// Task 10.1: Create CompatibilityStatus data structure and API
    /// 
    /// Requirements tested:
    /// - 10.1: CompatibilityStatus structure with all required fields
    /// - 10.2: GetCompatibilityStatusReport implementation
    /// - 10.3: Serialization lock state reporting
    /// - 10.4: Grid sanitization status reporting
    /// </summary>
    class TestCompatibilityStatus
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===========================================");
            Console.WriteLine("Test: CompatibilityStatus Data Structure");
            Console.WriteLine("===========================================");
            Console.WriteLine();
            
            try
            {
                // Test 1: CompatibilityStatus struct exists
                Console.WriteLine("Test 1: CompatibilityStatus struct exists");
                Type statusType = typeof(CompatibilityStatus);
                if (statusType != null)
                {
                    Console.WriteLine("  ✓ CompatibilityStatus type found");
                }
                else
                {
                    Console.WriteLine("  ✗ CompatibilityStatus type not found");
                    Environment.Exit(1);
                }
                Console.WriteLine();
                
                // Test 2: Verify required fields exist
                Console.WriteLine("Test 2: Verify required fields");
                string[] requiredFields = new string[]
                {
                    "IsCompatibilityModeActive",
                    "MorePropsVersion",
                    "CircuitBreakerFailureCount",
                    "IsCircuitBreakerTripped",
                    "LastUpdate",
                    "LastKnownGoodState"
                };
                
                foreach (string fieldName in requiredFields)
                {
                    FieldInfo field = statusType.GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);
                    if (field != null)
                    {
                        Console.WriteLine($"  ✓ Field '{fieldName}' exists (type: {field.FieldType.Name})");
                    }
                    else
                    {
                        Console.WriteLine($"  ✗ Field '{fieldName}' not found");
                        Environment.Exit(1);
                    }
                }
                Console.WriteLine();
                
                // Test 3: Verify field types
                Console.WriteLine("Test 3: Verify field types");
                FieldInfo isActiveField = statusType.GetField("IsCompatibilityModeActive", BindingFlags.Public | BindingFlags.Instance);
                if (isActiveField != null && isActiveField.FieldType == typeof(bool))
                {
                    Console.WriteLine("  ✓ IsCompatibilityModeActive is bool");
                }
                else
                {
                    Console.WriteLine("  ✗ IsCompatibilityModeActive type mismatch");
                    Environment.Exit(1);
                }
                
                FieldInfo versionField = statusType.GetField("MorePropsVersion", BindingFlags.Public | BindingFlags.Instance);
                if (versionField != null && versionField.FieldType == typeof(string))
                {
                    Console.WriteLine("  ✓ MorePropsVersion is string");
                }
                else
                {
                    Console.WriteLine("  ✗ MorePropsVersion type mismatch");
                    Environment.Exit(1);
                }
                
                FieldInfo failureCountField = statusType.GetField("CircuitBreakerFailureCount", BindingFlags.Public | BindingFlags.Instance);
                if (failureCountField != null && failureCountField.FieldType == typeof(int))
                {
                    Console.WriteLine("  ✓ CircuitBreakerFailureCount is int");
                }
                else
                {
                    Console.WriteLine("  ✗ CircuitBreakerFailureCount type mismatch");
                    Environment.Exit(1);
                }
                
                FieldInfo trippedField = statusType.GetField("IsCircuitBreakerTripped", BindingFlags.Public | BindingFlags.Instance);
                if (trippedField != null && trippedField.FieldType == typeof(bool))
                {
                    Console.WriteLine("  ✓ IsCircuitBreakerTripped is bool");
                }
                else
                {
                    Console.WriteLine("  ✗ IsCircuitBreakerTripped type mismatch");
                    Environment.Exit(1);
                }
                
                FieldInfo lastUpdateField = statusType.GetField("LastUpdate", BindingFlags.Public | BindingFlags.Instance);
                if (lastUpdateField != null && lastUpdateField.FieldType == typeof(DateTime))
                {
                    Console.WriteLine("  ✓ LastUpdate is DateTime");
                }
                else
                {
                    Console.WriteLine("  ✗ LastUpdate type mismatch");
                    Environment.Exit(1);
                }
                
                FieldInfo lastGoodStateField = statusType.GetField("LastKnownGoodState", BindingFlags.Public | BindingFlags.Instance);
                if (lastGoodStateField != null && lastGoodStateField.FieldType == typeof(string))
                {
                    Console.WriteLine("  ✓ LastKnownGoodState is string");
                }
                else
                {
                    Console.WriteLine("  ✗ LastKnownGoodState type mismatch");
                    Environment.Exit(1);
                }
                Console.WriteLine();
                
                // Test 4: GetCompatibilityStatus method exists
                Console.WriteLine("Test 4: GetCompatibilityStatus method exists");
                Type coordManagerType = typeof(CoordinationManager);
                MethodInfo getStatusMethod = coordManagerType.GetMethod("GetCompatibilityStatus", 
                    BindingFlags.Public | BindingFlags.Static);
                
                if (getStatusMethod != null && getStatusMethod.ReturnType == statusType)
                {
                    Console.WriteLine("  ✓ GetCompatibilityStatus method found (returns CompatibilityStatus)");
                }
                else
                {
                    Console.WriteLine("  ✗ GetCompatibilityStatus method missing or wrong signature");
                    Environment.Exit(1);
                }
                Console.WriteLine();
                
                // Test 5: GetCompatibilityStatusReport method exists
                Console.WriteLine("Test 5: GetCompatibilityStatusReport method exists");
                MethodInfo getReportMethod = coordManagerType.GetMethod("GetCompatibilityStatusReport", 
                    BindingFlags.Public | BindingFlags.Static);
                
                if (getReportMethod != null && getReportMethod.ReturnType == typeof(string))
                {
                    Console.WriteLine("  ✓ GetCompatibilityStatusReport method found (returns string)");
                }
                else
                {
                    Console.WriteLine("  ✗ GetCompatibilityStatusReport method missing or wrong signature");
                    Environment.Exit(1);
                }
                Console.WriteLine();
                
                // Test 6: Verify GetCompatibilityStatus signature and return type
                Console.WriteLine("Test 6: Verify GetCompatibilityStatus signature");
                if (getStatusMethod != null)
                {
                    Console.WriteLine("  ✓ Method is static and public");
                    Console.WriteLine($"  ✓ Return type: {getStatusMethod.ReturnType.Name}");
                    
                    ParameterInfo[] parameters = getStatusMethod.GetParameters();
                    if (parameters.Length == 0)
                    {
                        Console.WriteLine("  ✓ Method takes no parameters");
                    }
                    else
                    {
                        Console.WriteLine($"  ✗ Method should take no parameters, but has {parameters.Length}");
                        Environment.Exit(1);
                    }
                }
                Console.WriteLine();
                
                // Test 7: Verify GetCompatibilityStatusReport signature
                Console.WriteLine("Test 7: Verify GetCompatibilityStatusReport signature");
                if (getReportMethod != null)
                {
                    Console.WriteLine("  ✓ Method is static and public");
                    Console.WriteLine($"  ✓ Return type: {getReportMethod.ReturnType.Name}");
                    
                    ParameterInfo[] parameters = getReportMethod.GetParameters();
                    if (parameters.Length == 0)
                    {
                        Console.WriteLine("  ✓ Method takes no parameters");
                    }
                    else
                    {
                        Console.WriteLine($"  ✗ Method should take no parameters, but has {parameters.Length}");
                        Environment.Exit(1);
                    }
                }
                Console.WriteLine();
                
                // Test 8: Verify SaveCoordinator status properties exist
                Console.WriteLine("Test 8: Verify SaveCoordinator status properties");
                Type saveCoordType = Type.GetType("MoreNetworkCapacityRenew.SaveCoordinator");
                if (saveCoordType != null)
                {
                    PropertyInfo isLockedProp = saveCoordType.GetProperty("IsLocked", BindingFlags.Public | BindingFlags.Static);
                    PropertyInfo currentOwnerProp = saveCoordType.GetProperty("CurrentLockOwner", BindingFlags.Public | BindingFlags.Static);
                    PropertyInfo concurrentSaveProp = saveCoordType.GetProperty("IsConcurrentSaveDetected", BindingFlags.Public | BindingFlags.Static);
                    
                    if (isLockedProp != null && isLockedProp.PropertyType == typeof(bool))
                    {
                        Console.WriteLine("  ✓ IsLocked property exists (bool)");
                    }
                    else
                    {
                        Console.WriteLine("  ✗ IsLocked property missing or wrong type");
                        Environment.Exit(1);
                    }
                    
                    if (currentOwnerProp != null && currentOwnerProp.PropertyType == typeof(string))
                    {
                        Console.WriteLine("  ✓ CurrentLockOwner property exists (string)");
                    }
                    else
                    {
                        Console.WriteLine("  ✗ CurrentLockOwner property missing or wrong type");
                        Environment.Exit(1);
                    }
                    
                    if (concurrentSaveProp != null && concurrentSaveProp.PropertyType == typeof(bool))
                    {
                        Console.WriteLine("  ✓ IsConcurrentSaveDetected property exists (bool)");
                    }
                    else
                    {
                        Console.WriteLine("  ✗ IsConcurrentSaveDetected property missing or wrong type");
                        Environment.Exit(1);
                    }
                }
                else
                {
                    Console.WriteLine("  ✗ SaveCoordinator class not found");
                    Environment.Exit(1);
                }
                Console.WriteLine();
                
                // Test 9: Verify GridCoordinator status properties exist
                Console.WriteLine("Test 9: Verify GridCoordinator status properties");
                Type gridCoordType = Type.GetType("MoreNetworkCapacityRenew.GridCoordinator");
                if (gridCoordType != null)
                {
                    PropertyInfo sanitizationScheduledProp = gridCoordType.GetProperty("IsSanitizationScheduled", BindingFlags.Public | BindingFlags.Static);
                    PropertyInfo morePropsReadyProp = gridCoordType.GetProperty("IsMorePropsReady", BindingFlags.Public | BindingFlags.Static);
                    PropertyInfo eventRegSuccessProp = gridCoordType.GetProperty("IsEventRegistrationSucceeded", BindingFlags.Public | BindingFlags.Static);
                    
                    if (sanitizationScheduledProp != null && sanitizationScheduledProp.PropertyType == typeof(bool))
                    {
                        Console.WriteLine("  ✓ IsSanitizationScheduled property exists (bool)");
                    }
                    else
                    {
                        Console.WriteLine("  ✗ IsSanitizationScheduled property missing or wrong type");
                        Environment.Exit(1);
                    }
                    
                    if (morePropsReadyProp != null && morePropsReadyProp.PropertyType == typeof(bool))
                    {
                        Console.WriteLine("  ✓ IsMorePropsReady property exists (bool)");
                    }
                    else
                    {
                        Console.WriteLine("  ✗ IsMorePropsReady property missing or wrong type");
                        Environment.Exit(1);
                    }
                    
                    if (eventRegSuccessProp != null && eventRegSuccessProp.PropertyType == typeof(bool))
                    {
                        Console.WriteLine("  ✓ IsEventRegistrationSucceeded property exists (bool)");
                    }
                    else
                    {
                        Console.WriteLine("  ✗ IsEventRegistrationSucceeded property missing or wrong type");
                        Environment.Exit(1);
                    }
                }
                else
                {
                    Console.WriteLine("  ✗ GridCoordinator class not found");
                    Environment.Exit(1);
                }
                Console.WriteLine();
                
                Console.WriteLine("===========================================");
                Console.WriteLine("✅ ALL TESTS PASSED");
                Console.WriteLine("===========================================");
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("===========================================");
                Console.WriteLine($"❌ TEST FAILED: {ex.Message}");
                Console.WriteLine("===========================================");
                Environment.Exit(1);
            }
        }
    }
}

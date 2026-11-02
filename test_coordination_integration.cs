// Integration test for CoordinationManager
// Tests the actual compiled CoordinationManager class

using System;
using System.Reflection;
using System.IO;

public class TestCoordinationIntegration
{
    public static void Main()
    {
        Console.WriteLine("=== CoordinationManager Integration Test ===");
        Console.WriteLine();
        
        try
        {
            // Load the compiled DLL
            string dllPath = Path.Combine(Directory.GetCurrentDirectory(), "MoreNodesSegmentsBeta.dll");
            if (!File.Exists(dllPath))
            {
                dllPath = Path.Combine(Directory.GetCurrentDirectory(), "content_beta", "MoreNodesSegmentsBeta.dll");
            }
            
            if (!File.Exists(dllPath))
            {
                Console.WriteLine($"❌ DLL not found: {dllPath}");
                Environment.Exit(1);
            }
            
            Console.WriteLine($"Loading DLL: {dllPath}");
            Assembly asm = Assembly.LoadFrom(dllPath);
            Console.WriteLine($"✓ DLL loaded: {asm.GetName().Name} v{asm.GetName().Version}");
            Console.WriteLine();
            
            // Find CoordinationManager class
            Type coordManagerType = null;
            foreach (Type type in asm.GetTypes())
            {
                if (type.Name == "CoordinationManager")
                {
                    coordManagerType = type;
                    break;
                }
            }
            
            if (coordManagerType == null)
            {
                Console.WriteLine("❌ CoordinationManager class not found in assembly");
                Environment.Exit(1);
            }
            
            Console.WriteLine($"✓ Found CoordinationManager: {coordManagerType.FullName}");
            Console.WriteLine();
            
            // Test 1: Check IsMorePropsActive property exists
            Console.WriteLine("Test 1: IsMorePropsActive property");
            PropertyInfo isActiveProperty = coordManagerType.GetProperty("IsMorePropsActive", 
                BindingFlags.Public | BindingFlags.Static);
            
            if (isActiveProperty != null && isActiveProperty.PropertyType == typeof(bool))
            {
                Console.WriteLine("  ✓ IsMorePropsActive property found (bool)");
            }
            else
            {
                Console.WriteLine("  ✗ IsMorePropsActive property missing or wrong type");
                Environment.Exit(1);
            }
            Console.WriteLine();
            
            // Test 2: Check MorePropsVersion property exists
            Console.WriteLine("Test 2: MorePropsVersion property");
            PropertyInfo versionProperty = coordManagerType.GetProperty("MorePropsVersion", 
                BindingFlags.Public | BindingFlags.Static);
            
            if (versionProperty != null && versionProperty.PropertyType == typeof(string))
            {
                Console.WriteLine("  ✓ MorePropsVersion property found (string)");
            }
            else
            {
                Console.WriteLine("  ✗ MorePropsVersion property missing or wrong type");
                Environment.Exit(1);
            }
            Console.WriteLine();
            
            // Test 3: Check Initialize method exists
            Console.WriteLine("Test 3: Initialize method");
            MethodInfo initMethod = coordManagerType.GetMethod("Initialize", 
                BindingFlags.Public | BindingFlags.Static);
            
            if (initMethod != null && initMethod.ReturnType == typeof(void))
            {
                Console.WriteLine("  ✓ Initialize method found (void)");
            }
            else
            {
                Console.WriteLine("  ✗ Initialize method missing or wrong signature");
                Environment.Exit(1);
            }
            Console.WriteLine();
            
            // Test 4: Check GetExtendedPropManagerType method exists
            Console.WriteLine("Test 4: GetExtendedPropManagerType method");
            MethodInfo getTypeMethod = coordManagerType.GetMethod("GetExtendedPropManagerType", 
                BindingFlags.Public | BindingFlags.Static);
            
            if (getTypeMethod != null && getTypeMethod.ReturnType == typeof(Type))
            {
                Console.WriteLine("  ✓ GetExtendedPropManagerType method found (Type)");
            }
            else
            {
                Console.WriteLine("  ✗ GetExtendedPropManagerType method missing or wrong signature");
                Environment.Exit(1);
            }
            Console.WriteLine();
            
            // Test 5: Check circuit breaker methods
            Console.WriteLine("Test 5: Circuit breaker methods");
            MethodInfo incrementFailureMethod = coordManagerType.GetMethod("IncrementFailureCount", 
                BindingFlags.Public | BindingFlags.Static);
            MethodInfo isTrippedMethod = coordManagerType.GetMethod("IsCircuitBreakerTripped", 
                BindingFlags.Public | BindingFlags.Static);
            
            if (incrementFailureMethod != null && isTrippedMethod != null)
            {
                Console.WriteLine("  ✓ IncrementFailureCount method found");
                Console.WriteLine("  ✓ IsCircuitBreakerTripped method found");
            }
            else
            {
                Console.WriteLine("  ✗ Circuit breaker methods missing");
                Environment.Exit(1);
            }
            Console.WriteLine();
            
            // Test 6: Check Shutdown method
            Console.WriteLine("Test 6: Shutdown method");
            MethodInfo shutdownMethod = coordManagerType.GetMethod("Shutdown", 
                BindingFlags.Public | BindingFlags.Static);
            
            if (shutdownMethod != null && shutdownMethod.ReturnType == typeof(void))
            {
                Console.WriteLine("  ✓ Shutdown method found (void)");
            }
            else
            {
                Console.WriteLine("  ✗ Shutdown method missing or wrong signature");
                Environment.Exit(1);
            }
            Console.WriteLine();
            
            // Test 7: Check GetCompatibilityStatusReport method
            Console.WriteLine("Test 7: GetCompatibilityStatusReport method");
            MethodInfo statusMethod = coordManagerType.GetMethod("GetCompatibilityStatusReport", 
                BindingFlags.Public | BindingFlags.Static);
            
            if (statusMethod != null && statusMethod.ReturnType == typeof(string))
            {
                Console.WriteLine("  ✓ GetCompatibilityStatusReport method found (string)");
            }
            else
            {
                Console.WriteLine("  ✗ GetCompatibilityStatusReport method missing or wrong signature");
                Environment.Exit(1);
            }
            Console.WriteLine();
            
            // Summary
            Console.WriteLine("=================================");
            Console.WriteLine("✅ ALL INTEGRATION TESTS PASSED!");
            Console.WriteLine();
            Console.WriteLine("Verified CoordinationManager has:");
            Console.WriteLine("  • IsMorePropsActive property (Req 1.6)");
            Console.WriteLine("  • MorePropsVersion property (Req 1.3)");
            Console.WriteLine("  • Initialize method (Req 1.1-1.5)");
            Console.WriteLine("  • Type accessor methods (Req 1.5)");
            Console.WriteLine("  • Circuit breaker pattern (Req 8.7)");
            Console.WriteLine("  • Shutdown method");
            Console.WriteLine("  • Status reporting method");
            Console.WriteLine();
            
            Environment.Exit(0);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ INTEGRATION TEST FAILED: {ex}");
            Environment.Exit(1);
        }
    }
}

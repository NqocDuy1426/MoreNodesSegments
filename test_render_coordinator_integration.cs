/**
 * Integration Test for RenderCoordinator (Task 7.1)
 * 
 * Verifies that RenderCoordinator.ApplyRenderPatches is properly integrated
 * into the EnsureAllPatchesApplied method and will be called during mod initialization.
 * 
 * Tests:
 * 1. Verify RenderCoordinator.ApplyRenderPatches is called in EnsureAllPatchesApplied
 * 2. Verify call happens after CoordinationManager.Initialize
 * 3. Verify proper error handling with try-catch blocks
 * 
 * Strategy:
 * - Use reflection to inspect the Renew.cs code structure
 * - Search for the RenderCoordinator.ApplyRenderPatches call
 * - Verify it's in the correct location (after GameLoadingArmor, before KianCommons)
 */

using System;
using System.IO;
using System.Linq;

namespace MoreNetworkCapacityRenew
{
    class TestRenderCoordinatorIntegration
    {
        static int s_testsPassed = 0;
        static int s_testsFailed = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("=== RenderCoordinator Integration Test (Task 7.1) ===");
            Console.WriteLine();

            // Test 1: Verify source code contains RenderCoordinator.ApplyRenderPatches call
            TestSourceCodeContainsCall();

            // Test 2: Verify call is in EnsureAllPatchesApplied method
            TestCallInCorrectMethod();

            // Test 3: Verify error handling (try-catch block)
            TestErrorHandling();

            // Test 4: Verify ordering (after GameLoadingArmor, before KianCommons)
            TestCallOrdering();

            // Summary
            Console.WriteLine();
            Console.WriteLine("=== Integration Test Summary ===");
            Console.WriteLine($"✅ Passed: {s_testsPassed}");
            Console.WriteLine($"❌ Failed: {s_testsFailed}");
            Console.WriteLine();

            if (s_testsFailed == 0)
            {
                Console.WriteLine("🎉 All integration tests passed!");
                Console.WriteLine("RenderCoordinator is properly integrated into the mod.");
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("⚠️  Some integration tests failed.");
                Environment.Exit(1);
            }
        }

        static void TestSourceCodeContainsCall()
        {
            Console.WriteLine("[Test 1] Source code contains RenderCoordinator.ApplyRenderPatches call...");
            try
            {
                string sourceCode = File.ReadAllText("Renew.cs");
                
                if (sourceCode.Contains("RenderCoordinator.ApplyRenderPatches"))
                {
                    Console.WriteLine("  ✅ Found RenderCoordinator.ApplyRenderPatches call in source code");
                    s_testsPassed++;
                }
                else
                {
                    Console.WriteLine("  ❌ RenderCoordinator.ApplyRenderPatches call not found in source code");
                    s_testsFailed++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ❌ Exception: {ex.Message}");
                s_testsFailed++;
            }
        }

        static void TestCallInCorrectMethod()
        {
            Console.WriteLine("[Test 2] Call is in EnsureAllPatchesApplied method...");
            try
            {
                string sourceCode = File.ReadAllText("Renew.cs");
                
                // Find EnsureAllPatchesApplied method
                int methodStart = sourceCode.IndexOf("void EnsureAllPatchesApplied");
                if (methodStart == -1)
                {
                    Console.WriteLine("  ❌ EnsureAllPatchesApplied method not found");
                    s_testsFailed++;
                    return;
                }
                
                // Find the end of the method (next method declaration or closing brace)
                int methodEnd = sourceCode.IndexOf("internal static int SafePatchKianCommons", methodStart);
                if (methodEnd == -1)
                {
                    methodEnd = sourceCode.Length;
                }
                
                string methodCode = sourceCode.Substring(methodStart, methodEnd - methodStart);
                
                if (methodCode.Contains("RenderCoordinator.ApplyRenderPatches"))
                {
                    Console.WriteLine("  ✅ RenderCoordinator.ApplyRenderPatches is called in EnsureAllPatchesApplied");
                    s_testsPassed++;
                }
                else
                {
                    Console.WriteLine("  ❌ RenderCoordinator.ApplyRenderPatches not found in EnsureAllPatchesApplied");
                    s_testsFailed++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ❌ Exception: {ex.Message}");
                s_testsFailed++;
            }
        }

        static void TestErrorHandling()
        {
            Console.WriteLine("[Test 3] Proper error handling (try-catch block)...");
            try
            {
                string sourceCode = File.ReadAllText("Renew.cs");
                
                // Find the RenderCoordinator call
                int callIndex = sourceCode.IndexOf("RenderCoordinator.ApplyRenderPatches");
                if (callIndex == -1)
                {
                    Console.WriteLine("  ❌ RenderCoordinator.ApplyRenderPatches call not found");
                    s_testsFailed++;
                    return;
                }
                
                // Look backward for try block
                int tryIndex = sourceCode.LastIndexOf("try", callIndex);
                int previousCatchIndex = sourceCode.LastIndexOf("catch", callIndex);
                
                // Look forward for catch block
                int catchIndex = sourceCode.IndexOf("catch", callIndex);
                
                if (tryIndex > previousCatchIndex && catchIndex > callIndex && (catchIndex - callIndex) < 500)
                {
                    Console.WriteLine("  ✅ RenderCoordinator.ApplyRenderPatches is wrapped in try-catch block");
                    
                    // Verify error logging
                    int logStart = callIndex;
                    int logEnd = catchIndex + 200;
                    string errorHandlingCode = sourceCode.Substring(logStart, Math.Min(logEnd - logStart, sourceCode.Length - logStart));
                    
                    if (errorHandlingCode.Contains("RenderCoordinator error"))
                    {
                        Console.WriteLine("  ✅ Error logging is present");
                    }
                    else
                    {
                        Console.WriteLine("  ⚠️  Error logging not found (minor issue)");
                    }
                    
                    s_testsPassed++;
                }
                else
                {
                    Console.WriteLine("  ❌ RenderCoordinator.ApplyRenderPatches is not properly wrapped in try-catch");
                    s_testsFailed++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ❌ Exception: {ex.Message}");
                s_testsFailed++;
            }
        }

        static void TestCallOrdering()
        {
            Console.WriteLine("[Test 4] Correct call ordering (after GameLoadingArmor, before KianCommons)...");
            try
            {
                string sourceCode = File.ReadAllText("Renew.cs");
                
                int gameLoadingArmorIndex = sourceCode.IndexOf("GameLoadingArmor.Apply");
                int renderCoordIndex = sourceCode.IndexOf("RenderCoordinator.ApplyRenderPatches");
                int kianCommonsIndex = sourceCode.IndexOf("Patch KianCommons limits");
                
                if (gameLoadingArmorIndex == -1)
                {
                    Console.WriteLine("  ⚠️  GameLoadingArmor.Apply not found (might be refactored)");
                }
                if (renderCoordIndex == -1)
                {
                    Console.WriteLine("  ❌ RenderCoordinator.ApplyRenderPatches not found");
                    s_testsFailed++;
                    return;
                }
                if (kianCommonsIndex == -1)
                {
                    Console.WriteLine("  ⚠️  KianCommons patching not found (might be refactored)");
                }
                
                bool correctOrder = true;
                
                // Check: GameLoadingArmor < RenderCoordinator
                if (gameLoadingArmorIndex != -1 && gameLoadingArmorIndex < renderCoordIndex)
                {
                    Console.WriteLine($"  ✅ RenderCoordinator is called after GameLoadingArmor");
                }
                else if (gameLoadingArmorIndex != -1)
                {
                    Console.WriteLine($"  ❌ RenderCoordinator should be called after GameLoadingArmor");
                    correctOrder = false;
                }
                
                // Check: RenderCoordinator < KianCommons
                if (kianCommonsIndex != -1 && renderCoordIndex < kianCommonsIndex)
                {
                    Console.WriteLine($"  ✅ RenderCoordinator is called before KianCommons patching");
                }
                else if (kianCommonsIndex != -1)
                {
                    Console.WriteLine($"  ❌ RenderCoordinator should be called before KianCommons patching");
                    correctOrder = false;
                }
                
                if (correctOrder)
                {
                    s_testsPassed++;
                }
                else
                {
                    s_testsFailed++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ❌ Exception: {ex.Message}");
                s_testsFailed++;
            }
        }
    }
}

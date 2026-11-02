using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Test suite for third-party mod adapter API (Task 9.2).
/// Tests adapter registration, duplicate detection, cleanup, and event integration.
/// Requirements: 12.2, 12.3, 12.4, 12.5, 12.6, 12.8
/// </summary>
public class AdapterAPITests
{
    private static int s_testsPassed = 0;
    private static int s_testsFailed = 0;
    
    public static void RunAllTests()
    {
        Debug.Log("========================================");
        Debug.Log("ADAPTER API TEST SUITE");
        Debug.Log("========================================");
        
        s_testsPassed = 0;
        s_testsFailed = 0;
        
        TestAdapterRegistration();
        TestDuplicateDetection();
        TestAdapterLimit();
        TestAdapterCleanup();
        TestEventIntegration();
        TestNullAdapter();
        TestEmptyName();
        
        Debug.Log("========================================");
        Debug.Log($"ADAPTER API TESTS COMPLETE: {s_testsPassed} passed, {s_testsFailed} failed");
        Debug.Log("========================================");
    }
    
    /// <summary>
    /// Test: Adapter registration works correctly (Req 12.3, 12.4).
    /// </summary>
    private static void TestAdapterRegistration()
    {
        Debug.Log("\n[TEST] Adapter Registration");
        
        try
        {
            // Create and register a test adapter
            var adapter = new TestModAdapter("TestMod1");
            CoordinationManager.RegisterModForCoordination(adapter);
            
            // Verify registration
            string[] registered = CoordinationManager.GetRegisteredAdapters();
            if (registered.Length == 1 && registered[0] == "TestMod1")
            {
                Debug.Log("[PASS] Adapter registered successfully");
                s_testsPassed++;
            }
            else
            {
                Debug.LogError($"[FAIL] Expected 1 adapter, got {registered.Length}");
                s_testsFailed++;
            }
            
            // Cleanup for next test
            CoordinationManager.Shutdown();
        }
        catch (Exception ex)
        {
            Debug.LogError($"[FAIL] Adapter registration test failed: {ex}");
            s_testsFailed++;
        }
    }
    
    /// <summary>
    /// Test: Duplicate adapter registration is detected and rejected (Req 12.4).
    /// </summary>
    private static void TestDuplicateDetection()
    {
        Debug.Log("\n[TEST] Duplicate Detection");
        
        try
        {
            // Register first adapter
            var adapter1 = new TestModAdapter("DuplicateMod");
            CoordinationManager.RegisterModForCoordination(adapter1);
            
            // Try to register duplicate
            var adapter2 = new TestModAdapter("DuplicateMod");
            bool duplicateDetected = false;
            
            try
            {
                CoordinationManager.RegisterModForCoordination(adapter2);
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("already registered"))
                {
                    duplicateDetected = true;
                }
            }
            
            if (duplicateDetected)
            {
                Debug.Log("[PASS] Duplicate adapter detected and rejected");
                s_testsPassed++;
            }
            else
            {
                Debug.LogError("[FAIL] Duplicate adapter was not detected");
                s_testsFailed++;
            }
            
            // Cleanup
            CoordinationManager.Shutdown();
        }
        catch (Exception ex)
        {
            Debug.LogError($"[FAIL] Duplicate detection test failed: {ex}");
            s_testsFailed++;
        }
    }
    
    /// <summary>
    /// Test: Adapter limit (32 max) is enforced (Req 12.7).
    /// </summary>
    private static void TestAdapterLimit()
    {
        Debug.Log("\n[TEST] Adapter Limit Enforcement");
        
        try
        {
            // Register maximum adapters (32)
            for (int i = 0; i < 32; i++)
            {
                var adapter = new TestModAdapter($"Mod{i}");
                CoordinationManager.RegisterModForCoordination(adapter);
            }
            
            // Try to register one more
            bool limitEnforced = false;
            try
            {
                var overflowAdapter = new TestModAdapter("Mod33");
                CoordinationManager.RegisterModForCoordination(overflowAdapter);
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("Maximum") && ex.Message.Contains("32"))
                {
                    limitEnforced = true;
                }
            }
            
            if (limitEnforced)
            {
                Debug.Log("[PASS] Adapter limit (32) enforced correctly");
                s_testsPassed++;
            }
            else
            {
                Debug.LogError("[FAIL] Adapter limit was not enforced");
                s_testsFailed++;
            }
            
            // Cleanup
            CoordinationManager.Shutdown();
        }
        catch (Exception ex)
        {
            Debug.LogError($"[FAIL] Adapter limit test failed: {ex}");
            s_testsFailed++;
            CoordinationManager.Shutdown();
        }
    }
    
    /// <summary>
    /// Test: Cleanup is called on all adapters during shutdown (Req 12.8).
    /// </summary>
    private static void TestAdapterCleanup()
    {
        Debug.Log("\n[TEST] Adapter Cleanup");
        
        try
        {
            // Register test adapters
            var adapter1 = new TestModAdapter("CleanupMod1");
            var adapter2 = new TestModAdapter("CleanupMod2");
            CoordinationManager.RegisterModForCoordination(adapter1);
            CoordinationManager.RegisterModForCoordination(adapter2);
            
            // Shutdown should call Cleanup on all adapters
            CoordinationManager.Shutdown();
            
            // Verify cleanup was called
            if (adapter1.CleanupCalled && adapter2.CleanupCalled)
            {
                Debug.Log("[PASS] Cleanup called on all adapters during shutdown");
                s_testsPassed++;
            }
            else
            {
                Debug.LogError($"[FAIL] Cleanup not called: Mod1={adapter1.CleanupCalled}, Mod2={adapter2.CleanupCalled}");
                s_testsFailed++;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[FAIL] Adapter cleanup test failed: {ex}");
            s_testsFailed++;
        }
    }
    
    /// <summary>
    /// Test: Adapters can subscribe to events and receive callbacks (Req 12.2, 12.5).
    /// </summary>
    private static void TestEventIntegration()
    {
        Debug.Log("\n[TEST] Event Integration");
        
        try
        {
            // Register adapter that subscribes to events
            var adapter = new EventTestAdapter();
            CoordinationManager.RegisterModForCoordination(adapter);
            
            // Trigger a save coordination event
            var saveEvent = new SaveCoordinationStartedEvent("TestOwner");
            EventBus.Publish(saveEvent);
            
            // Verify adapter received the event
            if (adapter.SaveStartedReceived)
            {
                Debug.Log("[PASS] Adapter received event callback");
                s_testsPassed++;
            }
            else
            {
                Debug.LogError("[FAIL] Adapter did not receive event callback");
                s_testsFailed++;
            }
            
            // Cleanup
            CoordinationManager.Shutdown();
        }
        catch (Exception ex)
        {
            Debug.LogError($"[FAIL] Event integration test failed: {ex}");
            s_testsFailed++;
        }
    }
    
    /// <summary>
    /// Test: Null adapter is rejected (Req 12.3).
    /// </summary>
    private static void TestNullAdapter()
    {
        Debug.Log("\n[TEST] Null Adapter Rejection");
        
        try
        {
            bool nullRejected = false;
            try
            {
                CoordinationManager.RegisterModForCoordination(null);
            }
            catch (ArgumentNullException)
            {
                nullRejected = true;
            }
            
            if (nullRejected)
            {
                Debug.Log("[PASS] Null adapter rejected");
                s_testsPassed++;
            }
            else
            {
                Debug.LogError("[FAIL] Null adapter was not rejected");
                s_testsFailed++;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[FAIL] Null adapter test failed: {ex}");
            s_testsFailed++;
        }
    }
    
    /// <summary>
    /// Test: Adapter with empty name is rejected (Req 12.3).
    /// </summary>
    private static void TestEmptyName()
    {
        Debug.Log("\n[TEST] Empty Name Rejection");
        
        try
        {
            bool emptyRejected = false;
            try
            {
                var adapter = new TestModAdapter("");
                CoordinationManager.RegisterModForCoordination(adapter);
            }
            catch (ArgumentException ex)
            {
                if (ex.Message.Contains("name"))
                {
                    emptyRejected = true;
                }
            }
            
            if (emptyRejected)
            {
                Debug.Log("[PASS] Empty name adapter rejected");
                s_testsPassed++;
            }
            else
            {
                Debug.LogError("[FAIL] Empty name adapter was not rejected");
                s_testsFailed++;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[FAIL] Empty name test failed: {ex}");
            s_testsFailed++;
        }
    }
}

/// <summary>
/// Test adapter implementation for testing basic functionality.
/// </summary>
public class TestModAdapter : ICompatibilityAdapter
{
    private readonly string _name;
    public bool CleanupCalled { get; private set; }
    public bool RegisterCalled { get; private set; }
    
    public TestModAdapter(string name)
    {
        _name = name;
    }
    
    public string Name => _name;
    
    public void RegisterWithCoordinator()
    {
        RegisterCalled = true;
        Debug.Log($"[{Name}] RegisterWithCoordinator called");
    }
    
    public void Cleanup()
    {
        CleanupCalled = true;
        Debug.Log($"[{Name}] Cleanup called");
    }
}

/// <summary>
/// Test adapter that subscribes to events to verify event integration.
/// </summary>
public class EventTestAdapter : ICompatibilityAdapter
{
    public bool SaveStartedReceived { get; private set; }
    
    public string Name => "EventTestAdapter";
    
    public void RegisterWithCoordinator()
    {
        EventBus.Subscribe<SaveCoordinationStartedEvent>(OnSaveStarted);
        Debug.Log($"[{Name}] Subscribed to SaveCoordinationStartedEvent");
    }
    
    public void Cleanup()
    {
        EventBus.Unsubscribe<SaveCoordinationStartedEvent>(OnSaveStarted);
        Debug.Log($"[{Name}] Unsubscribed from events");
    }
    
    private void OnSaveStarted(SaveCoordinationStartedEvent evt)
    {
        SaveStartedReceived = true;
        Debug.Log($"[{Name}] Received SaveCoordinationStartedEvent from {evt.LockOwner}");
    }
}

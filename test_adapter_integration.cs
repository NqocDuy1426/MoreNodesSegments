using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Integration test demonstrating full adapter API workflow.
/// Shows how a third-party mod would use the adapter API to integrate with coordination.
/// Requirements: 12.2, 12.3, 12.4, 12.5, 12.6, 12.8
/// </summary>
public class AdapterIntegrationTest
{
    public static void RunIntegrationTest()
    {
        Debug.Log("\n========================================");
        Debug.Log("ADAPTER API INTEGRATION TEST");
        Debug.Log("========================================");
        
        try
        {
            // Initialize coordination system
            Debug.Log("\n1. Initializing coordination system...");
            ConfigManager.Initialize();
            CoordinationManager.Initialize();
            
            // Create and register adapters
            Debug.Log("\n2. Registering third-party mod adapters...");
            var tmpeAdapter = new TMPECompatibilityAdapter();
            var moveItAdapter = new MoveItCompatibilityAdapter();
            
            CoordinationManager.RegisterModForCoordination(tmpeAdapter);
            CoordinationManager.RegisterModForCoordination(moveItAdapter);
            
            Debug.Log($"   Registered adapters: {string.Join(", ", CoordinationManager.GetRegisteredAdapters())}");
            
            // Trigger events and verify adapters receive them
            Debug.Log("\n3. Triggering coordination events...");
            
            // Simulate save coordination
            var saveStartEvent = new SaveCoordinationStartedEvent("MNC");
            EventBus.Publish(saveStartEvent);
            
            System.Threading.Thread.Sleep(100); // Allow handlers to execute
            
            var saveCompleteEvent = new SaveCoordinationCompletedEvent(
                success: true, 
                duration: TimeSpan.FromMilliseconds(523), 
                lockOwner: "MNC"
            );
            EventBus.Publish(saveCompleteEvent);
            
            // Simulate grid sanitization
            var gridEvent = new GridSanitizationRequestedEvent("PostMorePropsInit");
            EventBus.Publish(gridEvent);
            
            Debug.Log("\n4. Verifying adapter event reception...");
            if (tmpeAdapter.EventsReceived >= 3 && moveItAdapter.EventsReceived >= 3)
            {
                Debug.Log($"   [✓] TMPE adapter received {tmpeAdapter.EventsReceived} events");
                Debug.Log($"   [✓] MoveIt adapter received {moveItAdapter.EventsReceived} events");
            }
            else
            {
                Debug.LogError($"   [✗] Event reception incomplete: TMPE={tmpeAdapter.EventsReceived}, MoveIt={moveItAdapter.EventsReceived}");
            }
            
            // Cleanup
            Debug.Log("\n5. Shutting down coordination system...");
            CoordinationManager.Shutdown();
            
            Debug.Log("\n========================================");
            Debug.Log("INTEGRATION TEST COMPLETE");
            Debug.Log("========================================");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Integration test failed: {ex}");
        }
    }
}

/// <summary>
/// Example adapter for Traffic Manager: President Edition (TMPE).
/// Demonstrates real-world usage pattern for third-party mod integration.
/// </summary>
public class TMPECompatibilityAdapter : ICompatibilityAdapter
{
    private bool _isRegistered;
    public int EventsReceived { get; private set; }
    
    public string Name => "TrafficManager";
    
    public void RegisterWithCoordinator()
    {
        if (_isRegistered)
        {
            Debug.LogWarning($"[{Name}] Already registered");
            return;
        }
        
        try
        {
            // Subscribe to relevant events
            EventBus.Subscribe<SaveCoordinationStartedEvent>(OnSaveStarted);
            EventBus.Subscribe<SaveCoordinationCompletedEvent>(OnSaveCompleted);
            EventBus.Subscribe<GridSanitizationRequestedEvent>(OnGridSanitization);
            
            _isRegistered = true;
            Debug.Log($"[{Name}] Registered with coordination system");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[{Name}] Registration failed: {ex}");
        }
    }
    
    public void Cleanup()
    {
        if (!_isRegistered)
        {
            return;
        }
        
        try
        {
            EventBus.Unsubscribe<SaveCoordinationStartedEvent>(OnSaveStarted);
            EventBus.Unsubscribe<SaveCoordinationCompletedEvent>(OnSaveCompleted);
            EventBus.Unsubscribe<GridSanitizationRequestedEvent>(OnGridSanitization);
            
            _isRegistered = false;
            Debug.Log($"[{Name}] Cleanup complete");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[{Name}] Cleanup error: {ex}");
        }
    }
    
    private void OnSaveStarted(SaveCoordinationStartedEvent evt)
    {
        EventsReceived++;
        Debug.Log($"[{Name}] Save started by {evt.LockOwner} - pausing TMPE pathfinding");
        // In real implementation: pause pathfinding calculations
    }
    
    private void OnSaveCompleted(SaveCoordinationCompletedEvent evt)
    {
        EventsReceived++;
        Debug.Log($"[{Name}] Save completed ({evt.Success}) in {evt.Duration.TotalMilliseconds}ms - resuming TMPE");
        // In real implementation: resume pathfinding calculations
    }
    
    private void OnGridSanitization(GridSanitizationRequestedEvent evt)
    {
        EventsReceived++;
        Debug.Log($"[{Name}] Grid sanitization requested: {evt.Context} - invalidating TMPE caches");
        // In real implementation: invalidate route caches
    }
}

/// <summary>
/// Example adapter for Move It mod.
/// Demonstrates coordination with object movement and Hot-Swap functionality.
/// </summary>
public class MoveItCompatibilityAdapter : ICompatibilityAdapter
{
    private bool _isRegistered;
    public int EventsReceived { get; private set; }
    
    public string Name => "MoveIt";
    
    public void RegisterWithCoordinator()
    {
        if (_isRegistered)
        {
            Debug.LogWarning($"[{Name}] Already registered");
            return;
        }
        
        try
        {
            // Subscribe to events relevant for object manipulation
            EventBus.Subscribe<SaveCoordinationStartedEvent>(OnSaveStarted);
            EventBus.Subscribe<SaveCoordinationCompletedEvent>(OnSaveCompleted);
            EventBus.Subscribe<GridSanitizationRequestedEvent>(OnGridSanitization);
            
            _isRegistered = true;
            Debug.Log($"[{Name}] Registered with coordination system");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[{Name}] Registration failed: {ex}");
        }
    }
    
    public void Cleanup()
    {
        if (!_isRegistered)
        {
            return;
        }
        
        try
        {
            EventBus.Unsubscribe<SaveCoordinationStartedEvent>(OnSaveStarted);
            EventBus.Unsubscribe<SaveCoordinationCompletedEvent>(OnSaveCompleted);
            EventBus.Unsubscribe<GridSanitizationRequestedEvent>(OnGridSanitization);
            
            _isRegistered = false;
            Debug.Log($"[{Name}] Cleanup complete");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[{Name}] Cleanup error: {ex}");
        }
    }
    
    private void OnSaveStarted(SaveCoordinationStartedEvent evt)
    {
        EventsReceived++;
        Debug.Log($"[{Name}] Save started - disabling object selection");
        // In real implementation: disable object selection during save
    }
    
    private void OnSaveCompleted(SaveCoordinationCompletedEvent evt)
    {
        EventsReceived++;
        Debug.Log($"[{Name}] Save completed - re-enabling object selection");
        // In real implementation: re-enable object selection
    }
    
    private void OnGridSanitization(GridSanitizationRequestedEvent evt)
    {
        EventsReceived++;
        Debug.Log($"[{Name}] Grid sanitization: {evt.Context} - refreshing object positions");
        // In real implementation: refresh cached object positions
    }
}

using System;
using System.Collections.Generic;
using MoreNetworkCapacityRenew;

/// <summary>
/// Test suite for EventBus implementation (Task 9.1).
/// Tests pub/sub pattern, error isolation, and subscriber limits.
/// </summary>
public class EventBusTests
{
    private static int s_testsPassed = 0;
    private static int s_testsFailed = 0;
    private static List<string> s_failures = new List<string>();
    
    public static void RunAllTests()
    {
        Console.WriteLine("========================================");
        Console.WriteLine("EventBus Test Suite");
        Console.WriteLine("========================================\n");
        
        // Reset event bus state
        EventBus.ClearAllSubscriptions();
        
        // Run tests
        TestSubscribeAndPublish();
        TestUnsubscribe();
        TestErrorIsolation();
        TestSubscriberLimit();
        TestMultipleSubscribers();
        TestEventDefinitions();
        TestGetSubscriberCount();
        
        // Summary
        Console.WriteLine("\n========================================");
        Console.WriteLine($"Tests Passed: {s_testsPassed}");
        Console.WriteLine($"Tests Failed: {s_testsFailed}");
        
        if (s_testsFailed > 0)
        {
            Console.WriteLine("\nFailure Details:");
            foreach (var failure in s_failures)
            {
                Console.WriteLine($"  - {failure}");
            }
        }
        else
        {
            Console.WriteLine("\n✅ All tests passed!");
        }
        
        Console.WriteLine("========================================");
    }
    
    private static void TestSubscribeAndPublish()
    {
        string testName = "TestSubscribeAndPublish";
        try
        {
            bool handlerCalled = false;
            string receivedVersion = null;
            
            EventBus.Subscribe<CompatibilityModeActivatedEvent>(evt => {
                handlerCalled = true;
                receivedVersion = evt.MorePropsVersion;
            });
            
            var testEvent = new CompatibilityModeActivatedEvent("1.0.7");
            EventBus.Publish(testEvent);
            
            Assert(handlerCalled, "Handler should be called after publish");
            Assert(receivedVersion == "1.0.7", $"Version should be 1.0.7, got {receivedVersion}");
            Assert(testEvent.Source == "CoordinationManager", "Source should be CoordinationManager");
            
            Pass(testName);
        }
        catch (Exception ex)
        {
            Fail(testName, ex.Message);
        }
        finally
        {
            EventBus.ClearAllSubscriptions();
        }
    }
    
    private static void TestUnsubscribe()
    {
        string testName = "TestUnsubscribe";
        try
        {
            int callCount = 0;
            Action<CompatibilityModeActivatedEvent> handler = evt => { callCount++; };
            
            EventBus.Subscribe(handler);
            EventBus.Publish(new CompatibilityModeActivatedEvent("1.0.7"));
            Assert(callCount == 1, $"Call count should be 1, got {callCount}");
            
            EventBus.Unsubscribe(handler);
            EventBus.Publish(new CompatibilityModeActivatedEvent("1.0.8"));
            Assert(callCount == 1, $"Call count should still be 1 after unsubscribe, got {callCount}");
            
            Pass(testName);
        }
        catch (Exception ex)
        {
            Fail(testName, ex.Message);
        }
        finally
        {
            EventBus.ClearAllSubscriptions();
        }
    }
    
    private static void TestErrorIsolation()
    {
        string testName = "TestErrorIsolation";
        try
        {
            int handler1Called = 0;
            int handler2Called = 0;
            int handler3Called = 0;
            
            EventBus.Subscribe<CompatibilityModeActivatedEvent>(evt => { handler1Called++; });
            EventBus.Subscribe<CompatibilityModeActivatedEvent>(evt => { 
                handler2Called++; 
                throw new Exception("Handler 2 error");
            });
            EventBus.Subscribe<CompatibilityModeActivatedEvent>(evt => { handler3Called++; });
            
            EventBus.Publish(new CompatibilityModeActivatedEvent("1.0.7"));
            
            Assert(handler1Called == 1, "Handler 1 should be called");
            Assert(handler2Called == 1, "Handler 2 should be called despite throwing");
            Assert(handler3Called == 1, "Handler 3 should be called despite handler 2 error");
            
            Pass(testName);
        }
        catch (Exception ex)
        {
            Fail(testName, ex.Message);
        }
        finally
        {
            EventBus.ClearAllSubscriptions();
        }
    }
    
    private static void TestSubscriberLimit()
    {
        string testName = "TestSubscriberLimit";
        try
        {
            // Subscribe 32 handlers (the limit)
            for (int i = 0; i < 32; i++)
            {
                int index = i;
                EventBus.Subscribe<CompatibilityModeActivatedEvent>(evt => { /* handler */ });
            }
            
            Assert(EventBus.GetSubscriberCount<CompatibilityModeActivatedEvent>() == 32, 
                "Should have 32 subscribers");
            
            // Attempt to subscribe 33rd handler (should throw)
            bool exceptionThrown = false;
            try
            {
                EventBus.Subscribe<CompatibilityModeActivatedEvent>(evt => { /* 33rd handler */ });
            }
            catch (InvalidOperationException)
            {
                exceptionThrown = true;
            }
            
            Assert(exceptionThrown, "Should throw exception when exceeding subscriber limit");
            
            Pass(testName);
        }
        catch (Exception ex)
        {
            Fail(testName, ex.Message);
        }
        finally
        {
            EventBus.ClearAllSubscriptions();
        }
    }
    
    private static void TestMultipleSubscribers()
    {
        string testName = "TestMultipleSubscribers";
        try
        {
            int total = 0;
            
            EventBus.Subscribe<SaveCoordinationStartedEvent>(evt => { total += 1; });
            EventBus.Subscribe<SaveCoordinationStartedEvent>(evt => { total += 10; });
            EventBus.Subscribe<SaveCoordinationStartedEvent>(evt => { total += 100; });
            
            EventBus.Publish(new SaveCoordinationStartedEvent("MNC"));
            
            Assert(total == 111, $"Total should be 111 (1+10+100), got {total}");
            
            Pass(testName);
        }
        catch (Exception ex)
        {
            Fail(testName, ex.Message);
        }
        finally
        {
            EventBus.ClearAllSubscriptions();
        }
    }
    
    private static void TestEventDefinitions()
    {
        string testName = "TestEventDefinitions";
        try
        {
            // Test CompatibilityModeActivatedEvent
            var evt1 = new CompatibilityModeActivatedEvent("1.0.7");
            Assert(evt1.MorePropsVersion == "1.0.7", "Version should match");
            Assert(evt1.Source == "CoordinationManager", "Source should be CoordinationManager");
            Assert(evt1.Timestamp != default(DateTime), "Timestamp should be set");
            
            // Test SaveCoordinationStartedEvent
            var evt2 = new SaveCoordinationStartedEvent("MNC");
            Assert(evt2.LockOwner == "MNC", "Lock owner should be MNC");
            Assert(evt2.Source == "SaveCoordinator", "Source should be SaveCoordinator");
            
            // Test SaveCoordinationCompletedEvent
            var duration = TimeSpan.FromSeconds(1.5);
            var evt3 = new SaveCoordinationCompletedEvent(true, duration, "MNC");
            Assert(evt3.Success == true, "Success should be true");
            Assert(evt3.Duration == duration, "Duration should match");
            Assert(evt3.LockOwner == "MNC", "Lock owner should be MNC");
            
            // Test GridSanitizationRequestedEvent
            var evt4 = new GridSanitizationRequestedEvent("PostMorePropsInit");
            Assert(evt4.Context == "PostMorePropsInit", "Context should match");
            Assert(evt4.Source == "GridCoordinator", "Source should be GridCoordinator");
            
            // Test GridSanitizationCompletedEvent
            var evt5 = new GridSanitizationCompletedEvent(5, TimeSpan.FromSeconds(0.5));
            Assert(evt5.CyclesRepaired == 5, "Cycles repaired should be 5");
            Assert(evt5.Duration.TotalSeconds == 0.5, "Duration should be 0.5 seconds");
            
            Pass(testName);
        }
        catch (Exception ex)
        {
            Fail(testName, ex.Message);
        }
    }
    
    private static void TestGetSubscriberCount()
    {
        string testName = "TestGetSubscriberCount";
        try
        {
            Assert(EventBus.GetSubscriberCount<CompatibilityModeActivatedEvent>() == 0, 
                "Should have 0 subscribers initially");
            
            EventBus.Subscribe<CompatibilityModeActivatedEvent>(evt => { });
            Assert(EventBus.GetSubscriberCount<CompatibilityModeActivatedEvent>() == 1, 
                "Should have 1 subscriber");
            
            EventBus.Subscribe<CompatibilityModeActivatedEvent>(evt => { });
            Assert(EventBus.GetSubscriberCount<CompatibilityModeActivatedEvent>() == 2, 
                "Should have 2 subscribers");
            
            Pass(testName);
        }
        catch (Exception ex)
        {
            Fail(testName, ex.Message);
        }
        finally
        {
            EventBus.ClearAllSubscriptions();
        }
    }
    
    // Helper methods
    private static void Assert(bool condition, string message)
    {
        if (!condition)
        {
            throw new Exception($"Assertion failed: {message}");
        }
    }
    
    private static void Pass(string testName)
    {
        s_testsPassed++;
        Console.WriteLine($"✅ {testName}");
    }
    
    private static void Fail(string testName, string reason)
    {
        s_testsFailed++;
        s_failures.Add($"{testName}: {reason}");
        Console.WriteLine($"❌ {testName}: {reason}");
    }
    
    // Entry point for testing
    public static void Main(string[] args)
    {
        RunAllTests();
    }
}

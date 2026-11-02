using System;
using System.Collections.Generic;
using MoreNetworkCapacityRenew;

/// <summary>
/// Simple EventBus test without Unity dependencies.
/// Tests core pub/sub functionality that can run standalone.
/// </summary>
public class SimpleEventBusTest
{
    private static int s_testsPassed = 0;
    private static int s_testsFailed = 0;
    
    public static void Main(string[] args)
    {
        Console.WriteLine("EventBus Simple Test Suite");
        Console.WriteLine("==========================================\n");
        
        TestBasicSubscribePublish();
        TestUnsubscribe();
        TestMultipleHandlers();
        TestErrorIsolation();
        TestEventDataIntegrity();
        
        Console.WriteLine("\n==========================================");
        Console.WriteLine($"PASSED: {s_testsPassed}, FAILED: {s_testsFailed}");
        
        if (s_testsFailed == 0)
        {
            Console.WriteLine("✅ All tests passed!");
        }
        else
        {
            Console.WriteLine("❌ Some tests failed");
            Environment.Exit(1);
        }
    }
    
    static void TestBasicSubscribePublish()
    {
        try
        {
            bool called = false;
            string version = null;
            
            Action<CompatibilityModeActivatedEvent> handler = evt => {
                called = true;
                version = evt.MorePropsVersion;
            };
            
            EventBus.Subscribe(handler);
            EventBus.Publish(new CompatibilityModeActivatedEvent("1.0.7"));
            
            if (!called) throw new Exception("Handler not called");
            if (version != "1.0.7") throw new Exception($"Expected version 1.0.7, got {version}");
            
            EventBus.Unsubscribe(handler);
            Pass("TestBasicSubscribePublish");
        }
        catch (Exception ex)
        {
            Fail("TestBasicSubscribePublish", ex.Message);
        }
    }
    
    static void TestUnsubscribe()
    {
        try
        {
            int count = 0;
            Action<CompatibilityModeActivatedEvent> handler = evt => { count++; };
            
            EventBus.Subscribe(handler);
            EventBus.Publish(new CompatibilityModeActivatedEvent("1.0.7"));
            if (count != 1) throw new Exception($"Expected count 1, got {count}");
            
            EventBus.Unsubscribe(handler);
            EventBus.Publish(new CompatibilityModeActivatedEvent("1.0.8"));
            if (count != 1) throw new Exception($"Expected count still 1 after unsubscribe, got {count}");
            
            Pass("TestUnsubscribe");
        }
        catch (Exception ex)
        {
            Fail("TestUnsubscribe", ex.Message);
        }
    }
    
    static void TestMultipleHandlers()
    {
        try
        {
            int total = 0;
            
            Action<SaveCoordinationStartedEvent> h1 = evt => { total += 1; };
            Action<SaveCoordinationStartedEvent> h2 = evt => { total += 10; };
            Action<SaveCoordinationStartedEvent> h3 = evt => { total += 100; };
            
            EventBus.Subscribe(h1);
            EventBus.Subscribe(h2);
            EventBus.Subscribe(h3);
            
            EventBus.Publish(new SaveCoordinationStartedEvent("MNC"));
            
            if (total != 111) throw new Exception($"Expected total 111, got {total}");
            
            EventBus.Unsubscribe(h1);
            EventBus.Unsubscribe(h2);
            EventBus.Unsubscribe(h3);
            
            Pass("TestMultipleHandlers");
        }
        catch (Exception ex)
        {
            Fail("TestMultipleHandlers", ex.Message);
        }
    }
    
    static void TestErrorIsolation()
    {
        try
        {
            int h1Count = 0, h2Count = 0, h3Count = 0;
            
            Action<CompatibilityModeActivatedEvent> h1 = evt => { h1Count++; };
            Action<CompatibilityModeActivatedEvent> h2 = evt => { 
                h2Count++; 
                throw new Exception("Intentional error");
            };
            Action<CompatibilityModeActivatedEvent> h3 = evt => { h3Count++; };
            
            EventBus.Subscribe(h1);
            EventBus.Subscribe(h2);
            EventBus.Subscribe(h3);
            
            // Should not throw - errors are isolated
            EventBus.Publish(new CompatibilityModeActivatedEvent("1.0.7"));
            
            if (h1Count != 1) throw new Exception($"H1 expected 1, got {h1Count}");
            if (h2Count != 1) throw new Exception($"H2 expected 1, got {h2Count}");
            if (h3Count != 1) throw new Exception($"H3 expected 1, got {h3Count}");
            
            EventBus.Unsubscribe(h1);
            EventBus.Unsubscribe(h2);
            EventBus.Unsubscribe(h3);
            
            Pass("TestErrorIsolation");
        }
        catch (Exception ex)
        {
            Fail("TestErrorIsolation", ex.Message);
        }
    }
    
    static void TestEventDataIntegrity()
    {
        try
        {
            // Test all event types have correct properties
            var evt1 = new CompatibilityModeActivatedEvent("1.0.7");
            if (evt1.MorePropsVersion != "1.0.7") throw new Exception("Version mismatch");
            if (evt1.Source != "CoordinationManager") throw new Exception("Source mismatch");
            if (evt1.Timestamp == default(DateTime)) throw new Exception("Timestamp not set");
            
            var evt2 = new SaveCoordinationStartedEvent("MNC");
            if (evt2.LockOwner != "MNC") throw new Exception("Lock owner mismatch");
            if (evt2.Source != "SaveCoordinator") throw new Exception("Source mismatch");
            
            var evt3 = new SaveCoordinationCompletedEvent(true, TimeSpan.FromSeconds(1.5), "MNC");
            if (evt3.Success != true) throw new Exception("Success mismatch");
            if (evt3.LockOwner != "MNC") throw new Exception("Lock owner mismatch");
            if (Math.Abs(evt3.Duration.TotalSeconds - 1.5) > 0.001) throw new Exception("Duration mismatch");
            
            var evt4 = new GridSanitizationRequestedEvent("Test");
            if (evt4.Context != "Test") throw new Exception("Context mismatch");
            if (evt4.Source != "GridCoordinator") throw new Exception("Source mismatch");
            
            var evt5 = new GridSanitizationCompletedEvent(5, TimeSpan.FromSeconds(0.5));
            if (evt5.CyclesRepaired != 5) throw new Exception("Cycles repaired mismatch");
            if (Math.Abs(evt5.Duration.TotalSeconds - 0.5) > 0.001) throw new Exception("Duration mismatch");
            
            Pass("TestEventDataIntegrity");
        }
        catch (Exception ex)
        {
            Fail("TestEventDataIntegrity", ex.Message);
        }
    }
    
    static void Pass(string test)
    {
        s_testsPassed++;
        Console.WriteLine($"✅ {test}");
    }
    
    static void Fail(string test, string reason)
    {
        s_testsFailed++;
        Console.WriteLine($"❌ {test}: {reason}");
    }
}

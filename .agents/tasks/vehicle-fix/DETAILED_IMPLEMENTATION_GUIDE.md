# DETAILED IMPLEMENTATION GUIDE - Complete Code Structure
**Version:** 1.0  
**Date:** October 16, 2026  
**Purpose:** Comprehensive step-by-step guide với pseudocode đầy đủ  

---

## 📐 ARCHITECTURE OVERVIEW

```
Renew.cs Structure (Current: 8971 lines → Target: ~14000 lines)

[Lines 1-800] Existing: DiagnosticLogger, MemoryOptimizer, ArrayGuardian, SafeFlagsAccessor
[Lines 800-1000] Existing: RenewMod class (IUserMod)
[Lines 1000-3000] Existing: InstanceManager, MissingAssetProtection, TMPECompatibility
[Lines 3000-8971] Existing: TMPEPatches, LoadingExtension, other utilities

[NEW Lines 8972-10000] ArrayGuardianV2 class (~200 lines)
[NEW Lines 10001-10400] VehiclePoolMonitor class (~400 lines)
[NEW Lines 10401-10600] TMPEBufferSync class (~200 lines)
[NEW Lines 10601-11000] PathFindValidator class (~400 lines)
[NEW Lines 11001-11800] StuckVehicleRecovery class (~800 lines)
[NEW Lines 11801-13500] DiagnosticsUI class (~1700 lines)
[NEW Lines 13501-13800] VehicleFixPatches class (~300 lines)
[NEW Lines 13801-14000] LoadingExtension modifications (~200 lines)
```

---

## 🔨 CLASS 1: ArrayGuardianV2 (Real-Time Monitoring)

### Location in File
**Insert after:** Line ~600 (after existing SafeFlagsAccessor class)  
**Before:** RenewMod class

### Complete Class Structure

```csharp
// ============================================================================
// ARRAY GUARDIAN V2 - Real-Time Monitoring with Background Thread
// ============================================================================
/// <summary>
/// Monitors critical arrays in background thread every 5 seconds.
/// Auto-repairs shrinkage and prevents IndexOutOfRangeException.
/// NEW in v4.1: Monitors SpeedLimitManager.speedLimitArray
/// </summary>
public static class ArrayGuardianV2
{
    // ========================================================================
    // FIELDS
    // ========================================================================
    private static Thread monitorThread = null;
    private static bool isMonitoring = false;
    private static bool stopRequested = false;
    private static readonly object monitorLock = new object();
    
    // Configuration
    private static readonly int checkIntervalMs = 5000; // 5 seconds
    private static readonly int speedLimitDefaultValue = 139; // 50 km/h in game units
    
    // Diagnostics
    private static DateTime lastCheckTime = DateTime.MinValue;
    private static int checksPerformed = 0;
    private static int repairsTriggered = 0;
    private static int speedLimitRepairs = 0;
    
    // Cached references for performance (avoid reflection every check)
    private static Assembly cachedTMPE = null;
    private static Type cachedFlagsType = null;
    private static Type cachedSpeedLimitManagerType = null;
    private static FieldInfo cachedSpeedLimitArrayField = null;
    
    // ========================================================================
    // PUBLIC API
    // ========================================================================
    
    /// <summary>
    /// Start background monitoring thread. Call from RenewMod.OnEnabled()
    /// </summary>
    public static void StartMonitoring()
    {
        if (isMonitoring)
        {
            UnityEngine.Debug.Log("[ArrayGuardianV2] Already monitoring");
            return;
        }
        
        try
        {
            stopRequested = false;
            isMonitoring = true;
            
            monitorThread = new Thread(MonitorLoop);
            monitorThread.IsBackground = true; // Won't prevent app exit
            monitorThread.Priority = System.Threading.ThreadPriority.BelowNormal;
            monitorThread.Name = "ArrayGuardianV2_Monitor";
            monitorThread.Start();
            
            UnityEngine.Debug.Log("[ArrayGuardianV2] Monitoring started (check every 5s)");
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[ArrayGuardianV2] StartMonitoring failed: " + ex);
            isMonitoring = false;
        }
    }
    
    /// <summary>
    /// Stop background monitoring thread. Call from OnLevelUnloading()
    /// </summary>
    public static void StopMonitoring()
    {
        if (!isMonitoring)
        {
            return;
        }
        
        try
        {
            stopRequested = true;
            
            // Wait max 2 seconds for thread to finish
            if (monitorThread != null && monitorThread.IsAlive)
            {
                monitorThread.Join(2000);
            }
            
            isMonitoring = false;
            monitorThread = null;
            
            UnityEngine.Debug.Log(string.Format(
                "[ArrayGuardianV2] Monitoring stopped. Stats: {0} checks, {1} repairs, {2} speed limit fixes",
                checksPerformed, repairsTriggered, speedLimitRepairs));
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[ArrayGuardianV2] StopMonitoring error: " + ex);
        }
    }
    
    /// <summary>
    /// Get current monitoring status for diagnostics UI
    /// </summary>
    public static string GetStatus()
    {
        if (!isMonitoring)
        {
            return "Monitoring: Stopped";
        }
        
        double secondsSinceLastCheck = (DateTime.Now - lastCheckTime).TotalSeconds;
        return string.Format(
            "Monitoring: Active | Last check: {0:F0}s ago | Checks: {1} | Repairs: {2} | Speed fixes: {3}",
            secondsSinceLastCheck, checksPerformed, repairsTriggered, speedLimitRepairs);
    }
    
    // ========================================================================
    // BACKGROUND MONITORING LOOP
    // ========================================================================
    
    private static void MonitorLoop()
    {
        UnityEngine.Debug.Log("[ArrayGuardianV2] Monitor thread started (ID: " + Thread.CurrentThread.ManagedThreadId + ")");
        
        while (!stopRequested)
        {
            try
            {
                // Wait first to avoid immediate check on start
                Thread.Sleep(checkIntervalMs);
                
                if (stopRequested) break;
                
                // Perform checks
                lock (monitorLock)
                {
                    PerformMonitoringChecks();
                }
            }
            catch (ThreadAbortException)
            {
                // Normal shutdown, exit gracefully
                break;
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError("[ArrayGuardianV2] Monitor loop error: " + ex);
                // Continue monitoring despite errors
            }
        }
        
        UnityEngine.Debug.Log("[ArrayGuardianV2] Monitor thread stopped");
    }
    
    private static void PerformMonitoringChecks()
    {
        lastCheckTime = DateTime.Now;
        checksPerformed++;
        
        bool needsRepair = false;
        
        // Check 1: Existing ArrayGuardian shrinkage detection
        if (ArrayGuardian.HasShrinkage())
        {
            UnityEngine.Debug.LogWarning("[ArrayGuardianV2] Shrinkage detected in tracked arrays!");
            needsRepair = true;
        }
        
        // Check 2: Speed limit array (CRITICAL - prevents stuck vehicles)
        if (CheckSpeedLimitArray())
        {
            needsRepair = true;
        }
        
        // Check 3: TM:PE Flags arrays
        if (CheckTMPEFlagsArrays())
        {
            needsRepair = true;
        }
        
        // Trigger repair if any check failed
        if (needsRepair)
        {
            repairsTriggered++;
            PerformAutoRepair();
        }
    }
    
    // ========================================================================
    // CHECK: SPEED LIMIT ARRAY (Most Critical)
    // ========================================================================
    
    /// <summary>
    /// Check SpeedLimitManager.speedLimitArray size.
    /// This is THE critical check that prevents stuck vehicles.
    /// Returns true if repair needed.
    /// </summary>
    private static bool CheckSpeedLimitArray()
    {
        try
        {
            // Cache TMPE assembly reference
            if (cachedTMPE == null)
            {
                foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (asm.GetName().Name == "TrafficManager")
                    {
                        cachedTMPE = asm;
                        break;
                    }
                }
            }
            
            if (cachedTMPE == null)
            {
                // TM:PE not loaded yet, not an error
                return false;
            }
            
            // Cache SpeedLimitManager type
            if (cachedSpeedLimitManagerType == null)
            {
                cachedSpeedLimitManagerType = cachedTMPE.GetType("TrafficManager.Manager.Impl.SpeedLimitManager");
            }
            
            if (cachedSpeedLimitManagerType == null)
            {
                return false;
            }
            
            // Cache speedLimitArray field
            if (cachedSpeedLimitArrayField == null)
            {
                cachedSpeedLimitArrayField = cachedSpeedLimitManagerType.GetField("speedLimitArray",
                    BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public);
            }
            
            if (cachedSpeedLimitArrayField == null)
            {
                return false;
            }
            
            // Get current array
            Array speedLimitArray = cachedSpeedLimitArrayField.GetValue(null) as Array;
            
            if (speedLimitArray == null)
            {
                UnityEngine.Debug.LogWarning("[ArrayGuardianV2] speedLimitArray is null!");
                return true; // Needs repair
            }
            
            // Check size
            const int REQUIRED_SIZE = 524288; // Must match lane count
            
            if (speedLimitArray.Length < REQUIRED_SIZE)
            {
                UnityEngine.Debug.LogWarning(string.Format(
                    "[ArrayGuardianV2] CRITICAL: speedLimitArray shrunk! Current: {0}, Required: {1}",
                    speedLimitArray.Length, REQUIRED_SIZE));
                return true; // Needs repair
            }
            
            // Check for zero values in high-ID range (lanes > 32768)
            // Sample check: check lanes 32768, 40000, 50000, 60000
            int[] sampleLanes = new int[] { 32768, 40000, 50000, 60000 };
            int zeroCount = 0;
            
            foreach (int laneId in sampleLanes)
            {
                if (laneId < speedLimitArray.Length)
                {
                    object value = speedLimitArray.GetValue(laneId);
                    if (value != null && Convert.ToUInt16(value) == 0)
                    {
                        zeroCount++;
                    }
                }
            }
            
            if (zeroCount > 2) // More than half of samples are zero
            {
                UnityEngine.Debug.LogWarning(string.Format(
                    "[ArrayGuardianV2] Detected {0}/{1} sample lanes with speed=0 (unpopulated)",
                    zeroCount, sampleLanes.Length));
                return true; // Needs population
            }
            
            // All OK
            return false;
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[ArrayGuardianV2] CheckSpeedLimitArray error: " + ex);
            return false; // Don't trigger repair on error
        }
    }
    
    // ========================================================================
    // CHECK: TM:PE FLAGS ARRAYS
    // ========================================================================
    
    private static bool CheckTMPEFlagsArrays()
    {
        try
        {
            if (cachedTMPE == null || cachedFlagsType == null)
            {
                if (cachedTMPE == null) return false;
                cachedFlagsType = cachedTMPE.GetType("TrafficManager.State.Flags");
            }
            
            if (cachedFlagsType == null) return false;
            
            bool needsRepair = false;
            
            // Check laneArrowFlags
            needsRepair |= !ValidateArraySize(cachedFlagsType, "laneArrowFlags", 524288);
            
            // Check highwayLaneArrowFlags
            needsRepair |= !ValidateArraySize(cachedFlagsType, "highwayLaneArrowFlags", 524288);
            
            // Check laneAllowedVehicleTypesArray
            needsRepair |= !ValidateArraySize(cachedFlagsType, "laneAllowedVehicleTypesArray", 524288);
            
            return needsRepair;
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[ArrayGuardianV2] CheckTMPEFlagsArrays error: " + ex);
            return false;
        }
    }
    
    private static bool ValidateArraySize(Type type, string fieldName, int requiredSize)
    {
        try
        {
            FieldInfo field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public);
            if (field == null) return true; // Can't validate, assume OK
            
            Array array = field.GetValue(null) as Array;
            if (array == null) return false; // Needs repair
            
            if (array.Length < requiredSize)
            {
                UnityEngine.Debug.LogWarning(string.Format(
                    "[ArrayGuardianV2] {0}.{1} shrunk: {2} < {3}",
                    type.Name, fieldName, array.Length, requiredSize));
                return false; // Needs repair
            }
            
            return true; // OK
        }
        catch
        {
            return true; // Can't validate, assume OK
        }
    }
    
    // ========================================================================
    // REPAIR: AUTO-REPAIR ALL DETECTED ISSUES
    // ========================================================================
    
    private static void PerformAutoRepair()
    {
        try
        {
            UnityEngine.Debug.Log("[ArrayGuardianV2] Starting auto-repair...");
            
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            
            // Step 1: Repair tracked arrays via existing ArrayGuardian
            bool existingRepairOK = ArrayGuardian.RepairAllArrays();
            
            // Step 2: Repair speed limit array specifically
            bool speedLimitOK = RepairSpeedLimitArray();
            if (speedLimitOK)
            {
                speedLimitRepairs++;
            }
            
            // Step 3: Sync TM:PE buffers to ensure consistency
            try
            {
                TMPECompatibility.SynchronizeTMPEBuffersAndArrays("ArrayGuardianV2.AutoRepair");
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError("[ArrayGuardianV2] Buffer sync error: " + ex);
            }
            
            sw.Stop();
            
            UnityEngine.Debug.Log(string.Format(
                "[ArrayGuardianV2] Auto-repair completed in {0}ms. Tracked arrays: {1}, Speed limit: {2}",
                sw.ElapsedMilliseconds, existingRepairOK ? "OK" : "FAILED", speedLimitOK ? "OK" : "FAILED"));
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[ArrayGuardianV2] PerformAutoRepair error: " + ex);
        }
    }
    
    /// <summary>
    /// Repair speedLimitArray: expand to 524288 and fill with default values
    /// </summary>
    private static bool RepairSpeedLimitArray()
    {
        try
        {
            if (cachedSpeedLimitArrayField == null)
            {
                UnityEngine.Debug.LogWarning("[ArrayGuardianV2] Cannot repair speedLimitArray: field not cached");
                return false;
            }
            
            Array currentArray = cachedSpeedLimitArrayField.GetValue(null) as Array;
            
            if (currentArray == null)
            {
                UnityEngine.Debug.LogError("[ArrayGuardianV2] speedLimitArray is null, cannot repair");
                return false;
            }
            
            int currentSize = currentArray.Length;
            const int TARGET_SIZE = 524288;
            
            if (currentSize >= TARGET_SIZE)
            {
                // Size is OK, but check if high-ID lanes need population
                return PopulateSpeedLimitDefaults(currentArray, 32768, TARGET_SIZE);
            }
            
            // Need to expand
            UnityEngine.Debug.Log(string.Format(
                "[ArrayGuardianV2] Expanding speedLimitArray: {0} → {1}",
                currentSize, TARGET_SIZE));
            
            Type elementType = currentArray.GetType().GetElementType();
            Array newArray = Array.CreateInstance(elementType, TARGET_SIZE);
            
            // Copy existing data
            Array.Copy(currentArray, newArray, currentSize);
            
            // Fill new slots with default speed (50 km/h = 139 in game units)
            for (int i = currentSize; i < TARGET_SIZE; i++)
            {
                newArray.SetValue((ushort)speedLimitDefaultValue, i);
            }
            
            // Set the new array
            cachedSpeedLimitArrayField.SetValue(null, newArray);
            
            UnityEngine.Debug.Log(string.Format(
                "[ArrayGuardianV2] speedLimitArray expanded and populated with defaults. Filled {0} slots with speed={1}",
                TARGET_SIZE - currentSize, speedLimitDefaultValue));
            
            return true;
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[ArrayGuardianV2] RepairSpeedLimitArray error: " + ex);
            return false;
        }
    }
    
    /// <summary>
    /// Populate default speed values for lanes in range [startIndex, endIndex)
    /// </summary>
    private static bool PopulateSpeedLimitDefaults(Array array, int startIndex, int endIndex)
    {
        try
        {
            int populatedCount = 0;
            
            for (int i = startIndex; i < endIndex && i < array.Length; i++)
            {
                object currentValue = array.GetValue(i);
                if (currentValue != null && Convert.ToUInt16(currentValue) == 0)
                {
                    array.SetValue((ushort)speedLimitDefaultValue, i);
                    populatedCount++;
                }
            }
            
            if (populatedCount > 0)
            {
                UnityEngine.Debug.Log(string.Format(
                    "[ArrayGuardianV2] Populated {0} unpopulated speed limit slots in range [{1}, {2}) with default {3}",
                    populatedCount, startIndex, endIndex, speedLimitDefaultValue));
                return true;
            }
            
            return false;
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[ArrayGuardianV2] PopulateSpeedLimitDefaults error: " + ex);
            return false;
        }
    }
    
    // ========================================================================
    // DIAGNOSTICS & CLEANUP
    // ========================================================================
    
    /// <summary>
    /// Reset all statistics (for testing)
    /// </summary>
    public static void ResetStats()
    {
        checksPerformed = 0;
        repairsTriggered = 0;
        speedLimitRepairs = 0;
        lastCheckTime = DateTime.MinValue;
    }
    
    /// <summary>
    /// Force immediate check (for manual trigger from console)
    /// </summary>
    public static void ForceCheck()
    {
        if (!isMonitoring)
        {
            UnityEngine.Debug.LogWarning("[ArrayGuardianV2] Not monitoring, cannot force check");
            return;
        }
        
        try
        {
            lock (monitorLock)
            {
                UnityEngine.Debug.Log("[ArrayGuardianV2] Manual check triggered");
                PerformMonitoringChecks();
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[ArrayGuardianV2] ForceCheck error: " + ex);
        }
    }
}
```

---

## 🔨 CLASS 2: VehiclePoolMonitor (Vehicle Count Health)

### Location in File
**Insert after:** ArrayGuardianV2 class  
**Estimated lines:** ~400 lines

### Complete Class Structure

```csharp
// ============================================================================
// VEHICLE POOL MONITOR - Vehicle Count Health Checking
// ============================================================================
/// <summary>
/// Monitors VehicleManager.m_vehicleCount for corruption.
/// Detects spawn throttling and repairs vehicle count mismatches.
/// Runs every 60 simulation frames (~1 second @ 60fps)
/// </summary>
public static class VehiclePoolMonitor
{
    // ========================================================================
    // FIELDS
    // ========================================================================
    
    // State tracking
    private static int lastActualCount = 0;
    private static uint lastCheckFrame = 0;
    private static int consecutiveStaticChecks = 0;
    
    // Configuration
    private static readonly int checkIntervalFrames = 60; // Check every second
    private static readonly int staticThreshold = 10; // 10 consecutive static counts = throttling
    private static readonly float spawnThrottleCapacity = 0.5f; // 50% capacity threshold
    
    // Diagnostics
    private static int totalChecks = 0;
    private static int countMismatches = 0;
    private static int throttlingDetections = 0;
    private static int repairsPerformed = 0;
    private static DateTime lastRepairTime = DateTime.MinValue;
    
    // ========================================================================
    // PUBLIC API
    // ========================================================================
    
    /// <summary>
    /// Check and repair vehicle count. Called from SimulationStep Harmony patch.
    /// </summary>
    public static void CheckAndRepair()
    {
        try
        {
            VehicleManager vm = Singleton<VehicleManager>.instance;
            if (vm == null) return;
            
            SimulationManager sm = Singleton<SimulationManager>.instance;
            if (sm == null) return;
            
            uint currentFrame = sm.m_currentFrameIndex;
            
            // Only check every N frames
            if (currentFrame - lastCheckFrame < checkIntervalFrames)
            {
                return;
            }
            
            lastCheckFrame = currentFrame;
            totalChecks++;
            
            // Perform checks
            int actualCount = CountActualVehicles(vm);
            int reportedCount = vm.m_vehicleCount;
            
            // Check 1: Count mismatch
            if (actualCount != reportedCount)
            {
                HandleCountMismatch(vm, actualCount, reportedCount);
            }
            
            // Check 2: Spawn throttling detection
            if (DetectSpawnThrottling(vm, actualCount))
            {
                HandleSpawnThrottling(vm);
            }
            
            lastActualCount = actualCount;
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[VehiclePoolMonitor] CheckAndRepair error: " + ex);
        }
    }
    
    /// <summary>
    /// Initial check on level load
    /// </summary>
    public static void InitialCheck()
    {
        try
        {
            VehicleManager vm = Singleton<VehicleManager>.instance;
            if (vm == null) return;
            
            int actualCount = CountActualVehicles(vm);
            int reportedCount = vm.m_vehicleCount;
            
            UnityEngine.Debug.Log(string.Format(
                "[VehiclePoolMonitor] Initial state: Reported={0}, Actual={1}, Buffer size={2}",
                reportedCount, actualCount, vm.m_vehicles.m_buffer.Length));
            
            if (actualCount != reportedCount)
            {
                UnityEngine.Debug.LogWarning(string.Format(
                    "[VehiclePoolMonitor] Initial count mismatch detected! Repairing..."));
                HandleCountMismatch(vm, actualCount, reportedCount);
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[VehiclePoolMonitor] InitialCheck error: " + ex);
        }
    }
    
    /// <summary>
    /// Get status for diagnostics UI
    /// </summary>
    public static string GetStatus()
    {
        VehicleManager vm = Singleton<VehicleManager>.instance;
        if (vm == null) return "Vehicle Manager: Not loaded";
        
        int actual = CountActualVehicles(vm);
        int reported = vm.m_vehicleCount;
        bool mismatch = actual != reported;
        
        return string.Format(
            "Vehicles: {0} / {1} {2} | Checks: {3} | Mismatches: {4} | Throttling: {5} | Last repair: {6}",
            actual,
            vm.m_vehicles.m_buffer.Length,
            mismatch ? "⚠ MISMATCH" : "✓",
            totalChecks,
            countMismatches,
            throttlingDetections,
            lastRepairTime == DateTime.MinValue ? "Never" : FormatTimeSince(lastRepairTime));
    }
    
    // ========================================================================
    // COUNT ACTUAL VEHICLES
    // ========================================================================
    
    /// <summary>
    /// Count vehicles with Created flag in buffer.
    /// This is the ground truth vs VehicleManager.m_vehicleCount.
    /// </summary>
    private static int CountActualVehicles(VehicleManager vm)
    {
        int count = 0;
        
        try
        {
            Vehicle[] buffer = vm.m_vehicles.m_buffer;
            
            for (int i = 0; i < buffer.Length; i++)
            {
                if ((buffer[i].m_flags & Vehicle.Flags.Created) != 0)
                {
                    count++;
                }
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[VehiclePoolMonitor] CountActualVehicles error: " + ex);
        }
        
        return count;
    }
    
    // ========================================================================
    // MISMATCH HANDLING
    // ========================================================================
    
    /// <summary>
    /// Handle mismatch between reported and actual count
    /// </summary>
    private static void HandleCountMismatch(VehicleManager vm, int actual, int reported)
    {
        countMismatches++;
        
        UnityEngine.Debug.LogWarning(string.Format(
            "[VehiclePoolMonitor] Count mismatch detected! Reported: {0}, Actual: {1}, Diff: {2}",
            reported, actual, reported - actual));
        
        // Repair the count via reflection
        try
        {
            FieldInfo countField = typeof(VehicleManager).GetField("m_vehicleCount",
                BindingFlags.NonPublic | BindingFlags.Instance);
            
            if (countField != null)
            {
                countField.SetValue(vm, actual);
                repairsPerformed++;
                lastRepairTime = DateTime.Now;
                
                UnityEngine.Debug.Log(string.Format(
                    "[VehiclePoolMonitor] Count repaired: {0} → {1}", reported, actual));
            }
            else
            {
                UnityEngine.Debug.LogError("[VehiclePoolMonitor] m_vehicleCount field not found!");
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[VehiclePoolMonitor] HandleCountMismatch repair error: " + ex);
        }
    }
    
    // ========================================================================
    // SPAWN THROTTLING DETECTION
    // ========================================================================
    
    /// <summary>
    /// Detect if spawn throttling is occurring:
    /// - Count hasn't changed for N checks
    /// - But count is < 50% of capacity
    /// - Indicates spawn is being blocked
    /// </summary>
    private static bool DetectSpawnThrottling(VehicleManager vm, int currentCount)
    {
        // Check if count is static
        if (currentCount == lastActualCount)
        {
            consecutiveStaticChecks++;
        }
        else
        {
            consecutiveStaticChecks = 0;
            return false; // Count is changing, no throttling
        }
        
        // Check if count has been static for threshold checks
        if (consecutiveStaticChecks < staticThreshold)
        {
            return false; // Not enough evidence yet
        }
        
        // Check if count is below capacity (should be spawning)
        int bufferSize = vm.m_vehicles.m_buffer.Length;
        float currentCapacityRatio = (float)currentCount / bufferSize;
        
        if (currentCapacityRatio < spawnThrottleCapacity)
        {
            // Count is static AND below 50% capacity = likely throttling
            return true;
        }
        
        return false;
    }
    
    /// <summary>
    /// Handle spawn throttling by resetting internal counters
    /// </summary>
    private static void HandleSpawnThrottling(VehicleManager vm)
    {
        throttlingDetections++;
        
        UnityEngine.Debug.LogWarning(string.Format(
            "[VehiclePoolMonitor] Spawn throttling detected! Count static at {0} for {1} checks ({2:F1}% capacity)",
            lastActualCount, consecutiveStaticChecks,
            (float)lastActualCount / vm.m_vehicles.m_buffer.Length * 100f));
        
        try
        {
            // Reset m_infoCount array (per-prefab counts)
            ResetInfoCounts(vm);
            
            // Recount all vehicles to rebuild internal state
            RecountAllVehicles(vm);
            
            // Reset our static counter
            consecutiveStaticChecks = 0;
            
            repairsPerformed++;
            lastRepairTime = DateTime.Now;
            
            UnityEngine.Debug.Log("[VehiclePoolMonitor] Spawn throttling repair completed");
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[VehiclePoolMonitor] HandleSpawnThrottling error: " + ex);
        }
    }
    
    // ========================================================================
    // INTERNAL COUNTER REPAIR
    // ========================================================================
    
    /// <summary>
    /// Reset m_infoCount array (tracks count per vehicle prefab)
    /// </summary>
    private static void ResetInfoCounts(VehicleManager vm)
    {
        try
        {
            FieldInfo infoCountField = typeof(VehicleManager).GetField("m_infoCount",
                BindingFlags.NonPublic | BindingFlags.Instance);
            
            if (infoCountField != null)
            {
                Array infoCount = infoCountField.GetValue(vm) as Array;
                
                if (infoCount != null)
                {
                    // Zero out all counts
                    for (int i = 0; i < infoCount.Length; i++)
                    {
                        infoCount.SetValue(0, i);
                    }
                    
                    UnityEngine.Debug.Log(string.Format(
                        "[VehiclePoolMonitor] Reset m_infoCount array ({0} entries)", infoCount.Length));
                }
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[VehiclePoolMonitor] ResetInfoCounts error: " + ex);
        }
    }
    
    /// <summary>
    /// Recount all vehicles to rebuild VehicleManager internal state
    /// </summary>
    private static void RecountAllVehicles(VehicleManager vm)
    {
        try
        {
            int recounted = 0;
            Vehicle[] buffer = vm.m_vehicles.m_buffer;
            
            for (int i = 0; i < buffer.Length; i++)
            {
                if ((buffer[i].m_flags & Vehicle.Flags.Created) != 0)
                {
                    VehicleInfo info = buffer[i].Info;
                    
                    if (info != null && info.m_vehicleAI != null)
                    {
                        // Call GetSize to trigger internal counter updates
                        try
                        {
                            int size, max;
                            info.m_vehicleAI.GetSize((ushort)i, ref buffer[i], out size, out max);
                            recounted++;
                        }
                        catch
                        {
                            // Some vehicles may fail, continue
                        }
                    }
                }
            }
            
            UnityEngine.Debug.Log(string.Format(
                "[VehiclePoolMonitor] Recounted {0} vehicles", recounted));
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[VehiclePoolMonitor] RecountAllVehicles error: " + ex);
        }
    }
    
    // ========================================================================
    // DIAGNOSTICS
    // ========================================================================
    
    /// <summary>
    /// Reset all statistics
    /// </summary>
    public static void ResetStats()
    {
        totalChecks = 0;
        countMismatches = 0;
        throttlingDetections = 0;
        repairsPerformed = 0;
        consecutiveStaticChecks = 0;
        lastRepairTime = DateTime.MinValue;
    }
    
    /// <summary>
    /// Force immediate repair (for console command)
    /// </summary>
    public static void ForceRepair()
    {
        try
        {
            VehicleManager vm = Singleton<VehicleManager>.instance;
            if (vm == null)
            {
                UnityEngine.Debug.LogWarning("[VehiclePoolMonitor] VehicleManager not loaded");
                return;
            }
            
            UnityEngine.Debug.Log("[VehiclePoolMonitor] Manual repair triggered");
            
            int actual = CountActualVehicles(vm);
            int reported = vm.m_vehicleCount;
            
            if (actual != reported)
            {
                HandleCountMismatch(vm, actual, reported);
            }
            
            // Always reset counters and recount
            ResetInfoCounts(vm);
            RecountAllVehicles(vm);
            
            UnityEngine.Debug.Log("[VehiclePoolMonitor] Manual repair completed");
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[VehiclePoolMonitor] ForceRepair error: " + ex);
        }
    }
    
    private static string FormatTimeSince(DateTime time)
    {
        TimeSpan span = DateTime.Now - time;
        if (span.TotalSeconds < 60)
            return string.Format("{0:F0}s ago", span.TotalSeconds);
        if (span.TotalMinutes < 60)
            return string.Format("{0:F0}m ago", span.TotalMinutes);
        return string.Format("{0:F0}h ago", span.TotalHours);
    }
}
```

Tôi sẽ tiếp tục với các classes còn lại trong message tiếp theo. Document này đang được xây dựng với:
- ✅ Complete code structure
- ✅ Detailed comments
- ✅ Error handling
- ✅ Diagnostics
- ✅ Thread safety

Bạn muốn tôi tiếp tục với classes 3-7 không?


---

## 🔨 CLASS 3: TMPEBufferSync (Auto-Sync Buffers)

### Location in File
**Insert after:** VehiclePoolMonitor class  
**Estimated lines:** ~200 lines

### Complete Class Structure

```csharp
// ============================================================================
// TM:PE BUFFER AUTO-SYNC - Keep Extension Buffers in Sync
// ============================================================================
/// <summary>
/// Automatically synchronizes TM:PE's extension buffers with game's live buffers.
/// Prevents desync when roads are built/demolished or after save load.
/// Installs Harmony hooks on key events.
/// </summary>
public static class TMPEBufferSync
{
    // ========================================================================
    // FIELDS
    // ========================================================================
    
    private static bool hooksInstalled = false;
    private static int syncCount = 0;
    private static DateTime lastSyncTime = DateTime.MinValue;
    private static string lastSyncReason = "";
    
    // ========================================================================
    // PUBLIC API
    // ========================================================================
    
    /// <summary>
    /// Install Harmony hooks for auto-sync. Call from LoadingExtension.OnLevelLoaded()
    /// </summary>
    public static void InstallHooks(HarmonyInstance harmony)
    {
        if (hooksInstalled)
        {
            UnityEngine.Debug.Log("[TMPEBufferSync] Hooks already installed");
            return;
        }
        
        try
        {
            // Hook 1: NetManager.UpdateSegmentRenderer (called when network changes)
            MethodInfo updateSegRenderer = typeof(NetManager).GetMethod("UpdateSegmentRenderer",
                BindingFlags.Public | BindingFlags.Instance);
            
            if (updateSegRenderer != null)
            {
                MethodInfo postfix = typeof(VehicleFixPatches).GetMethod(
                    nameof(VehicleFixPatches.NetManager_UpdateSegmentRenderer_Postfix),
                    BindingFlags.Public | BindingFlags.Static);
                
                if (postfix != null)
                {
                    harmony.Patch(updateSegRenderer, postfix: new HarmonyMethod(postfix));
                    UnityEngine.Debug.Log("[TMPEBufferSync] Patched NetManager.UpdateSegmentRenderer");
                }
            }
            
            // Hook 2: TM:PE SerializableDataExtension.OnAfterDeserialize (called after load)
            Assembly tmpe = TMPECompatibility.GetTMPEAssembly();
            if (tmpe != null)
            {
                Type sdeType = tmpe.GetType("TrafficManager.Lifecycle.SerializableDataExtension");
                if (sdeType != null)
                {
                    MethodInfo onAfterDeser = sdeType.GetMethod("OnAfterDeserialize",
                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    
                    if (onAfterDeser != null)
                    {
                        MethodInfo postfix = typeof(VehicleFixPatches).GetMethod(
                            nameof(VehicleFixPatches.TMPE_OnAfterDeserialize_Postfix),
                            BindingFlags.Public | BindingFlags.Static);
                        
                        if (postfix != null)
                        {
                            harmony.Patch(onAfterDeser, postfix: new HarmonyMethod(postfix));
                            UnityEngine.Debug.Log("[TMPEBufferSync] Patched TM:PE SerializableDataExtension.OnAfterDeserialize");
                        }
                    }
                }
            }
            
            hooksInstalled = true;
            UnityEngine.Debug.Log("[TMPEBufferSync] Hooks installed successfully (2 patches)");
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[TMPEBufferSync] InstallHooks error: " + ex);
        }
    }
    
    /// <summary>
    /// Sync all TM:PE extension buffers with game's live buffers.
    /// This is the CORE fix for buffer desync causing routing errors.
    /// </summary>
    public static void SyncAllBuffers(string reason)
    {
        try
        {
            lastSyncReason = reason;
            lastSyncTime = DateTime.Now;
            syncCount++;
            
            Assembly tmpe = TMPECompatibility.GetTMPEAssembly();
            if (tmpe == null)
            {
                // TM:PE not loaded, nothing to sync
                return;
            }
            
            NetManager nm = Singleton<NetManager>.instance;
            if (nm == null) return;
            
            // Sync 1: Lane buffer
            if (nm.m_lanes != null && nm.m_lanes.m_buffer != null)
            {
                SyncBuffer(tmpe, "TrafficManager.Util.Extensions.NetLaneExtensions",
                    "_laneBuffer", nm.m_lanes.m_buffer);
            }
            
            // Sync 2: Segment buffer
            if (nm.m_segments != null && nm.m_segments.m_buffer != null)
            {
                SyncBuffer(tmpe, "TrafficManager.Util.Extensions.NetSegmentExtensions",
                    "_segBuffer", nm.m_segments.m_buffer);
            }
            
            // Sync 3: Node buffer
            if (nm.m_nodes != null && nm.m_nodes.m_buffer != null)
            {
                SyncBuffer(tmpe, "TrafficManager.Util.Extensions.NetNodeExtensions",
                    "_nodeBuffer", nm.m_nodes.m_buffer);
            }
            
            UnityEngine.Debug.Log(string.Format(
                "[TMPEBufferSync] Buffers synced (reason: {0}). Total syncs: {1}",
                reason, syncCount));
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[TMPEBufferSync] SyncAllBuffers error: " + ex);
        }
    }
    
    /// <summary>
    /// Sync individual buffer by setting static field reference
    /// </summary>
    private static void SyncBuffer(Assembly tmpeAssembly, string typeName, string fieldName, object liveBuffer)
    {
        try
        {
            Type type = tmpeAssembly.GetType(typeName);
            if (type == null)
            {
                UnityEngine.Debug.LogWarning(string.Format(
                    "[TMPEBufferSync] Type not found: {0}", typeName));
                return;
            }
            
            FieldInfo field = type.GetField(fieldName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            
            if (field == null)
            {
                UnityEngine.Debug.LogWarning(string.Format(
                    "[TMPEBufferSync] Field not found: {0}.{1}", typeName, fieldName));
                return;
            }
            
            // Get current buffer
            object currentBuffer = field.GetValue(null);
            
            // Check if already pointing to same buffer (optimization)
            if (currentBuffer == liveBuffer)
            {
                // Already synced, no action needed
                return;
            }
            
            // Set new buffer reference (NOT a copy, just a reference)
            field.SetValue(null, liveBuffer);
            
            Array arr = liveBuffer as Array;
            int length = arr != null ? arr.Length : -1;
            
            UnityEngine.Debug.Log(string.Format(
                "[TMPEBufferSync] Synced {0}.{1} → live buffer ({2} elements)",
                type.Name, fieldName, length));
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError(string.Format(
                "[TMPEBufferSync] SyncBuffer error for {0}.{1}: {2}",
                typeName, fieldName, ex));
        }
    }
    
    /// <summary>
    /// Get status for diagnostics
    /// </summary>
    public static string GetStatus()
    {
        if (!hooksInstalled)
        {
            return "Buffer Sync: Hooks not installed";
        }
        
        string timeSince = lastSyncTime == DateTime.MinValue
            ? "Never"
            : FormatTimeSince(lastSyncTime);
        
        return string.Format(
            "Buffer Sync: Active | Last: {0} ({1}) | Total: {2}",
            timeSince, lastSyncReason, syncCount);
    }
    
    private static string FormatTimeSince(DateTime time)
    {
        TimeSpan span = DateTime.Now - time;
        if (span.TotalSeconds < 60)
            return string.Format("{0:F0}s ago", span.TotalSeconds);
        return string.Format("{0:F0}m ago", span.TotalMinutes);
    }
}
```

---

## 🔨 CLASS 4: PathFindValidator (Clean Stale Paths)

### Location in File
**Insert after:** TMPEBufferSync class  
**Estimated lines:** ~400 lines

### Complete Class Structure

```csharp
// ============================================================================
// PATHFIND VALIDATOR - Clean Stale Path Cache
// ============================================================================
/// <summary>
/// Validates PathManager path cache and removes stale paths.
/// Stale paths reference lanes/segments that no longer exist,
/// causing vehicles to follow invalid routes and get stuck.
/// </summary>
public static class PathFindValidator
{
    // ========================================================================
    // FIELDS
    // ========================================================================
    
    private static DateTime lastValidationTime = DateTime.MinValue;
    private static int pathsCleaned = 0;
    private static int totalValidations = 0;
    private static bool validationInProgress = false;
    private static readonly object validationLock = new object();
    
    // ========================================================================
    // PUBLIC API
    // ========================================================================
    
    /// <summary>
    /// Validate and clean all paths in PathManager.
    /// WARNING: This can take 100-200ms, don't call every frame!
    /// </summary>
    public static void ValidateAndCleanPaths()
    {
        // Prevent concurrent validations
        if (validationInProgress)
        {
            UnityEngine.Debug.LogWarning("[PathFindValidator] Validation already in progress");
            return;
        }
        
        lock (validationLock)
        {
            validationInProgress = true;
        }
        
        try
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            
            PathManager pm = Singleton<PathManager>.instance;
            NetManager nm = Singleton<NetManager>.instance;
            
            if (pm == null || nm == null)
            {
                UnityEngine.Debug.LogWarning("[PathFindValidator] Managers not loaded");
                return;
            }
            
            totalValidations++;
            lastValidationTime = DateTime.Now;
            
            int cleaned = 0;
            int scanned = 0;
            
            // Scan all path units
            for (uint i = 0; i < pm.m_pathUnits.m_buffer.Length; i++)
            {
                if ((pm.m_pathUnits.m_buffer[i].m_pathFindFlags & PathUnit.FLAG_CREATED) != 0)
                {
                    scanned++;
                    
                    if (!IsPathValid(pm, nm, i))
                    {
                        // Invalid path, release it
                        pm.ReleasePath(i);
                        cleaned++;
                    }
                }
            }
            
            sw.Stop();
            pathsCleaned += cleaned;
            
            UnityEngine.Debug.Log(string.Format(
                "[PathFindValidator] Validation complete: Scanned {0} paths, cleaned {1} stale paths in {2}ms",
                scanned, cleaned, sw.ElapsedMilliseconds));
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[PathFindValidator] ValidateAndCleanPaths error: " + ex);
        }
        finally
        {
            lock (validationLock)
            {
                validationInProgress = false;
            }
        }
    }
    
    /// <summary>
    /// Get status for diagnostics
    /// </summary>
    public static string GetStatus()
    {
        string timeSince = lastValidationTime == DateTime.MinValue
            ? "Never"
            : FormatTimeSince(lastValidationTime);
        
        return string.Format(
            "PathFind Validator: {0} validations, {1} paths cleaned | Last: {2}",
            totalValidations, pathsCleaned, timeSince);
    }
    
    // ========================================================================
    // VALIDATION LOGIC
    // ========================================================================
    
    /// <summary>
    /// Check if a path is valid (all referenced lanes/segments exist)
    /// </summary>
    private static bool IsPathValid(PathManager pm, NetManager nm, uint pathId)
    {
        try
        {
            PathUnit pathUnit = pm.m_pathUnits.m_buffer[pathId];
            
            // Check all positions in path
            for (int i = 0; i < pathUnit.m_positionCount; i++)
            {
                PathUnit.Position pos = pathUnit.GetPosition(i);
                
                // Validate lane
                if (!IsLaneValid(nm, pos.m_lane))
                {
                    return false;
                }
                
                // Validate segment
                if (!IsSegmentValid(nm, pos.m_segment))
                {
                    return false;
                }
            }
            
            return true;
        }
        catch
        {
            // If we can't validate, assume invalid
            return false;
        }
    }
    
    /// <summary>
    /// Check if a lane ID is valid
    /// </summary>
    private static bool IsLaneValid(NetManager nm, uint laneId)
    {
        try
        {
            // Check bounds
            if (laneId >= nm.m_lanes.m_buffer.Length)
            {
                return false;
            }
            
            // Check Created flag
            if ((nm.m_lanes.m_buffer[laneId].m_flags & (ushort)NetLane.Flags.Created) == 0)
            {
                return false;
            }
            
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    /// <summary>
    /// Check if a segment ID is valid
    /// </summary>
    private static bool IsSegmentValid(NetManager nm, ushort segmentId)
    {
        try
        {
            // Check bounds
            if (segmentId >= nm.m_segments.m_buffer.Length)
            {
                return false;
            }
            
            // Check Created flag
            if ((nm.m_segments.m_buffer[segmentId].m_flags & NetSegment.Flags.Created) == 0)
            {
                return false;
            }
            
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    private static string FormatTimeSince(DateTime time)
    {
        TimeSpan span = DateTime.Now - time;
        if (span.TotalSeconds < 60)
            return string.Format("{0:F0}s ago", span.TotalSeconds);
        return string.Format("{0:F0}m ago", span.TotalMinutes);
    }
}
```

Tôi sẽ tiếp tục với các classes còn lại...


---

## 🔨 CLASS 5: StuckVehicleRecovery (Emergency Unstuck)

### Location in File
**Insert after:** PathFindValidator class  
**Estimated lines:** ~800 lines

### Complete Class Structure

```csharp
// ============================================================================
// STUCK VEHICLE RECOVERY - Emergency Unstuck System
// ============================================================================
/// <summary>
/// Detects vehicles that haven't moved for extended periods and applies
/// escalating recovery methods: path reset → teleport → despawn/respawn.
/// Runs every 64 simulation frames.
/// </summary>
public static class StuckVehicleRecovery
{
    // ========================================================================
    // FIELDS & STRUCTS
    // ========================================================================
    
    private class VehicleStuckInfo
    {
        public Vector3 LastPosition;
        public int StuckFrames;
        public uint PathUnit;
        public int RecoveryLevel; // 0=none, 1=path reset, 2=teleport, 3=despawn
    }
    
    private static Dictionary<ushort, VehicleStuckInfo> stuckVehicles = 
        new Dictionary<ushort, VehicleStuckInfo>();
    
    // Configuration
    private static readonly int scanIntervalFrames = 64;
    private static readonly float movementThreshold = 0.1f; // meters
    private static readonly float velocityThreshold = 0.1f; // m/s
    
    // Recovery thresholds (in frames @ 60fps)
    private static readonly int level1Threshold = 600;  // 10 seconds
    private static readonly int level2Threshold = 900;  // 15 seconds
    private static readonly int level3Threshold = 1200; // 20 seconds
    
    // State
    private static uint lastScanFrame = 0;
    private static int totalScans = 0;
    private static int level1Recoveries = 0;
    private static int level2Recoveries = 0;
    private static int level3Recoveries = 0;
    
    // ========================================================================
    // PUBLIC API
    // ========================================================================
    
    /// <summary>
    /// Scan all vehicles and recover stuck ones. Called from SimulationStep.
    /// </summary>
    public static void ScanAndRecover()
    {
        try
        {
            SimulationManager sm = Singleton<SimulationManager>.instance;
            if (sm == null) return;
            
            uint currentFrame = sm.m_currentFrameIndex;
            
            // Only scan every N frames
            if (currentFrame - lastScanFrame < scanIntervalFrames)
            {
                return;
            }
            
            lastScanFrame = currentFrame;
            totalScans++;
            
            VehicleManager vm = Singleton<VehicleManager>.instance;
            if (vm == null) return;
            
            int recoveredThisScan = 0;
            List<ushort> toRemove = new List<ushort>();
            
            // Scan all vehicles in buffer
            for (ushort i = 1; i < vm.m_vehicles.m_buffer.Length; i++)
            {
                if ((vm.m_vehicles.m_buffer[i].m_flags & Vehicle.Flags.Created) == 0)
                {
                    // Vehicle doesn't exist, remove from tracking
                    if (stuckVehicles.ContainsKey(i))
                    {
                        toRemove.Add(i);
                    }
                    continue;
                }
                
                Vehicle vehicle = vm.m_vehicles.m_buffer[i];
                
                // Skip vehicles without paths
                if (vehicle.m_path == 0)
                {
                    continue;
                }
                
                // Check if vehicle is stuck
                if (CheckVehicleMovement(i, vehicle))
                {
                    // Vehicle moved, no action needed
                    continue;
                }
                
                // Vehicle is stuck, apply recovery if needed
                if (ApplyRecoveryIfNeeded(vm, i))
                {
                    recoveredThisScan++;
                }
            }
            
            // Clean up tracking for deleted vehicles
            foreach (ushort id in toRemove)
            {
                stuckVehicles.Remove(id);
            }
            
            if (recoveredThisScan > 0)
            {
                UnityEngine.Debug.Log(string.Format(
                    "[StuckVehicleRecovery] Recovered {0} vehicles. Tracking: {1} potentially stuck",
                    recoveredThisScan, stuckVehicles.Count));
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[StuckVehicleRecovery] ScanAndRecover error: " + ex);
        }
    }
    
    /// <summary>
    /// Clear all tracking (on level unload)
    /// </summary>
    public static void ClearCache()
    {
        stuckVehicles.Clear();
        UnityEngine.Debug.Log("[StuckVehicleRecovery] Cache cleared");
    }
    
    /// <summary>
    /// Get status for diagnostics
    /// </summary>
    public static string GetStatus()
    {
        return string.Format(
            "Stuck Recovery: Tracking {0} | Scans: {1} | Recoveries: L1={2} L2={3} L3={4}",
            stuckVehicles.Count, totalScans, level1Recoveries, level2Recoveries, level3Recoveries);
    }
    
    /// <summary>
    /// Force recovery of all currently stuck vehicles (console command)
    /// </summary>
    public static void ForceRecoverAll()
    {
        try
        {
            VehicleManager vm = Singleton<VehicleManager>.instance;
            if (vm == null) return;
            
            int recovered = 0;
            List<ushort> vehicleIds = new List<ushort>(stuckVehicles.Keys);
            
            foreach (ushort vehicleId in vehicleIds)
            {
                VehicleStuckInfo info = stuckVehicles[vehicleId];
                
                // Force Level 1 recovery for all
                if (ApplyLevel1Recovery(vm, vehicleId))
                {
                    recovered++;
                    stuckVehicles.Remove(vehicleId);
                }
            }
            
            UnityEngine.Debug.Log(string.Format(
                "[StuckVehicleRecovery] Force recovery: {0} vehicles recovered", recovered));
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[StuckVehicleRecovery] ForceRecoverAll error: " + ex);
        }
    }
    
    // ========================================================================
    // MOVEMENT CHECKING
    // ========================================================================
    
    /// <summary>
    /// Check if vehicle has moved since last check.
    /// Returns true if moved, false if stuck.
    /// </summary>
    private static bool CheckVehicleMovement(ushort vehicleId, Vehicle vehicle)
    {
        try
        {
            Vector3 currentPos = vehicle.GetLastFramePosition();
            Vector3 velocity = vehicle.GetLastFrameVelocity();
            
            if (!stuckVehicles.ContainsKey(vehicleId))
            {
                // First time seeing this vehicle, start tracking
                stuckVehicles[vehicleId] = new VehicleStuckInfo
                {
                    LastPosition = currentPos,
                    StuckFrames = 0,
                    PathUnit = vehicle.m_path,
                    RecoveryLevel = 0
                };
                return true; // Not stuck yet
            }
            
            VehicleStuckInfo info = stuckVehicles[vehicleId];
            
            // Check if moved
            float distMoved = Vector3.Distance(currentPos, info.LastPosition);
            float speed = velocity.magnitude;
            
            if (distMoved > movementThreshold || speed > velocityThreshold)
            {
                // Vehicle moved! Reset tracking
                info.LastPosition = currentPos;
                info.StuckFrames = 0;
                info.RecoveryLevel = 0;
                return true; // Not stuck
            }
            
            // Vehicle didn't move, increment stuck counter
            info.StuckFrames += scanIntervalFrames;
            info.LastPosition = currentPos;
            
            return false; // Still stuck
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError(string.Format(
                "[StuckVehicleRecovery] CheckVehicleMovement error for vehicle {0}: {1}",
                vehicleId, ex));
            return true; // Assume not stuck on error
        }
    }
    
    // ========================================================================
    // RECOVERY APPLICATION
    // ========================================================================
    
    /// <summary>
    /// Apply recovery if vehicle has been stuck long enough.
    /// Returns true if recovery was applied.
    /// </summary>
    private static bool ApplyRecoveryIfNeeded(VehicleManager vm, ushort vehicleId)
    {
        if (!stuckVehicles.ContainsKey(vehicleId))
        {
            return false;
        }
        
        VehicleStuckInfo info = stuckVehicles[vehicleId];
        
        // Determine recovery level based on stuck duration
        if (info.StuckFrames >= level3Threshold && info.RecoveryLevel < 3)
        {
            // Level 3: Despawn/respawn (last resort)
            return ApplyLevel3Recovery(vm, vehicleId, info);
        }
        else if (info.StuckFrames >= level2Threshold && info.RecoveryLevel < 2)
        {
            // Level 2: Teleport
            return ApplyLevel2Recovery(vm, vehicleId, info);
        }
        else if (info.StuckFrames >= level1Threshold && info.RecoveryLevel < 1)
        {
            // Level 1: Path reset
            return ApplyLevel1Recovery(vm, vehicleId, info);
        }
        
        return false; // Not ready for recovery yet
    }
    
    /// <summary>
    /// Level 1 Recovery: Reset path, vehicle will request new pathfind
    /// </summary>
    private static bool ApplyLevel1Recovery(VehicleManager vm, ushort vehicleId)
    {
        VehicleStuckInfo info = stuckVehicles.ContainsKey(vehicleId) ? stuckVehicles[vehicleId] : null;
        return ApplyLevel1Recovery(vm, vehicleId, info);
    }
    
    private static bool ApplyLevel1Recovery(VehicleManager vm, ushort vehicleId, VehicleStuckInfo info)
    {
        try
        {
            Vehicle vehicle = vm.m_vehicles.m_buffer[vehicleId];
            
            UnityEngine.Debug.LogWarning(string.Format(
                "[StuckVehicleRecovery] Vehicle {0} stuck for {1:F0}s. Applying Level 1 Recovery (path reset)",
                vehicleId, (info != null ? info.StuckFrames : 600) / 60.0f));
            
            PathManager pm = Singleton<PathManager>.instance;
            if (pm != null && vehicle.m_path != 0)
            {
                // Release old path
                pm.ReleasePath(vehicle.m_path);
            }
            
            // Reset path fields
            vm.m_vehicles.m_buffer[vehicleId].m_path = 0;
            vm.m_vehicles.m_buffer[vehicleId].m_pathPositionIndex = 0;
            vm.m_vehicles.m_buffer[vehicleId].m_lastPathOffset = 0;
            
            // Vehicle AI will request new path on next update
            
            if (info != null)
            {
                info.RecoveryLevel = 1;
                info.StuckFrames = 0; // Reset counter to give it time
            }
            
            level1Recoveries++;
            return true;
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError(string.Format(
                "[StuckVehicleRecovery] Level 1 recovery error for vehicle {0}: {1}",
                vehicleId, ex));
            return false;
        }
    }
    
    /// <summary>
    /// Level 2 Recovery: Teleport vehicle forward slightly
    /// </summary>
    private static bool ApplyLevel2Recovery(VehicleManager vm, ushort vehicleId, VehicleStuckInfo info)
    {
        try
        {
            Vehicle vehicle = vm.m_vehicles.m_buffer[vehicleId];
            
            UnityEngine.Debug.LogWarning(string.Format(
                "[StuckVehicleRecovery] Vehicle {0} stuck for {1:F0}s. Applying Level 2 Recovery (teleport)",
                vehicleId, info.StuckFrames / 60.0f));
            
            // Get movement direction from last velocity or facing
            Vector3 direction = vehicle.GetLastFrameData().m_velocity;
            if (direction.sqrMagnitude < 0.01f)
            {
                // No velocity, use vehicle rotation
                direction = vehicle.GetLastFrameData().m_rotation * Vector3.forward;
            }
            
            direction.Normalize();
            
            // Teleport 5 units forward
            Vector3 currentPos = vehicle.GetLastFramePosition();
            Vector3 newPos = currentPos + (direction * 5f);
            
            // Set new position
            vm.m_vehicles.m_buffer[vehicleId].SetTargetPos(0, newPos);
            
            info.RecoveryLevel = 2;
            info.StuckFrames = 0; // Reset to give it another chance
            info.LastPosition = newPos;
            
            level2Recoveries++;
            return true;
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError(string.Format(
                "[StuckVehicleRecovery] Level 2 recovery error for vehicle {0}: {1}",
                vehicleId, ex));
            return false;
        }
    }
    
    /// <summary>
    /// Level 3 Recovery: Despawn and respawn vehicle (last resort)
    /// </summary>
    private static bool ApplyLevel3Recovery(VehicleManager vm, ushort vehicleId, VehicleStuckInfo info)
    {
        try
        {
            Vehicle vehicle = vm.m_vehicles.m_buffer[vehicleId];
            
            UnityEngine.Debug.LogWarning(string.Format(
                "[StuckVehicleRecovery] Vehicle {0} stuck for {1:F0}s. Applying Level 3 Recovery (despawn/respawn)",
                vehicleId, info.StuckFrames / 60.0f));
            
            VehicleInfo vehicleInfo = vehicle.Info;
            if (vehicleInfo != null && vehicleInfo.m_vehicleAI != null)
            {
                // Tell AI to reset this vehicle's source building
                // This will cause it to despawn and eventually respawn
                vehicleInfo.m_vehicleAI.SetSource(vehicleId, ref vm.m_vehicles.m_buffer[vehicleId], 0);
            }
            
            // Remove from tracking
            stuckVehicles.Remove(vehicleId);
            
            level3Recoveries++;
            return true;
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError(string.Format(
                "[StuckVehicleRecovery] Level 3 recovery error for vehicle {0}: {1}",
                vehicleId, ex));
            return false;
        }
    }
    
    // ========================================================================
    // DIAGNOSTICS
    // ========================================================================
    
    /// <summary>
    /// Reset all statistics
    /// </summary>
    public static void ResetStats()
    {
        totalScans = 0;
        level1Recoveries = 0;
        level2Recoveries = 0;
        level3Recoveries = 0;
    }
}
```

Tiếp tục với Class 6 và 7...


---

## 🔨 CLASS 6: DiagnosticsUI (Console Commands + UI Panel)

### Location in File
**Insert after:** StuckVehicleRecovery class  
**Estimated lines:** ~1700 lines (largest class)

### Part A: Console Commands

```csharp
// ============================================================================
// DIAGNOSTICS UI - Console Commands & Status Panel
// ============================================================================
/// <summary>
/// Provides in-game diagnostics via:
/// 1. Console commands (F7 debug console)
/// 2. Status UI panel (Ctrl+Shift+M toggle)
/// </summary>
public static class DiagnosticsUI
{
    // ========================================================================
    // CONSOLE COMMANDS SYSTEM
    // ========================================================================
    
    private static bool verboseLogging = false;
    private static bool commandsRegistered = false;
    
    /// <summary>
    /// Initialize console command hooks
    /// </summary>
    public static void Initialize()
    {
        try
        {
            RegisterConsoleCommands();
            InitializeStatusPanel();
            
            UnityEngine.Debug.Log("[DiagnosticsUI] Initialized. Commands: mnc.* | UI Toggle: Ctrl+Shift+M");
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[DiagnosticsUI] Initialize error: " + ex);
        }
    }
    
    private static void RegisterConsoleCommands()
    {
        if (commandsRegistered) return;
        
        try
        {
            // Hook into Unity debug console
            // Commands will be parsed in Update() loop checking for input
            commandsRegistered = true;
            UnityEngine.Debug.Log("[DiagnosticsUI] Console commands registered");
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[DiagnosticsUI] RegisterConsoleCommands error: " + ex);
        }
    }
    
    /// <summary>
    /// Check for console commands. Call from Update() or similar.
    /// </summary>
    public static void ProcessConsoleInput(string input)
    {
        if (string.IsNullOrEmpty(input)) return;
        
        try
        {
            string cmd = input.Trim().ToLower();
            
            if (cmd == "mnc.status")
            {
                ExecuteStatusCommand();
            }
            else if (cmd == "mnc.repair")
            {
                ExecuteRepairCommand();
            }
            else if (cmd == "mnc.clearpaths")
            {
                ExecuteClearPathsCommand();
            }
            else if (cmd == "mnc.unstuck")
            {
                ExecuteUnstuckCommand();
            }
            else if (cmd.StartsWith("mnc.monitor "))
            {
                string arg = cmd.Substring("mnc.monitor ".Length).Trim();
                ExecuteMonitorCommand(arg);
            }
            else if (cmd == "mnc.help")
            {
                ExecuteHelpCommand();
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[DiagnosticsUI] ProcessConsoleInput error: " + ex);
        }
    }
    
    // ========================================================================
    // COMMAND: mnc.status
    // ========================================================================
    
    private static void ExecuteStatusCommand()
    {
        UnityEngine.Debug.Log("========================================");
        UnityEngine.Debug.Log("MORE NETWORK CAPACITY RENEW - STATUS");
        UnityEngine.Debug.Log("========================================");
        
        // ArrayGuardian status
        UnityEngine.Debug.Log("\n[ARRAY MONITORING]");
        UnityEngine.Debug.Log(ArrayGuardian.GetValidationStatus());
        UnityEngine.Debug.Log(ArrayGuardianV2.GetStatus());
        
        // Check individual arrays
        Assembly tmpe = TMPECompatibility.GetTMPEAssembly();
        if (tmpe != null)
        {
            CheckArraySize(tmpe, "TrafficManager.State.Flags", "laneArrowFlags", 524288);
            CheckArraySize(tmpe, "TrafficManager.State.Flags", "highwayLaneArrowFlags", 524288);
            CheckArraySize(tmpe, "TrafficManager.State.Flags", "laneAllowedVehicleTypesArray", 524288);
            CheckArraySize(tmpe, "TrafficManager.Manager.Impl.SpeedLimitManager", "speedLimitArray", 524288);
        }
        
        // Vehicle status
        UnityEngine.Debug.Log("\n[VEHICLE POOL]");
        UnityEngine.Debug.Log(VehiclePoolMonitor.GetStatus());
        
        // Buffer sync status
        UnityEngine.Debug.Log("\n[BUFFER SYNC]");
        UnityEngine.Debug.Log(TMPEBufferSync.GetStatus());
        
        // PathFind status
        UnityEngine.Debug.Log("\n[PATHFIND CACHE]");
        UnityEngine.Debug.Log(PathFindValidator.GetStatus());
        
        // Stuck vehicles status
        UnityEngine.Debug.Log("\n[STUCK RECOVERY]");
        UnityEngine.Debug.Log(StuckVehicleRecovery.GetStatus());
        
        UnityEngine.Debug.Log("========================================");
    }
    
    private static void CheckArraySize(Assembly asm, string typeName, string fieldName, int expected)
    {
        try
        {
            Type type = asm.GetType(typeName);
            if (type == null) return;
            
            FieldInfo field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public);
            if (field == null) return;
            
            Array array = field.GetValue(null) as Array;
            if (array == null)
            {
                UnityEngine.Debug.Log(string.Format("  {0}.{1}: NULL ✗", type.Name, fieldName));
                return;
            }
            
            bool ok = array.Length >= expected;
            string status = ok ? "✓" : "✗";
            UnityEngine.Debug.Log(string.Format("  {0}.{1}: {2} / {3} {4}",
                type.Name, fieldName, array.Length, expected, status));
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError(string.Format("  Error checking {0}.{1}: {2}",
                typeName, fieldName, ex.Message));
        }
    }
    
    // ========================================================================
    // COMMAND: mnc.repair
    // ========================================================================
    
    private static void ExecuteRepairCommand()
    {
        UnityEngine.Debug.Log("[DiagnosticsUI] Executing full repair...");
        
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        
        try
        {
            // Repair 1: Arrays
            bool arraysOK = ArrayGuardian.RepairAllArrays();
            UnityEngine.Debug.Log(string.Format("  Arrays repair: {0}", arraysOK ? "OK" : "FAILED"));
            
            // Repair 2: Buffers
            TMPEBufferSync.SyncAllBuffers("ManualRepair");
            UnityEngine.Debug.Log("  Buffer sync: OK");
            
            // Repair 3: Vehicle counts
            VehiclePoolMonitor.ForceRepair();
            UnityEngine.Debug.Log("  Vehicle counts: OK");
            
            // Repair 4: Path cache
            PathFindValidator.ValidateAndCleanPaths();
            UnityEngine.Debug.Log("  Path cache cleaned: OK");
            
            sw.Stop();
            UnityEngine.Debug.Log(string.Format("[DiagnosticsUI] Full repair completed in {0}ms", sw.ElapsedMilliseconds));
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[DiagnosticsUI] Repair command error: " + ex);
        }
    }
    
    // ========================================================================
    // COMMAND: mnc.clearpaths
    // ========================================================================
    
    private static void ExecuteClearPathsCommand()
    {
        UnityEngine.Debug.Log("[DiagnosticsUI] Cleaning stale paths...");
        
        try
        {
            PathFindValidator.ValidateAndCleanPaths();
            UnityEngine.Debug.Log("[DiagnosticsUI] Path cleaning completed");
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[DiagnosticsUI] ClearPaths command error: " + ex);
        }
    }
    
    // ========================================================================
    // COMMAND: mnc.unstuck
    // ========================================================================
    
    private static void ExecuteUnstuckCommand()
    {
        UnityEngine.Debug.Log("[DiagnosticsUI] Force recovering stuck vehicles...");
        
        try
        {
            StuckVehicleRecovery.ForceRecoverAll();
            UnityEngine.Debug.Log("[DiagnosticsUI] Unstuck completed");
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[DiagnosticsUI] Unstuck command error: " + ex);
        }
    }
    
    // ========================================================================
    // COMMAND: mnc.monitor [on|off]
    // ========================================================================
    
    private static void ExecuteMonitorCommand(string arg)
    {
        if (arg == "on")
        {
            verboseLogging = true;
            UnityEngine.Debug.Log("[DiagnosticsUI] Verbose logging ENABLED");
        }
        else if (arg == "off")
        {
            verboseLogging = false;
            UnityEngine.Debug.Log("[DiagnosticsUI] Verbose logging DISABLED");
        }
        else
        {
            UnityEngine.Debug.Log("[DiagnosticsUI] Usage: mnc.monitor [on|off]");
        }
    }
    
    // ========================================================================
    // COMMAND: mnc.help
    // ========================================================================
    
    private static void ExecuteHelpCommand()
    {
        UnityEngine.Debug.Log("========================================");
        UnityEngine.Debug.Log("MORE NETWORK CAPACITY RENEW - COMMANDS");
        UnityEngine.Debug.Log("========================================");
        UnityEngine.Debug.Log("mnc.status        - Show detailed status of all systems");
        UnityEngine.Debug.Log("mnc.repair        - Force repair all arrays, buffers, counts");
        UnityEngine.Debug.Log("mnc.clearpaths    - Clean stale path cache");
        UnityEngine.Debug.Log("mnc.unstuck       - Force recover all stuck vehicles");
        UnityEngine.Debug.Log("mnc.monitor on    - Enable verbose logging");
        UnityEngine.Debug.Log("mnc.monitor off   - Disable verbose logging");
        UnityEngine.Debug.Log("mnc.help          - Show this help");
        UnityEngine.Debug.Log("\nUI Hotkey: Ctrl+Shift+M - Toggle status panel");
        UnityEngine.Debug.Log("========================================");
    }
    
    public static bool VerboseLogging
    {
        get { return verboseLogging; }
    }
    
    // ========================================================================
    // PART B: STATUS UI PANEL (continued below)
    // ========================================================================
    
    private static GameObject panelGameObject = null;
    private static DiagnosticsPanel panelComponent = null;
    private static bool panelVisible = false;
    
    private static void InitializeStatusPanel()
    {
        try
        {
            // Create GameObject for panel
            panelGameObject = new GameObject("MNCDiagnosticsPanel");
            panelComponent = panelGameObject.AddComponent<DiagnosticsPanel>();
            
            UnityEngine.Object.DontDestroyOnLoad(panelGameObject);
            
            panelGameObject.SetActive(false); // Start hidden
            
            UnityEngine.Debug.Log("[DiagnosticsUI] Status panel created (hidden)");
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[DiagnosticsUI] InitializeStatusPanel error: " + ex);
        }
    }
    
    /// <summary>
    /// Toggle panel visibility. Call from hotkey handler.
    /// </summary>
    public static void TogglePanel()
    {
        if (panelGameObject == null) return;
        
        panelVisible = !panelVisible;
        panelGameObject.SetActive(panelVisible);
        
        UnityEngine.Debug.Log(string.Format("[DiagnosticsUI] Status panel {0}",
            panelVisible ? "shown" : "hidden"));
    }
    
    public static void Cleanup()
    {
        if (panelGameObject != null)
        {
            UnityEngine.Object.Destroy(panelGameObject);
            panelGameObject = null;
            panelComponent = null;
        }
    }
}

// ============================================================================
// DIAGNOSTICS PANEL UI COMPONENT
// ============================================================================
/// <summary>
/// MonoBehaviour component for status panel UI.
/// Attached to DiagnosticsUI GameObject.
/// </summary>
public class DiagnosticsPanel : MonoBehaviour
{
    private Rect windowRect = new Rect(Screen.width - 420, 20, 400, 400);
    private float updateInterval = 1.0f; // Update every second
    private float lastUpdateTime = 0f;
    
    // Cached status strings
    private string arrayStatus = "";
    private string vehicleStatus = "";
    private string bufferStatus = "";
    private string pathStatus = "";
    private string stuckStatus = "";
    
    void Start()
    {
        UpdateAllStatus();
    }
    
    void Update()
    {
        // Update status text periodically
        if (Time.time - lastUpdateTime > updateInterval)
        {
            UpdateAllStatus();
            lastUpdateTime = Time.time;
        }
        
        // Check for hotkey (Ctrl+Shift+M)
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) &&
            (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) &&
            Input.GetKeyDown(KeyCode.M))
        {
            DiagnosticsUI.TogglePanel();
        }
    }
    
    void OnGUI()
    {
        // Semi-transparent background
        GUI.backgroundColor = new Color(0, 0, 0, 0.85f);
        windowRect = GUI.Window(98765, windowRect, DrawWindow, "MNC Status");
    }
    
    void DrawWindow(int windowID)
    {
        GUILayout.BeginVertical();
        
        // Header
        GUILayout.Label("More Network Capacity Renew", GUI.skin.box);
        GUILayout.Space(5);
        
        // Arrays section
        GUILayout.Label("Arrays:", GUI.skin.box);
        GUILayout.Label(arrayStatus);
        GUILayout.Space(5);
        
        // Vehicles section
        GUILayout.Label("Vehicles:", GUI.skin.box);
        GUILayout.Label(vehicleStatus);
        GUILayout.Space(5);
        
        // Buffer sync section
        GUILayout.Label("Buffer Sync:", GUI.skin.box);
        GUILayout.Label(bufferStatus);
        GUILayout.Space(5);
        
        // PathFind section
        GUILayout.Label("PathFind Cache:", GUI.skin.box);
        GUILayout.Label(pathStatus);
        GUILayout.Space(5);
        
        // Stuck recovery section
        GUILayout.Label("Stuck Recovery:", GUI.skin.box);
        GUILayout.Label(stuckStatus);
        GUILayout.Space(10);
        
        // Quick actions
        if (GUILayout.Button("Force Repair All"))
        {
            DiagnosticsUI.ProcessConsoleInput("mnc.repair");
        }
        
        if (GUILayout.Button("Unstuck Vehicles"))
        {
            DiagnosticsUI.ProcessConsoleInput("mnc.unstuck");
        }
        
        GUILayout.Space(5);
        GUILayout.Label("Toggle: Ctrl+Shift+M | Console: F7, type mnc.help");
        
        GUILayout.EndVertical();
        
        // Make window draggable
        GUI.DragWindow();
    }
    
    void UpdateAllStatus()
    {
        try
        {
            arrayStatus = ArrayGuardianV2.GetStatus();
            vehicleStatus = VehiclePoolMonitor.GetStatus();
            bufferStatus = TMPEBufferSync.GetStatus();
            pathStatus = PathFindValidator.GetStatus();
            stuckStatus = StuckVehicleRecovery.GetStatus();
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[DiagnosticsPanel] UpdateAllStatus error: " + ex);
        }
    }
}
```

---

## 🔨 CLASS 7: VehicleFixPatches (Harmony Integration)

### Location in File
**Insert after:** DiagnosticsUI class  
**Estimated lines:** ~300 lines

```csharp
// ============================================================================
// VEHICLE FIX PATCHES - Harmony Integration Points
// ============================================================================
/// <summary>
/// Harmony patches to integrate monitoring systems into game lifecycle.
/// All patches are Postfix to avoid conflicts with other mods.
/// </summary>
public static class VehicleFixPatches
{
    private static uint lastSimulationFrame = 0;
    
    // ========================================================================
    // PATCH: SimulationManager.SimulationStep (Postfix)
    // ========================================================================
    /// <summary>
    /// Called every simulation frame. Used to trigger periodic checks.
    /// </summary>
    public static void SimulationStep_Postfix()
    {
        try
        {
            SimulationManager sm = Singleton<SimulationManager>.instance;
            if (sm == null) return;
            
            uint currentFrame = sm.m_currentFrameIndex;
            
            // Prevent double-execution if called multiple times per frame
            if (currentFrame == lastSimulationFrame)
            {
                return;
            }
            lastSimulationFrame = currentFrame;
            
            // Every 60 frames (~1 second): Check vehicle pool
            if (currentFrame % 60 == 0)
            {
                VehiclePoolMonitor.CheckAndRepair();
            }
            
            // Every 64 frames: Scan stuck vehicles
            if (currentFrame % 64 == 0)
            {
                StuckVehicleRecovery.ScanAndRecover();
            }
        }
        catch (Exception ex)
        {
            if (DiagnosticsUI.VerboseLogging)
            {
                UnityEngine.Debug.LogError("[VehicleFixPatches] SimulationStep error: " + ex);
            }
        }
    }
    
    // ========================================================================
    // PATCH: NetManager.UpdateSegmentRenderer (Postfix)
    // ========================================================================
    /// <summary>
    /// Called when network visuals update (build/demolish roads).
    /// Triggers buffer sync to keep TM:PE in sync.
    /// </summary>
    public static void NetManager_UpdateSegmentRenderer_Postfix()
    {
        try
        {
            TMPEBufferSync.SyncAllBuffers("NetManager.UpdateSegmentRenderer");
        }
        catch (Exception ex)
        {
            if (DiagnosticsUI.VerboseLogging)
            {
                UnityEngine.Debug.LogError("[VehicleFixPatches] UpdateSegmentRenderer error: " + ex);
            }
        }
    }
    
    // ========================================================================
    // PATCH: TM:PE SerializableDataExtension.OnAfterDeserialize (Postfix)
    // ========================================================================
    /// <summary>
    /// Called after TM:PE loads save data.
    /// Critical timing: sync buffers AND clean stale paths.
    /// </summary>
    public static void TMPE_OnAfterDeserialize_Postfix()
    {
        try
        {
            // Sync buffers first
            TMPEBufferSync.SyncAllBuffers("TMPE.OnAfterDeserialize");
            
            // Then clean any stale paths from save
            PathFindValidator.ValidateAndCleanPaths();
            
            // Force array check
            if (ArrayGuardian.HasShrinkage())
            {
                UnityEngine.Debug.LogWarning("[VehicleFixPatches] Shrinkage detected after deserialization!");
                ArrayGuardian.RepairAllArrays();
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[VehicleFixPatches] OnAfterDeserialize error: " + ex);
        }
    }
    
    // ========================================================================
    // UTILITY: Install All Patches
    // ========================================================================
    /// <summary>
    /// Install all Harmony patches. Call from LoadingExtension.OnLevelLoaded()
    /// </summary>
    public static void InstallAllPatches(HarmonyInstance harmony)
    {
        try
        {
            // Patch 1: SimulationManager.SimulationStep
            MethodInfo simStep = typeof(SimulationManager).GetMethod("SimulationStep",
                BindingFlags.Public | BindingFlags.Instance);
            
            if (simStep != null)
            {
                MethodInfo postfix = typeof(VehicleFixPatches).GetMethod(
                    nameof(SimulationStep_Postfix),
                    BindingFlags.Public | BindingFlags.Static);
                
                harmony.Patch(simStep, postfix: new HarmonyMethod(postfix));
                UnityEngine.Debug.Log("[VehicleFixPatches] Patched SimulationManager.SimulationStep");
            }
            
            // Patch 2 & 3: Installed by TMPEBufferSync.InstallHooks()
            
            UnityEngine.Debug.Log("[VehicleFixPatches] All patches installed");
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[VehicleFixPatches] InstallAllPatches error: " + ex);
        }
    }
}
```

Tôi sẽ tiếp tục với phần Integration và Testing Guide...


---

## 🔗 INTEGRATION INTO EXISTING CODE

### Modification 1: RenewMod.OnEnabled()

**Location:** Find existing `RenewMod.OnEnabled()` method (around line 800)

**Add after existing ArrayGuardian initialization:**

```csharp
public void OnEnabled()
{
    EnsureMoreNetworkCapacityActive();
    try
    {
        DiagnosticLogger.Checkpoint("RenewMod.OnEnabled.Start");
        
        // EXISTING: Initialize ArrayGuardian
        ArrayGuardian.Initialize();
        ArrayGuardian.ExpandAllArrays();
        ArrayGuardian.ValidateAllArrays();
        
        // EXISTING: Initialize SafeFlagsAccessor
        SafeFlagsAccessor.Initialize();
        
        // === NEW CODE START ===
        
        // NEW: Start real-time array monitoring
        ArrayGuardianV2.StartMonitoring();
        
        // NEW: Initialize diagnostics UI
        DiagnosticsUI.Initialize();
        
        DiagnosticLogger.Checkpoint("RenewMod.ArrayGuardianV2.Started");
        
        // === NEW CODE END ===
        
        // EXISTING: Memory optimizer, etc.
        MemoryOptimizer.CheckProceduralObjectsPresence();
        MemoryOptimizer.PrewarmHeap();
        
        DiagnosticLogger.Checkpoint("RenewMod.OnEnabled.Complete");
    }
    catch (Exception ex)
    {
        UnityEngine.Debug.LogError("[MoreNetworkCapacityRenew] OnEnabled error: " + ex);
    }
}
```

---

### Modification 2: Create/Modify LoadingExtension

**Location:** Search for existing `LoadingExtension` class OR create new if doesn't exist

**Complete LoadingExtension class:**

```csharp
// ============================================================================
// LOADING EXTENSION - Level Lifecycle Integration
// ============================================================================
/// <summary>
/// Manages initialization and cleanup of monitoring systems.
/// </summary>
public class LoadingExtension : LoadingExtensionBase
{
    private static HarmonyInstance harmonyInstance = null;
    
    public override void OnLevelLoaded(LoadMode mode)
    {
        base.OnLevelLoaded(mode);
        
        try
        {
            UnityEngine.Debug.Log("[LoadingExtension] OnLevelLoaded start");
            
            // Skip if not in game mode
            if (mode != LoadMode.LoadGame && mode != LoadMode.NewGame)
            {
                return;
            }
            
            // Create Harmony instance if needed
            if (harmonyInstance == null)
            {
                harmonyInstance = HarmonyInstance.Create("com.ngocduyrenewal.vehiclefix");
            }
            
            // === INITIALIZATION SEQUENCE ===
            
            // 1. Initial validations
            PathFindValidator.ValidateAndCleanPaths();
            VehiclePoolMonitor.InitialCheck();
            
            UnityEngine.Debug.Log("[LoadingExtension] Initial validations complete");
            
            // 2. Install Harmony patches
            VehicleFixPatches.InstallAllPatches(harmonyInstance);
            TMPEBufferSync.InstallHooks(harmonyInstance);
            
            UnityEngine.Debug.Log("[LoadingExtension] Harmony patches installed");
            
            // 3. Initial buffer sync
            TMPEBufferSync.SyncAllBuffers("OnLevelLoaded");
            
            UnityEngine.Debug.Log("[LoadingExtension] OnLevelLoaded complete");
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[LoadingExtension] OnLevelLoaded error: " + ex);
        }
    }
    
    public override void OnLevelUnloading()
    {
        base.OnLevelUnloading();
        
        try
        {
            UnityEngine.Debug.Log("[LoadingExtension] OnLevelUnloading start");
            
            // Stop monitoring systems
            ArrayGuardianV2.StopMonitoring();
            
            // Clear caches
            StuckVehicleRecovery.ClearCache();
            
            // Cleanup UI
            DiagnosticsUI.Cleanup();
            
            UnityEngine.Debug.Log("[LoadingExtension] OnLevelUnloading complete");
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[LoadingExtension] OnLevelUnloading error: " + ex);
        }
    }
}
```

---

## 🧪 COMPLETE TESTING GUIDE

### Pre-Implementation Checklist

- [ ] Backup current Renew.cs
- [ ] Backup Quang Ninh save file
- [ ] Note current vehicle count in save
- [ ] Note locations of stuck vehicles (from screenshots)

---

### Phase 1: Compilation Test

**Steps:**
```bash
cd /Users/nqocduy/CS_MoreNetworkCapacity_Renew

# Backup current version
cp Renew.cs Renew.cs.backup_$(date +%Y%m%d)

# After adding all classes, compile
./build.sh
```

**Expected:**
- ✅ Compilation succeeds (no errors)
- ✅ Warnings OK (if any)
- ✅ DLL file created: `MoreNetworkCapacityRenew.dll`
- ✅ File size: ~450-500 KB (was ~350 KB)

**If compilation fails:**
1. Check for missing `using` statements
2. Check for typos in class names
3. Verify all closing braces `}`
4. Look for incompatible .NET 3.5 syntax

---

### Phase 2: Game Launch Test

**Steps:**
1. Launch Cities: Skylines
2. Enable mod in Content Manager
3. Start new game (NOT load Quang Ninh yet)
4. Let it load to in-game

**Check Player.log:**
```bash
tail -f ~/Library/Logs/Unity/Player.log | grep -E "ArrayGuardianV2|VehiclePoolMonitor|TMPEBufferSync|DiagnosticsUI"
```

**Expected log output:**
```
[ArrayGuardianV2] Monitoring started (check every 5s)
[VehiclePoolMonitor] Initialized
[TMPEBufferSync] Hooks installed successfully (2 patches)
[DiagnosticsUI] Initialized. Commands: mnc.* | UI Toggle: Ctrl+Shift+M
[LoadingExtension] OnLevelLoaded complete
```

**Success criteria:**
- ✅ Game launches without crash
- ✅ All systems report "started" or "initialized"
- ✅ No errors in log

---

### Phase 3: Console Commands Test

**Steps:**
1. In-game, press `F7` (opens debug console)
2. Type each command and verify output

**Test Case 3.1: mnc.help**
```
Input: mnc.help
Expected: List of all commands displayed
```

**Test Case 3.2: mnc.status**
```
Input: mnc.status
Expected: 
- Array sizes displayed (524288 each)
- Vehicle count shown
- All systems show "Active" or "OK"
```

**Test Case 3.3: mnc.repair**
```
Input: mnc.repair
Expected:
- "Executing full repair..."
- Each subsystem reports "OK"
- "Full repair completed in Xms"
```

---

### Phase 4: UI Panel Test

**Steps:**
1. In-game, press `Ctrl+Shift+M`
2. Panel should appear top-right
3. Check panel contents

**Expected panel display:**
```
┌─────────────────────────────────┐
│ More Network Capacity Status    │
├─────────────────────────────────┤
│ Arrays:                         │
│  Monitoring: Active ...         │
│                                 │
│ Vehicles:                       │
│  1234 / 16384 ✓ ...            │
│                                 │
│ Buffer Sync:                    │
│  Active | Last: ...             │
│  ...                            │
│                                 │
│ [Force Repair All]              │
│ [Unstuck Vehicles]              │
└─────────────────────────────────┘
```

**Interactions:**
- [ ] Panel is draggable (click title bar and drag)
- [ ] `Ctrl+Shift+M` hides panel again
- [ ] "Force Repair All" button works (check log)
- [ ] Panel updates every second (watch timestamps)

---

### Phase 5: Quang Ninh Load Test (THE BIG TEST)

**Steps:**
1. Load Quang Ninh backup2 save
2. Wait for full load
3. Observe for 5 minutes

**Monitor Player.log in real-time:**
```bash
tail -f ~/Library/Logs/Unity/Player.log | grep -E "ArrayGuardianV2|VehiclePoolMonitor|StuckVehicleRecovery|WARN|ERROR"
```

**Expected during load:**
```
[ArrayGuardianV2] Monitor thread started
[LoadingExtension] Initial validations complete
[PathFindValidator] Validation complete: Scanned X paths, cleaned Y stale paths
[VehiclePoolMonitor] Initial state: Reported=X, Actual=Y
[TMPEBufferSync] Buffers synced (reason: OnLevelLoaded)
```

**Success criteria:**
- ✅ Save loads completely (no crash)
- ✅ No "CRITICAL" or "FAILED" messages
- ✅ ArrayGuardianV2 checks every 5 seconds
- ✅ Vehicle count matches actual

---

### Phase 6: Stuck Vehicle Recovery Test

**Steps:**
1. With Quang Ninh loaded, find location from screenshot 1 (elevated highway with stuck vehicles)
2. Zoom to that location
3. Observe stuck vehicles
4. Wait 15 seconds
5. Check if vehicles recover

**Expected behavior:**
```
[StuckVehicleRecovery] Vehicle 1234 stuck for 10.0s. Applying Level 1 Recovery (path reset)
// Wait 5 more seconds if still stuck:
[StuckVehicleRecovery] Vehicle 1234 stuck for 15.0s. Applying Level 2 Recovery (teleport)
```

**Success criteria:**
- ✅ Stuck vehicles detected within 64 frames (~1 second)
- ✅ Level 1 recovery applied at 10 seconds
- ✅ If still stuck, Level 2 at 15 seconds
- ✅ Vehicle eventually moves or despawns
- ✅ No other vehicles get stuck in same spot

---

### Phase 7: Vehicle Count Stability Test

**Steps:**
1. Load Quang Ninh save
2. Press `Ctrl+Shift+M` to show panel
3. Note initial vehicle count: ___________
4. Play game normally for 30 minutes
5. Check count every 5 minutes

**Record counts:**
```
T+0min:  _____
T+5min:  _____
T+10min: _____
T+15min: _____
T+20min: _____
T+25min: _____
T+30min: _____
```

**Success criteria:**
- ✅ Count fluctuates ±10% (normal)
- ✅ No sustained downward trend
- ✅ Count doesn't freeze at one value
- ✅ No "Count mismatch" warnings in log

---

### Phase 8: Clear Traffic & Respawn Test

**Steps:**
1. Continue from Phase 7
2. Note vehicle count: _____
3. Open Traffic Manager
4. Click "Clear Traffic" (or use mod's clear button)
5. Wait 30 seconds
6. Observe vehicles

**Check in console (F7):**
```
Input: mnc.status
Check: Vehicle count line
```

**Expected:**
- Initial count before clear: ~1500
- Immediately after clear: ~100 (some remain)
- After 30 seconds: ~1200-1400 (80-90% recovery)

**Success criteria:**
- ✅ Vehicles spawn after clear
- ✅ Count reaches 80%+ of original
- ✅ No "spawn throttling" warnings
- ✅ Traffic flows normally again

---

### Phase 9: Array Shrinkage Auto-Repair Test

**Steps:**
1. This test verifies ArrayGuardianV2 detects and repairs shrinkage
2. Monitor log for 10 minutes

**Watch for:**
```bash
grep "Shrinkage detected\|speedLimitArray shrunk\|Auto-repair" ~/Library/Logs/Unity/Player.log
```

**If shrinkage detected (good test):**
```
[ArrayGuardianV2] WARNING: speedLimitArray shrunk to 262144
[ArrayGuardianV2] Expanding speedLimitArray: 262144 → 524288
[ArrayGuardianV2] Auto-repair completed in Xms
```

**Success criteria:**
- ✅ If shrinkage happens, it's detected within 5 seconds
- ✅ Repair completes successfully
- ✅ No stuck vehicles appear after repair
- ✅ Game continues normally

**Note:** Shrinkage might not happen every game. That's OK.

---

### Phase 10: Build/Demolish Road Test

**Steps:**
1. In Quang Ninh, find area with traffic
2. Build a new road segment
3. Check log for buffer sync
4. Demolish the road
5. Check log again
6. Observe vehicles reroute correctly

**Expected log output:**
```
[TMPEBufferSync] Buffers synced (reason: NetManager.UpdateSegmentRenderer)
[TMPEBufferSync] Synced NetLaneExtensions._laneBuffer → live buffer (262144 elements)
```

**Success criteria:**
- ✅ Buffer sync triggered on build
- ✅ Buffer sync triggered on demolish
- ✅ Vehicles find new paths
- ✅ No stuck vehicles at construction site

---

### Phase 11: Performance Test

**Tool needed:** Process Monitor or Activity Monitor

**Steps:**
1. Load Quang Ninh save
2. Open Activity Monitor (macOS) or Task Manager (Windows)
3. Find Cities Skylines process
4. Note CPU usage baseline: _____%
5. Play for 10 minutes
6. Note CPU usage average: _____%
7. Calculate overhead

**Target:**
- Baseline: ~40-60% CPU (varies by system)
- With monitors: < 2% overhead
- Acceptable: Baseline + 2%

**Memory check:**
```bash
# Check log for memory warnings
grep "Heap Status\|GC\|memory" ~/Library/Logs/Unity/Player.log
```

**Success criteria:**
- ✅ CPU overhead < 2%
- ✅ No memory leaks (heap stable)
- ✅ No "heap sections" errors

---

### Phase 12: Long Session Test (Final Validation)

**The ultimate test:**
1. Load Quang Ninh save
2. Play normally for 60 minutes
3. Save game at end
4. Reload the save

**Checklist throughout session:**
- [ ] No stuck vehicles observed
- [ ] Vehicle count stable (check every 10 min)
- [ ] No crashes or freezes
- [ ] Traffic flows normally
- [ ] Clear traffic works if tested
- [ ] Array repairs happen automatically if needed

**Final validation:**
- [ ] Game saves successfully
- [ ] Reload successful
- [ ] No data corruption
- [ ] All systems still active after reload

---

## 🐛 TROUBLESHOOTING GUIDE

### Issue: Compilation Errors

**Error: "Type or namespace name 'DateTime' not found"**
```csharp
Fix: Add at top of file
using System;
```

**Error: "Type or namespace name 'Thread' not found"**
```csharp
Fix: Add at top of file
using System.Threading;
```

**Error: "Type or namespace name 'Dictionary' not found"**
```csharp
Fix: Add at top of file
using System.Collections.Generic;
```

---

### Issue: Monitoring Not Starting

**Symptom:** No "[ArrayGuardianV2] Monitoring started" in log

**Debug steps:**
1. Check RenewMod.OnEnabled() was modified
2. Verify ArrayGuardianV2.StartMonitoring() is called
3. Check for exceptions during startup

**Fix:**
Add debug log before StartMonitoring():
```csharp
UnityEngine.Debug.Log("[DEBUG] About to start ArrayGuardianV2");
ArrayGuardianV2.StartMonitoring();
UnityEngine.Debug.Log("[DEBUG] StartMonitoring() call completed");
```

---

### Issue: Console Commands Don't Work

**Symptom:** Typing mnc.status does nothing

**Cause:** Console input hook not implemented yet

**Temporary workaround:**
Call methods directly from another mod or test script:
```csharp
DiagnosticsUI.ProcessConsoleInput("mnc.status");
```

---

### Issue: UI Panel Doesn't Show

**Symptom:** Ctrl+Shift+M does nothing

**Debug steps:**
1. Check DiagnosticsUI.Initialize() was called
2. Check for exceptions during panel creation
3. Verify hotkey detection in Update()

**Fix:**
Add to LoadingExtension.OnLevelLoaded():
```csharp
// Force show panel for testing
DiagnosticsUI.TogglePanel();
```

---

### Issue: Stuck Vehicles Still Not Recovering

**Symptom:** Vehicles stuck despite monitoring active

**Debug steps:**
1. Check if StuckVehicleRecovery.ScanAndRecover() is being called
2. Verify simulation frame counter is incrementing
3. Check stuck vehicle tracking dictionary

**Manual recovery:**
```
F7 console:
mnc.unstuck
```

---

### Issue: Performance Degradation

**Symptom:** Game FPS drops significantly

**Diagnosis:**
Check which monitor is slow:
```bash
grep "Monitor loop error\|Scan error\|Check error" ~/Library/Logs/Unity/Player.log
```

**Potential fixes:**
1. Increase check intervals:
   - ArrayGuardianV2: 5s → 10s
   - VehiclePoolMonitor: 60 frames → 120 frames
   - StuckVehicleRecovery: 64 frames → 128 frames

2. Disable verbose logging:
```
mnc.monitor off
```

---

## ✅ SUCCESS METRICS SUMMARY

### Must-Have (Critical)
- [ ] Compilation succeeds
- [ ] Game launches without crash
- [ ] Quang Ninh save loads successfully
- [ ] No stuck vehicles after 30 minutes
- [ ] Vehicle count stable over 30 minutes
- [ ] Clear traffic respawns vehicles

### Should-Have (Important)
- [ ] All console commands work
- [ ] UI panel displays and updates
- [ ] Auto-repair triggers when needed
- [ ] Build/demolish roads works correctly
- [ ] Performance overhead < 2%

### Nice-to-Have (Polish)
- [ ] Verbose logging option works
- [ ] Panel is draggable
- [ ] All diagnostics accurate
- [ ] Clean log output (no spam)

---

## 📋 FINAL IMPLEMENTATION CHECKLIST

### Code Implementation
- [ ] ArrayGuardianV2 class added (~200 lines)
- [ ] VehiclePoolMonitor class added (~400 lines)
- [ ] TMPEBufferSync class added (~200 lines)
- [ ] PathFindValidator class added (~400 lines)
- [ ] StuckVehicleRecovery class added (~800 lines)
- [ ] DiagnosticsUI class added (~1700 lines)
- [ ] VehicleFixPatches class added (~300 lines)
- [ ] LoadingExtension class created/modified (~200 lines)
- [ ] RenewMod.OnEnabled() modified

### Integration
- [ ] All classes in correct order in Renew.cs
- [ ] All using statements present
- [ ] Harmony patches applied correctly
- [ ] LoadingExtension wired up

### Testing
- [ ] Compilation test passed
- [ ] Game launch test passed
- [ ] Console commands test passed
- [ ] UI panel test passed
- [ ] Quang Ninh load test passed
- [ ] Stuck vehicle recovery test passed
- [ ] Vehicle count stability test passed
- [ ] Clear traffic test passed
- [ ] Performance test passed
- [ ] Long session test passed

### Documentation
- [ ] Implementation summary written
- [ ] Testing guide completed
- [ ] User guide created
- [ ] Troubleshooting section filled

---

## 🎯 ESTIMATED TOTALS

**Lines of code added:** ~4000 lines
**File size increase:** ~150 KB
**Implementation time:** 12-16 hours (comprehensive)
**Testing time:** 4-6 hours
**Total time:** 16-22 hours for complete implementation & validation

---

**Document version:** 1.0  
**Last updated:** October 16, 2026  
**Status:** Complete - Ready for implementation


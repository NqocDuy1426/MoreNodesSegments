# Comprehensive Vehicle Fix - Implementation Plan
**Date:** October 16, 2026  
**Approach:** Aggressive - All 6 Solutions  
**Target:** Zero stuck vehicles, stable spawning, full debug tools  

---

## 📦 DELIVERABLES OVERVIEW

### New Classes to Add (6 classes total)
1. `ArrayGuardianV2` - Real-time monitoring with background thread
2. `VehiclePoolMonitor` - Vehicle count health checking
3. `TMPEBufferSync` - Auto-sync TM:PE buffers on events
4. `PathFindValidator` - Clean stale path cache
5. `StuckVehicleRecovery` - Emergency unstuck system
6. `DiagnosticsUI` - In-game status panel + console commands

### New Harmony Patches (3 patches)
1. `SimulationManager.SimulationStep` Postfix
2. `NetManager.UpdateSegmentRenderer` Postfix  
3. `SerializableDataExtension.OnAfterDeserialize` Postfix

### Documentation Files
1. `IMPLEMENTATION_SUMMARY.md` - What was added
2. `TESTING_GUIDE.md` - How to test
3. `USER_GUIDE.md` - Console commands reference

---

## 🏗️ PHASE 1: Core Monitoring Infrastructure

### Class 1: ArrayGuardianV2
**File location:** Add after existing `SafeFlagsAccessor` class in Renew.cs

**Purpose:** Monitor arrays in background, auto-repair shrinkage

**Key components:**
```plaintext
Fields:
- monitorThread (Thread)
- isMonitoring (bool)
- checkIntervalMs = 5000 (int)
- lastCheckTime (DateTime)

Methods:
- StartMonitoring() → Create and start background thread
- StopMonitoring() → Stop thread cleanly
- MonitorLoop() → While loop checking arrays
- ValidateSpeedLimitArray() → NEW: Check speedLimitArray specifically
- ValidateTMPEArrays() → Check all TM:PE Flags arrays
- AutoRepair() → Call existing ArrayGuardian.RepairAllArrays() + sync buffers

Thread safety:
- Use lock(monitorLock) for all array access
- Background thread with IsBackground = true
- Try-catch around entire loop

Integration points:
- Call StartMonitoring() in RenewMod.OnEnabled()
- Call StopMonitoring() in existing LoadingExtension.OnLevelUnloading()
```

**Special handling for speedLimitArray:**
```plaintext
1. Get array via reflection from SpeedLimitManager
2. If length < 524288 → expand
3. Fill new slots with default: (ushort)139 (= 50 km/h)
4. This is CRITICAL - prevents speed = 0 stuck issue
```

### Class 2: VehiclePoolMonitor
**File location:** Add after ArrayGuardianV2

**Purpose:** Fix vehicle count corruption causing spawn throttling

**Key components:**
```plaintext
Fields:
- lastVehicleCount (int)
- lastCheckFrame (uint)
- stuckFrameThreshold = 600 (int) 
- consecutiveStuckChecks (int)

Methods:
- CheckAndRepair() → Called every 60 frames from simulation
- CountActualVehicles() → Loop buffer, count Created flag
- DetectCountMismatch() → Compare m_vehicleCount vs actual
- RepairVehicleCount() → Force correct count via reflection
- DetectSpawnThrottling() → Check if count frozen but should spawn
- ResetInternalCounters() → Clear m_infoCount array

Logic flow:
1. Every 60 frames:
2. Count actual vehicles in buffer
3. If mismatch with m_vehicleCount → repair
4. If count hasn't changed AND < 50% capacity → throttling
5. If throttling detected 10 times → reset counters
```

### Class 3: TMPEBufferSync  
**File location:** Add after VehiclePoolMonitor

**Purpose:** Keep TM:PE extension buffers in sync with game

**Key components:**
```plaintext
Fields:
- lastSyncTime (DateTime)
- syncCount (int)

Methods:
- InstallHooks(HarmonyInstance) → Apply Harmony patches
- SyncAllBuffers(string reason) → Do the actual sync
- NetManager_UpdateSegmentRenderer_Postfix() → Harmony callback
- TMPE_OnAfterDeserialize_Postfix() → Harmony callback

Sync logic:
1. Get NetManager.m_lanes.m_buffer reference
2. Get NetManager.m_segments.m_buffer reference
3. Get NetManager.m_nodes.m_buffer reference
4. Set TM:PE's _laneBuffer = lanes (reference assignment, not copy)
5. Set TM:PE's _segBuffer = segments
6. Set TM:PE's _nodeBuffer = nodes
7. This ensures TM:PE always reads live data

Harmony patch targets:
- NetManager.UpdateSegmentRenderer (called when roads change)
- SerializableDataExtension.OnAfterDeserialize (called after load)
```

---

## 🏗️ PHASE 2: Path & Recovery Systems

### Class 4: PathFindValidator
**File location:** Add after TMPEBufferSync

**Purpose:** Remove stale paths causing routing errors

**Key components:**
```plaintext
Fields:
- lastValidationTime (DateTime)
- pathsCleaned (int)
- validationInProgress (bool)

Methods:
- ValidateAndCleanPaths() → Main validation method
- IsPathValid(PathUnit) → Check individual path
- IsLaneValid(uint laneId) → Check lane exists and created
- IsSegmentValid(ushort segId) → Check segment exists

Validation algorithm:
1. Loop all PathManager.m_pathUnits.m_buffer
2. For each path with FLAG_CREATED:
   a. Loop through all positions in path
   b. Check lane ID < NetManager.m_lanes.m_buffer.Length
   c. Check segment ID < NetManager.m_segments.m_buffer.Length
   d. Check lane has Created flag
   e. If any check fails → mark path invalid
3. Release all invalid paths: pm.ReleasePath(pathId)

Triggers:
- OnLevelLoaded (initial cleanup)
- After TM:PE deserialization
- User command: mnc.clearpaths
- Can be slow (100-200ms) so not every frame
```

### Class 5: StuckVehicleRecovery
**File location:** Add after PathFindValidator

**Purpose:** Last resort to unstuck frozen vehicles

**Key components:**
```plaintext
Fields:
- stuckVehicles (Dictionary<ushort, StuckInfo>)
- scanIntervalFrames = 64 (int)
- lastScanFrame (uint)

StuckInfo struct:
- lastPosition (Vector3)
- stuckFrames (int)
- pathUnit (uint)
- recoveryLevel (int)

Methods:
- ScanAndRecover() → Called every 64 frames
- CheckVehicleMovement(ushort vehicleId) → Has it moved?
- ApplyRecovery(ushort vehicleId, int level) → Escalating fixes
- ClearCache() → Reset tracking

Recovery escalation:
Level 1 (600 frames = 10s stuck):
  - Release current path
  - Set m_path = 0
  - Vehicle will request new path

Level 2 (900 frames = 15s stuck):
  - Teleport vehicle 5 units forward
  - Use GetLastFrameData().m_velocity for direction

Level 3 (1200 frames = 20s stuck):
  - Despawn vehicle
  - Call VehicleAI.SetSource(vehicleId, 0)
  - Will respawn at source building

Threshold detection:
- If distance moved < 0.1m AND velocity < 0.1
- Increment stuckFrames
- If moved → reset stuckFrames = 0
```

---

## 🏗️ PHASE 3: User Diagnostics & Tools

### Class 6: DiagnosticsUI
**File location:** Add after StuckVehicleRecovery

**Purpose:** In-game UI + console commands for debugging

**Components:**

#### A. Console Commands System
```plaintext
Commands to implement:

1. mnc.status
   Output:
   - Array sizes: laneArrowFlags, speedLimitArray, etc.
   - Vehicle count: m_vehicleCount vs actual
   - Stuck vehicles: count from StuckVehicleRecovery
   - Monitor status: ArrayGuardianV2 running?, last check time
   - Last repair: timestamp, reason

2. mnc.repair
   Actions:
   - Call ArrayGuardian.RepairAllArrays()
   - Call TMPEBufferSync.SyncAllBuffers("Manual")
   - Call VehiclePoolMonitor.RepairVehicleCount()
   - Output: "Repair completed: X arrays fixed, Y buffers synced"

3. mnc.clearpaths
   Actions:
   - Call PathFindValidator.ValidateAndCleanPaths()
   - Output: "Cleaned X stale paths"

4. mnc.unstuck
   Actions:
   - Force StuckVehicleRecovery.ScanAndRecover()
   - Apply Level 1 recovery to all stuck > 5s
   - Output: "Recovered X vehicles"

5. mnc.monitor [on|off]
   Actions:
   - Enable/disable verbose logging from monitors
   - Set static bool DiagnosticsUI.VerboseLogging

Implementation:
- Hook into Debug Console (F7)
- Parse input with string.StartsWith("mnc.")
- Use reflection to access DebugOutputPanel.AddMessage()
```

#### B. In-Game Status Panel
```plaintext
UI Layout (top-right corner):

┌─────────────────────────────────┐
│ More Network Capacity Status    │
├─────────────────────────────────┤
│ Arrays:                         │
│  └ laneArrowFlags: 524288 ✓     │
│  └ speedLimitArray: 524288 ✓    │
│  └ _laneBuffer: synced ✓        │
│                                 │
│ Vehicles:                       │
│  └ Count: 1234 / 1234 ✓         │
│  └ Stuck: 0                     │
│                                 │
│ Monitors:                       │
│  └ ArrayGuardian: Active        │
│  └ Last check: 2s ago           │
│  └ Last repair: Never           │
│                                 │
│ [Toggle: Ctrl+Shift+M]          │
└─────────────────────────────────┘

Features:
- Semi-transparent (alpha 0.85)
- Draggable
- Toggle with Ctrl+Shift+M hotkey
- Auto-update every 1 second
- Green checkmarks when OK, red X when issues

Implementation:
- Use ColossalFramework.UI.UIPanel
- Create in MonoBehaviour subclass
- Update() method refreshes every second
- UIButton for toggle
- UILabel for each stat line
```

---

## 🔗 INTEGRATION POINTS

### Modified: RenewMod.OnEnabled()
```plaintext
After existing ArrayGuardian.Initialize():

// NEW: Start real-time monitoring
ArrayGuardianV2.StartMonitoring();

// NEW: Initialize diagnostics UI
DiagnosticsUI.Initialize();
```

### Modified: LoadingExtension (create if doesn't exist)
```plaintext
public class LoadingExtension : LoadingExtensionBase
{
    public override void OnLevelLoaded(LoadMode mode)
    {
        // Existing checks...
        
        // NEW: Initial validations
        PathFindValidator.ValidateAndCleanPaths();
        VehiclePoolMonitor.InitialCheck();
        
        // NEW: Install sync hooks
        var harmony = HarmonyInstance.Create("com.ngocduyrenewal.vehiclefix");
        TMPEBufferSync.InstallHooks(harmony);
        InstallSimulationHooks(harmony);
    }
    
    public override void OnLevelUnloading()
    {
        // NEW: Stop monitoring
        ArrayGuardianV2.StopMonitoring();
        StuckVehicleRecovery.ClearCache();
        DiagnosticsUI.Cleanup();
    }
    
    private void InstallSimulationHooks(HarmonyInstance harmony)
    {
        // Patch SimulationManager.SimulationStep
        var simStep = typeof(SimulationManager).GetMethod("SimulationStep", 
            BindingFlags.Public | BindingFlags.Instance);
        var postfix = typeof(VehicleFixPatches).GetMethod("SimulationStep_Postfix");
        harmony.Patch(simStep, postfix: new HarmonyMethod(postfix));
    }
}
```

### New: VehicleFixPatches class
```plaintext
Add to TMPEPatches section or create new class:

public static class VehicleFixPatches
{
    private static uint lastFrame = 0;
    
    public static void SimulationStep_Postfix()
    {
        try
        {
            uint currentFrame = Singleton<SimulationManager>.instance.m_currentFrameIndex;
            
            // Every 60 frames: check vehicle pool
            if (currentFrame % 60 == 0)
            {
                VehiclePoolMonitor.CheckAndRepair();
            }
            
            // Every 64 frames: scan stuck vehicles
            if (currentFrame % 64 == 0)
            {
                StuckVehicleRecovery.ScanAndRecover();
            }
            
            lastFrame = currentFrame;
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[VehicleFixPatches] SimulationStep error: " + ex);
        }
    }
    
    public static void NetManager_UpdateSegmentRenderer_Postfix()
    {
        TMPEBufferSync.SyncAllBuffers("NetManager.UpdateSegmentRenderer");
    }
    
    public static void TMPE_OnAfterDeserialize_Postfix()
    {
        TMPEBufferSync.SyncAllBuffers("TMPE.OnAfterDeserialize");
        PathFindValidator.ValidateAndCleanPaths();
    }
}
```

---

## 🧪 TESTING PROTOCOL

### Test 1: Monitor Startup
**Steps:**
1. Add debug logs to StartMonitoring()
2. Launch game, enable mod
3. Check Player.log

**Expected:**
```
[ArrayGuardianV2] Monitoring started
[VehiclePoolMonitor] Initialized
[TMPEBufferSync] Hooks installed (2 patches)
[DiagnosticsUI] Initialized, press Ctrl+Shift+M
```

### Test 2: Array Shrinkage Auto-Repair
**Steps:**
1. Load Quang Ninh save
2. Wait 30 seconds (6 monitor checks)
3. Check logs for repairs

**Expected if shrinkage detected:**
```
[ArrayGuardianV2] WARNING: speedLimitArray shrunk to 262144
[ArrayGuardianV2] Expanding speedLimitArray: 262144 → 524288
[ArrayGuardianV2] Filled 262144 slots with default speed (139)
[ArrayGuardianV2] Repair complete
```

### Test 3: Stuck Vehicle Recovery
**Steps:**
1. Find stuck vehicle in Quang Ninh (from screenshots)
2. Observe for 15 seconds
3. Check if it recovers

**Expected:**
```
[StuckVehicleRecovery] Vehicle 1234 stuck for 10s! Applying Level 1 recovery
[StuckVehicleRecovery] Released path for vehicle 1234
// Wait 5 more seconds if still stuck:
[StuckVehicleRecovery] Vehicle 1234 stuck for 15s! Applying Level 2 recovery
[StuckVehicleRecovery] Teleported vehicle 1234 forward 5 units
```

### Test 4: Vehicle Count Stability
**Steps:**
1. Note initial count: Open F7 console, type `mnc.status`
2. Play 30 minutes
3. Check count again

**Expected:**
- Count should stay within ±5% of initial
- No warnings about count mismatch in logs

### Test 5: Clear Traffic Respawn
**Steps:**
1. Type `mnc.status` → note vehicle count
2. Use Traffic Manager to clear all traffic
3. Wait 5 minutes
4. Type `mnc.status` again

**Expected:**
- Count returns to 80-90% of original
- No "spawn throttling detected" warnings

### Test 6: Console Commands
**Steps:**
```
F7 console:
mnc.status     → Should show detailed stats
mnc.repair     → Should trigger repairs
mnc.clearpaths → Should clean paths
mnc.unstuck    → Should recover vehicles
```

**Expected:**
- Each command outputs confirmation
- No exceptions in log

### Test 7: UI Panel
**Steps:**
1. Press Ctrl+Shift+M
2. Panel appears top-right
3. All stats show green checkmarks
4. Press Ctrl+Shift+M again → panel hides

**Expected:**
- Panel appears/disappears smoothly
- Stats update every second
- No performance impact

---

## 📊 PERFORMANCE TARGETS

### CPU Overhead (measured with profiler)
- ArrayGuardianV2: < 0.3% (only checks every 5s)
- VehiclePoolMonitor: < 0.2% (checks every 60 frames)
- StuckVehicleRecovery: < 0.5% (scans every 64 frames)
- TMPEBufferSync: < 0.1% (only on events)
- PathFindValidator: < 0.2% (infrequent)
- DiagnosticsUI: < 0.1% (UI update)
- **Total: < 1.4% CPU overhead**

### Memory Overhead
- ArrayGuardianV2: ~1KB (thread stack + fields)
- VehiclePoolMonitor: ~100 bytes
- StuckVehicleRecovery: ~10KB (Dictionary tracking ~100 vehicles)
- TMPEBufferSync: ~100 bytes
- PathFindValidator: ~100 bytes
- DiagnosticsUI: ~50KB (UI components)
- **Total: < 100KB memory overhead**

### Response Times
- Array shrinkage detection: < 5 seconds
- Vehicle count fix: < 1 second
- Stuck vehicle recovery: 10-20 seconds (by design)
- Buffer sync: < 1ms (reference assignment)
- Path validation: 100-200ms (acceptable, infrequent)

---

## ⚠️ KNOWN RISKS & MITIGATIONS

### Risk 1: Thread Safety Issues
**Problem:** Background thread accessing game objects
**Mitigation:**
- Use locks around all shared data access
- Don't call Unity API from background thread
- Only read/write via reflection with try-catch

### Risk 2: Performance Degradation
**Problem:** Too much scanning/checking
**Mitigation:**
- Tune check intervals (5s for arrays is safe)
- Only scan stuck vehicles every 64 frames
- PathFind validation only when needed, not every frame

### Risk 3: Harmony Patch Conflicts
**Problem:** Other mods patching same methods
**Mitigation:**
- Use Postfix instead of Prefix where possible
- Always wrap in try-catch
- Log errors clearly

### Risk 4: Edge Cases Not Covered
**Problem:** Specific scenarios cause stuck vehicles
**Mitigation:**
- Multiple layers of defense (6 different systems)
- Emergency recovery as last resort
- User can manually trigger with commands

---

## 📝 DOCUMENTATION CHECKLIST

### Implementation Summary
- [ ] List all classes added
- [ ] List all Harmony patches added
- [ ] Document integration points
- [ ] Include line count changes

### Testing Guide  
- [ ] 7 test scenarios with steps
- [ ] Expected log outputs
- [ ] Performance measurement instructions
- [ ] Regression test checklist

### User Guide
- [ ] Console commands reference
- [ ] UI panel screenshot/description
- [ ] Hotkey list (Ctrl+Shift+M)
- [ ] Troubleshooting section

---

## 🚀 IMPLEMENTATION ORDER

### Day 1: Core Infrastructure (Phase 1)
1. Add ArrayGuardianV2 class → Test compilation
2. Add VehiclePoolMonitor class → Test compilation
3. Add TMPEBufferSync class → Test compilation
4. Integrate in RenewMod.OnEnabled() → Test in-game

### Day 2: Recovery Systems (Phase 2)
1. Add PathFindValidator class → Test compilation
2. Add StuckVehicleRecovery class → Test compilation
3. Add VehicleFixPatches → Apply Harmony patches → Test in-game

### Day 3: User Tools (Phase 3)
1. Add DiagnosticsUI console commands → Test with F7
2. Add DiagnosticsUI panel → Test UI appearance
3. Full integration test on Quang Ninh save

### Day 4: Documentation & Polish
1. Write IMPLEMENTATION_SUMMARY.md
2. Write TESTING_GUIDE.md
3. Write USER_GUIDE.md
4. Final testing & bug fixes

---

## ✅ COMPLETION CRITERIA

- [ ] All 6 classes implemented and compiling
- [ ] All 3 Harmony patches applied
- [ ] Monitors start/stop cleanly
- [ ] Console commands work
- [ ] UI panel toggles and updates
- [ ] Quang Ninh save loads without crashes
- [ ] No stuck vehicles after 30 min play
- [ ] Vehicle count stays stable
- [ ] Clear traffic works
- [ ] Performance < 2% CPU overhead
- [ ] All documentation complete

---

**Ready to begin implementation!**

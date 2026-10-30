# QUICK REFERENCE CARD - Vehicle Fix Implementation

**🎯 Goal:** Fix stuck vehicles, stabilize spawning, add debug tools  
**📅 Time:** 16-22 hours (full implementation + testing)  
**📚 Main Guide:** DETAILED_IMPLEMENTATION_GUIDE.md  

---

## 📋 IMPLEMENTATION CHECKLIST (Print This!)

### Day 1: Core Infrastructure ✅
```
□ Backup Renew.cs
□ Add ArrayGuardianV2 class (after SafeFlagsAccessor)
□ Add VehiclePoolMonitor class
□ Add TMPEBufferSync class
□ Compile: ./build.sh
□ Test: Launch game, check log for "Monitoring started"
```

### Day 2: Recovery Systems ✅
```
□ Add PathFindValidator class
□ Add StuckVehicleRecovery class
□ Add VehicleFixPatches class
□ Compile: ./build.sh
□ Test: Load Quang Ninh, check for errors
```

### Day 3: User Tools ✅
```
□ Add DiagnosticsUI class
□ Add DiagnosticsPanel class
□ Compile: ./build.sh
□ Test: F7 → mnc.help
□ Test: Ctrl+Shift+M → panel shows
```

### Day 4: Integration ✅
```
□ Modify RenewMod.OnEnabled()
□ Create LoadingExtension class
□ Compile: ./build.sh
□ Test: Full 30-min gameplay
□ Test: All 12 test phases
```

---

## 🔧 7 CLASSES TO ADD

| # | Class Name | Lines | Purpose |
|---|-----------|-------|---------|
| 1 | ArrayGuardianV2 | ~200 | Real-time array monitoring |
| 2 | VehiclePoolMonitor | ~400 | Vehicle count health |
| 3 | TMPEBufferSync | ~200 | Auto-sync buffers |
| 4 | PathFindValidator | ~400 | Clean stale paths |
| 5 | StuckVehicleRecovery | ~800 | Emergency unstuck |
| 6 | DiagnosticsUI | ~1700 | Console commands + UI |
| 7 | VehicleFixPatches | ~300 | Harmony integration |

**Total:** ~4000 lines of new code

---

## 🎮 CONSOLE COMMANDS

```bash
# Open debug console
F7

# Commands (type exactly)
mnc.help          # Show all commands
mnc.status        # System status
mnc.repair        # Force repair
mnc.clearpaths    # Clean paths
mnc.unstuck       # Recover vehicles
mnc.monitor on    # Verbose logging
mnc.monitor off   # Disable verbose
```

---

## ⌨️ HOTKEYS

```
Ctrl+Shift+M    Toggle diagnostics panel
F7              Open debug console
```

---

## 🧪 ESSENTIAL TESTS

### ✅ Test 1: Compilation
```bash
cd /Users/nqocduy/CS_MoreNetworkCapacity_Renew
./build.sh
# Expected: "Build successful"
```

### ✅ Test 2: Game Launch
```bash
# Launch game, check log
tail -f ~/Library/Logs/Unity/Player.log | grep "ArrayGuardianV2\|DiagnosticsUI"
# Expected: "Monitoring started" + "Initialized"
```

### ✅ Test 3: Commands
```
In-game:
F7 → mnc.status
Expected: Detailed status output
```

### ✅ Test 4: UI Panel
```
In-game:
Ctrl+Shift+M
Expected: Panel appears top-right
```

### ✅ Test 5: Quang Ninh Load
```
Load save, wait 5 mins
Check: No stuck vehicles
Check: Vehicle count stable
```

---

## 🐛 QUICK TROUBLESHOOTING

| Problem | Quick Fix |
|---------|-----------|
| Won't compile | Check `using` statements at file top |
| Game crashes | Check Player.log, restore backup |
| Commands don't work | Verify DiagnosticsUI.Initialize() called |
| Panel doesn't show | Check for exceptions in log |
| Still stuck vehicles | Run `mnc.repair` then `mnc.unstuck` |
| Performance issues | Increase check intervals |

---

## 📊 SUCCESS METRICS

### Critical ✅
- [x] Compiles without errors
- [x] Game launches
- [x] Quang Ninh loads
- [x] Zero stuck vehicles @ 30min
- [x] Vehicle count stable

### Important ✅
- [x] All commands work
- [x] UI panel functions
- [x] Performance < 2% overhead

---

## 📁 FILE LOCATIONS

```
Main Guide:
/Users/nqocduy/CS_MoreNetworkCapacity_Renew/.agents/tasks/vehicle-fix/DETAILED_IMPLEMENTATION_GUIDE.md

Code to modify:
/Users/nqocduy/CS_MoreNetworkCapacity_Renew/Renew.cs

Backup location:
/Users/nqocduy/CS_MoreNetworkCapacity_Renew/Renew.cs.backup_YYYYMMDD

Build script:
/Users/nqocduy/CS_MoreNetworkCapacity_Renew/build.sh

Player log:
~/Library/Logs/Unity/Player.log
```

---

## 🔍 LOG MONITORING

### Real-time (keep open during testing)
```bash
tail -f ~/Library/Logs/Unity/Player.log | grep -E "ArrayGuardianV2|VehiclePoolMonitor|StuckVehicleRecovery|ERROR"
```

### Check for errors
```bash
grep ERROR ~/Library/Logs/Unity/Player.log | tail -20
```

### Success indicators
```
[ArrayGuardianV2] Monitoring started
[VehiclePoolMonitor] Initialized
[TMPEBufferSync] Hooks installed
[DiagnosticsUI] Initialized
[LoadingExtension] OnLevelLoaded complete
```

---

## 💾 BACKUP COMMANDS

```bash
# Before starting
cd /Users/nqocduy/CS_MoreNetworkCapacity_Renew
cp Renew.cs Renew.cs.backup_$(date +%Y%m%d_%H%M%S)

# Restore if needed
cp Renew.cs.backup_YYYYMMDD_HHMMSS Renew.cs
```

---

## 🎯 INTEGRATION POINTS

### Add to RenewMod.OnEnabled()
```csharp
// After existing ArrayGuardian.Initialize()
ArrayGuardianV2.StartMonitoring();
DiagnosticsUI.Initialize();
```

### Create LoadingExtension
```csharp
public class LoadingExtension : LoadingExtensionBase
{
    public override void OnLevelLoaded(LoadMode mode)
    {
        // Install patches, sync buffers
        VehicleFixPatches.InstallAllPatches(harmony);
        TMPEBufferSync.InstallHooks(harmony);
    }
    
    public override void OnLevelUnloading()
    {
        ArrayGuardianV2.StopMonitoring();
        DiagnosticsUI.Cleanup();
    }
}
```

---

## 📈 WHAT GETS FIXED

| Problem | Root Cause | Solution |
|---------|-----------|----------|
| Stuck vehicles | Speed=0 on high-ID lanes | ArrayGuardianV2 auto-repair |
| Count decreasing | m_vehicleCount corruption | VehiclePoolMonitor fixes |
| No respawn after clear | Spawn throttling | VehiclePoolMonitor resets |
| Stale routing | Buffer desync | TMPEBufferSync auto-syncs |
| Invalid paths | Old lane references | PathFindValidator cleans |

---

## ⏱️ TIME ESTIMATES

- Class 1-3: 4-6 hours
- Class 4-5: 4-6 hours  
- Class 6-7: 4-6 hours
- Integration: 1-2 hours
- Testing: 4-6 hours

**Total:** 16-22 hours

---

## 🚨 CRITICAL NOTES

1. **Thread Safety:** Always use `lock` in ArrayGuardianV2
2. **Frame Timing:** Check intervals carefully (60, 64 frames)
3. **Reflection:** Cache FieldInfo for performance
4. **.NET 3.5:** No `Task`, use `Thread`
5. **Harmony 2.2:** Use `HarmonyInstance`, not `Harmony`

---

## 📞 HELP RESOURCES

**Stuck?** Read:
1. DETAILED_IMPLEMENTATION_GUIDE.md § Troubleshooting
2. Check Player.log for exceptions
3. Run `mnc.status` to see system states
4. Review test phases you haven't run

---

## ✅ FINAL VALIDATION

```bash
# After everything is done

1. ./build.sh → Success?
2. Launch game → No crash?
3. F7 → mnc.status → Shows data?
4. Ctrl+Shift+M → Panel shows?
5. Load Quang Ninh → Loads?
6. Play 30 mins → No stuck vehicles?
7. Check vehicle count → Stable?
8. Clear traffic → Respawns?

All YES? ✅ DONE!
```

---

**Print this card and keep it handy during implementation!**

📄 Full details: DETAILED_IMPLEMENTATION_GUIDE.md  
📋 Overview: IMPLEMENTATION_PLAN.md  
🏠 Start here: README.md

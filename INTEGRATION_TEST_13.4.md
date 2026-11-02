# Integration Test Guide: Task 13.4 - Configuration Hot-Reload

## Test Objective
Validate that the ConfigManager hot-reload functionality works correctly in Cities: Skylines, allowing configuration changes without game restart.

**Requirement:** 9.7  
**Task:** 13.4

## Prerequisites
1. Cities: Skylines installed
2. More Network Capacity Renew mod installed
3. Text editor for modifying configuration files
4. Ability to view Unity console logs (F7 in-game with mod developer console)

## Test Environment
- **Config file location:** `~/.local/share/Colossal Order/Cities_Skylines/.kiro/MorePropsCompatibility.config`
- **Hot-reload hotkey:** `Ctrl+Shift+F12`
- **Log prefix:** `[MNC-Compat]`

## Test Procedures

### Test 1: Load Game with Default Configuration

**Steps:**
1. Delete config file if it exists:
   ```bash
   rm ~/.local/share/Colossal\ Order/Cities_Skylines/.kiro/MorePropsCompatibility.config
   ```

2. Launch Cities: Skylines

3. Load any city or start new game

**Expected Result:**
- Config file created at expected location
- Unity console shows: `[MNC-Compat] ConfigManager initialized. Config path: <path>`
- Unity console shows: `[MNC-Compat] Default configuration created.`
- Default values loaded:
  - `EnableCoordination: true`
  - `SerializationLockTimeoutSeconds: 60`
  - `EnableDebugLogging: false`

**Verification:**
```bash
cat ~/.local/share/Colossal\ Order/Cities_Skylines/.kiro/MorePropsCompatibility.config
```

### Test 2: Modify Config File - Change Timeout

**Steps:**
1. With game running, alt-tab to terminal

2. Edit config file:
   ```bash
   # Open in your favorite editor
   nano ~/.local/share/Colossal\ Order/Cities_Skylines/.kiro/MorePropsCompatibility.config
   ```

3. Change timeout value:
   ```json
   {
     "EnableCoordination": true,
     "SerializationLockTimeoutSeconds": 90,  // Changed from 60
     ...
   }
   ```

4. Save file

5. Return to game

6. Press `Ctrl+Shift+F12`

**Expected Result:**
- Unity console shows: `[MNC-Compat] Reloading configuration...`
- Unity console shows: `[MNC-Compat] Configuration loaded successfully.`
- Unity console shows: `[MNC-Compat] Configuration reloaded successfully.`
- Unity console shows: `SerializationLockTimeoutSeconds: 90`

**Verification:**
- Check logs confirm reload
- Next save operation should use 90-second timeout (verify in SaveCoordinator logs)

### Test 3: Modify Config File - Enable Debug Logging

**Steps:**
1. Edit config file (game still running):
   ```json
   {
     ...
     "EnableDebugLogging": true,  // Changed from false
     ...
   }
   ```

2. Press `Ctrl+Shift+F12` in game

**Expected Result:**
- Unity console shows: `[MNC-Compat] Debug logging changed: False -> True`
- Unity console shows: `[MNC-Compat] Configuration reloaded successfully.`
- Detailed debug logs immediately start appearing:
  - `[SaveCoordinator]` messages with timestamps
  - `[GridCoordinator]` detailed operation logs
  - `[MNC-Compat]` coordination event logs

**Verification:**
- Perform any mod operation (save, load) and verify debug logs appear
- Debug logs should include thread IDs and timestamps

### Test 4: Hot-Reload Without Restart

**Steps:**
1. Note current game state (city population, time, camera position)

2. Edit config file, change multiple values:
   ```json
   {
     "SerializationLockTimeoutSeconds": 120,
     "GridSanitizationDelayMs": 100,
     "OrphanedSaveRecoveryDelaySeconds": 5,
     "EnableDebugLogging": true
   }
   ```

3. Press `Ctrl+Shift+F12`

4. Continue gameplay immediately

**Expected Result:**
- Configuration reloaded instantly
- No game pause or lag
- All values take effect immediately
- Game state unchanged (no restart occurred)
- Unity console shows all changed values logged

**Verification:**
- City continues running normally
- New configuration values active immediately
- No need to restart game

### Test 5: Invalid Configuration - Missing File

**Steps:**
1. With game running, delete config file:
   ```bash
   rm ~/.local/share/Colossal\ Order/Cities_Skylines/.kiro/MorePropsCompatibility.config
   ```

2. Press `Ctrl+Shift+F12` in game

**Expected Result:**
- Unity console shows: `[MNC-Compat] Config file not found. Creating default configuration.`
- Unity console shows: `[MNC-Compat] Default configuration created.`
- Default config file recreated
- Game continues normally with default values

**Verification:**
```bash
cat ~/.local/share/Colossal\ Order/Cities_Skylines/.kiro/MorePropsCompatibility.config
# Should show default values
```

### Test 6: Invalid Configuration - Invalid JSON

**Steps:**
1. Edit config file with invalid JSON:
   ```
   { this is not valid JSON ][[ }
   ```

2. Press `Ctrl+Shift+F12` in game

**Expected Result:**
- Unity console shows: `[MNC-Compat] Failed to load configuration: <error>. Using defaults.`
- Unity console shows: `[MNC-Compat] Default configuration created.`
- Config file overwritten with valid default JSON
- Game continues normally

**Verification:**
- Config file now contains valid JSON
- Default values restored

### Test 7: Invalid Configuration - Out of Range Values

**Steps:**
1. Edit config file with invalid values:
   ```json
   {
     "SerializationLockTimeoutSeconds": 999,  // Max is 300
     "GridSanitizationDelayMs": 10000,        // Max is 5000
     ...
   }
   ```

2. Press `Ctrl+Shift+F12` in game

**Expected Result:**
- Unity console shows: `[MNC-Compat] Invalid configuration values detected. Using defaults.`
- Unity console shows: `[MNC-Compat] Default configuration created.`
- Config file overwritten with valid defaults
- Game continues normally

**Verification:**
- Config file contains valid values within allowed ranges

### Test 8: Config Changes During Gameplay

**Scenario:** Player wants to debug a save issue mid-game

**Steps:**
1. Start playing normally (population growing, traffic flowing)

2. Encounter a save operation that seems slow

3. Alt-tab, edit config:
   ```json
   {
     "EnableDebugLogging": true,
     "SerializationLockTimeoutSeconds": 120
   }
   ```

4. Return to game, press `Ctrl+Shift+F12`

5. Trigger another save operation

**Expected Result:**
- Debug logs immediately appear
- Next save operation logs detailed timing information
- Longer timeout prevents timeout warnings
- No game disruption

**Verification:**
- Unity console shows detailed save coordination logs
- Timeout applied immediately to next save

### Test 9: Multiple Reload Cycles

**Steps:**
1. Perform 5 consecutive hot-reloads:
   - Edit config, change timeout to 60s, reload
   - Edit config, change timeout to 75s, reload
   - Edit config, change timeout to 90s, reload
   - Edit config, toggle debug logging, reload
   - Edit config, restore defaults, reload

**Expected Result:**
- Each reload completes successfully
- Each reload logs confirmation message
- Each reload shows changed values
- No memory leaks or performance degradation
- Game remains stable

**Verification:**
- Unity console shows 5 "Configuration reloaded successfully" messages
- Each reload shows updated values
- Game performance unchanged

### Test 10: Configuration Persistence Across Sessions

**Steps:**
1. With game running, set custom configuration:
   ```json
   {
     "EnableDebugLogging": true,
     "SerializationLockTimeoutSeconds": 90
   }
   ```

2. Press `Ctrl+Shift+F12` to reload

3. Save game and exit Cities: Skylines completely

4. Restart Cities: Skylines

5. Load a city

**Expected Result:**
- Unity console shows configuration loaded from file (not defaults)
- Custom values from previous session still active
- Unity console shows: `SerializationLockTimeoutSeconds: 90`
- Unity console shows: `EnableDebugLogging: true`

**Verification:**
- Custom configuration persisted across game restart
- No need to reconfigure

## Success Criteria

All tests must pass with the following outcomes:

✅ Default configuration created automatically  
✅ Configuration hot-reload works via Ctrl+Shift+F12  
✅ Changes apply immediately without restart  
✅ Invalid configurations fall back to defaults gracefully  
✅ Multiple reload cycles work without issues  
✅ Debug logging toggles immediately  
✅ Timeout changes apply to next save operation  
✅ Configuration persists across game sessions  
✅ No crashes or errors during any test  
✅ Unity console logs confirm all operations  

## Common Issues and Troubleshooting

### Issue: Hotkey doesn't work
- **Cause:** Input.GetKeyDown not registering
- **Solution:** Ensure RenewMod.OnUpdate() is executing (check for other logs)
- **Workaround:** Manually call ConfigManager.ReloadConfiguration() via console

### Issue: Config file not found after reload
- **Cause:** Permission or path issue
- **Solution:** Check directory permissions: `~/.local/share/Colossal Order/Cities_Skylines/.kiro/`
- **Verification:** Try creating file manually

### Issue: Changes don't take effect
- **Cause:** Old config cached
- **Solution:** Restart game and verify ConfigManager.Initialize() runs
- **Check:** Unity console for "ConfigManager initialized" message

### Issue: Game crashes on reload
- **Cause:** JSON parsing error not caught
- **Solution:** Review ConfigManager.ReloadConfiguration() exception handling
- **Workaround:** Restore default config manually

## Test Results Template

```
Date: ___________
Tester: ___________
Environment: macOS / Windows / Linux
Game Version: ___________
Mod Version: ___________

Test 1 - Load Default Config:               [ ] Pass [ ] Fail
Test 2 - Change Timeout:                    [ ] Pass [ ] Fail
Test 3 - Enable Debug Logging:              [ ] Pass [ ] Fail
Test 4 - Hot-Reload Without Restart:        [ ] Pass [ ] Fail
Test 5 - Invalid Config - Missing File:     [ ] Pass [ ] Fail
Test 6 - Invalid Config - Invalid JSON:     [ ] Pass [ ] Fail
Test 7 - Invalid Config - Out of Range:     [ ] Pass [ ] Fail
Test 8 - Config Changes During Gameplay:    [ ] Pass [ ] Fail
Test 9 - Multiple Reload Cycles:            [ ] Pass [ ] Fail
Test 10 - Persistence Across Sessions:      [ ] Pass [ ] Fail

Overall Result: [ ] All Pass [ ] Some Fail

Notes:
_______________________________________
_______________________________________
```

## Automated Test Output

The standalone test harness (`test_config_hotreload.exe`) validates the configuration file format and operations outside of Cities: Skylines. Run it with:

```bash
cd CS_MoreNetworkCapacity_Renew
mono test_config_hotreload.exe
```

This test validates:
- JSON serialization/deserialization
- Configuration validation logic
- File I/O operations
- Round-trip property
- Default value generation

**Integration testing in Cities: Skylines is still required** to validate:
- Unity Input system (hotkey detection)
- Unity console logging
- Runtime configuration changes
- Actual save/load coordination with new values

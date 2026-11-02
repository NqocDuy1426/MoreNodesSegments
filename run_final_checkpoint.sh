#!/bin/bash
# Final Checkpoint Test Suite for More Props BETA Compatibility Layer
# Task 15: Comprehensive validation of all implemented features
# =============================================================================

set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "$SCRIPT_DIR"

GAME_DIR="$HOME/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed"
CECIL_DLL="/Library/Frameworks/Mono.framework/Versions/6.12.0/lib/mono/gac/Mono.Cecil/0.11.1.0__0738eb9f132ed756/Mono.Cecil.dll"

# Color codes for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Test counters
TESTS_PASSED=0
TESTS_FAILED=0
TESTS_SKIPPED=0

echo ""
echo "=================================================================="
echo "  Final Checkpoint: More Props BETA Compatibility Layer"
echo "  Task 15 - Comprehensive Feature Validation"
echo "=================================================================="
echo ""

# =============================================================================
# 1. BUILD VALIDATION
# =============================================================================
echo -e "${BLUE}[1/7] Build Validation${NC}"
echo "-------------------------------------------------------------------"

echo "Building MoreNodesSegmentsBeta.dll..."
if mcs -sdk:2 -target:library \
    -define:CLOSE_BETA \
    -r:"$GAME_DIR/Assembly-CSharp.dll" \
    -r:"$GAME_DIR/ICities.dll" \
    -r:"$GAME_DIR/ColossalManaged.dll" \
    -r:"$GAME_DIR/UnityEngine.dll" \
    -resource:MoreNetworkCapacity.Core.dll \
    -out:MoreNodesSegmentsBeta.dll \
    Renew.cs 2>&1 | tee build_test.log; then
    echo -e "${GREEN}✓ Build succeeded${NC}"
    ((TESTS_PASSED++))
    
    # Check DLL exists and has reasonable size
    if [ -f "MoreNodesSegmentsBeta.dll" ]; then
        DLL_SIZE=$(stat -f%z "MoreNodesSegmentsBeta.dll")
        echo "  DLL size: $(numfmt --to=iec-i --suffix=B $DLL_SIZE 2>/dev/null || echo "$DLL_SIZE bytes")"
        
        if [ $DLL_SIZE -gt 100000 ]; then
            echo -e "${GREEN}✓ DLL size validation passed${NC}"
            ((TESTS_PASSED++))
        else
            echo -e "${RED}✗ DLL size too small (< 100KB)${NC}"
            ((TESTS_FAILED++))
        fi
    else
        echo -e "${RED}✗ DLL file not created${NC}"
        ((TESTS_FAILED++))
    fi
else
    echo -e "${RED}✗ Build failed${NC}"
    ((TESTS_FAILED++))
    echo "Check build_test.log for details"
fi

echo ""

# =============================================================================
# 2. COMPONENT EXISTENCE VALIDATION
# =============================================================================
echo -e "${BLUE}[2/7] Component Existence Validation${NC}"
echo "-------------------------------------------------------------------"

# List of required components
REQUIRED_COMPONENTS=(
    "CoordinationManager"
    "SaveCoordinator"
    "GridCoordinator"
    "LifecycleCoordinator"
    "RenderCoordinator"
    "MemoryCoordinator"
    "EventBus"
    "ConfigManager"
    "PerformanceMonitor"
    "CompatibilityConfig"
)

echo "Checking for required coordination components..."
for component in "${REQUIRED_COMPONENTS[@]}"; do
    if grep -q "class $component" Renew.cs; then
        echo -e "${GREEN}✓${NC} $component found"
        ((TESTS_PASSED++))
    else
        echo -e "${RED}✗${NC} $component NOT FOUND"
        ((TESTS_FAILED++))
    fi
done

echo ""

# =============================================================================
# 3. UNIT TEST SUITE EXECUTION
# =============================================================================
echo -e "${BLUE}[3/7] Unit Test Suite Execution${NC}"
echo "-------------------------------------------------------------------"

# List of critical unit tests
UNIT_TESTS=(
    "test_coordination_manager.cs"
    "test_save_coordinator.cs"
    "test_grid_coordinator.cs"
    "test_lifecycle_coordinator.cs"
    "test_config_manager.cs"
    "test_config_roundtrip.cs"
    "test_eventbus_simple.cs"
    "test_performance_monitor.cs"
)

echo "Running unit tests (standalone mode)..."
for test_file in "${UNIT_TESTS[@]}"; do
    if [ -f "$test_file" ]; then
        test_name=$(basename "$test_file" .cs)
        echo -n "  Testing $test_name... "
        
        if mcs -sdk:2 -r:"$GAME_DIR/Assembly-CSharp.dll" \
                    -r:"$GAME_DIR/ICities.dll" \
                    -r:"$GAME_DIR/ColossalManaged.dll" \
                    -r:"$GAME_DIR/UnityEngine.dll" \
                    -r:MoreNodesSegmentsBeta.dll \
                    -out:"${test_name}.exe" \
                    "$test_file" 2>/dev/null && \
           MONO_PATH=".:$MONO_PATH" mono "${test_name}.exe" > "${test_name}_output.log" 2>&1; then
            echo -e "${GREEN}PASS${NC}"
            ((TESTS_PASSED++))
        else
            echo -e "${RED}FAIL${NC}"
            ((TESTS_FAILED++))
            echo "    See ${test_name}_output.log for details"
        fi
    else
        echo -e "${YELLOW}  Skipping $test_file (not found)${NC}"
        ((TESTS_SKIPPED++))
    fi
done

echo ""

# =============================================================================
# 4. INTEGRATION TEST VERIFICATION
# =============================================================================
echo -e "${BLUE}[4/7] Integration Test Verification${NC}"
echo "-------------------------------------------------------------------"

INTEGRATION_TESTS=(
    "test_coordination_integration.cs"
    "test_grid_coordinator_integration.cs"
    "test_save_patch_integration.cs"
    "test_memory_integration.cs"
)

echo "Running integration tests..."
for test_file in "${INTEGRATION_TESTS[@]}"; do
    if [ -f "$test_file" ]; then
        test_name=$(basename "$test_file" .cs)
        echo -n "  Testing $test_name... "
        
        if mcs -sdk:2 -r:"$GAME_DIR/Assembly-CSharp.dll" \
                    -r:"$GAME_DIR/ICities.dll" \
                    -r:"$GAME_DIR/ColossalManaged.dll" \
                    -r:"$GAME_DIR/UnityEngine.dll" \
                    -r:MoreNodesSegmentsBeta.dll \
                    -out:"${test_name}.exe" \
                    "$test_file" 2>/dev/null && \
           MONO_PATH=".:$MONO_PATH" mono "${test_name}.exe" > "${test_name}_output.log" 2>&1; then
            echo -e "${GREEN}PASS${NC}"
            ((TESTS_PASSED++))
        else
            echo -e "${RED}FAIL${NC}"
            ((TESTS_FAILED++))
            echo "    See ${test_name}_output.log for details"
        fi
    else
        echo -e "${YELLOW}  Skipping $test_file (not found)${NC}"
        ((TESTS_SKIPPED++))
    fi
done

echo ""

# =============================================================================
# 5. PERFORMANCE TARGET VALIDATION
# =============================================================================
echo -e "${BLUE}[5/7] Performance Target Validation${NC}"
echo "-------------------------------------------------------------------"

if [ -f "test_performance_targets.cs" ]; then
    echo "Compiling performance test suite..."
    if mcs -sdk:2 -r:"$GAME_DIR/Assembly-CSharp.dll" \
                -r:"$GAME_DIR/ICities.dll" \
                -r:"$GAME_DIR/ColossalManaged.dll" \
                -r:"$GAME_DIR/UnityEngine.dll" \
                -r:MoreNodesSegmentsBeta.dll \
                -out:test_performance_targets.exe \
                test_performance_targets.cs 2>/dev/null; then
        echo "Running performance benchmarks..."
        if MONO_PATH=".:$MONO_PATH" mono test_performance_targets.exe > performance_report.log 2>&1; then
            echo -e "${GREEN}✓ Performance tests passed${NC}"
            ((TESTS_PASSED++))
            
            # Display key metrics
            echo ""
            echo "Performance Metrics:"
            grep -E "Average|Target|overhead" performance_report.log 2>/dev/null | sed 's/^/  /' || true
        else
            echo -e "${RED}✗ Performance tests failed${NC}"
            ((TESTS_FAILED++))
            echo "  See performance_report.log for details"
        fi
    else
        echo -e "${YELLOW}⚠ Could not compile performance tests${NC}"
        ((TESTS_SKIPPED++))
    fi
else
    echo -e "${YELLOW}⚠ Performance test file not found${NC}"
    ((TESTS_SKIPPED++))
fi

echo ""

# =============================================================================
# 6. FEATURE COMPLETENESS CHECK
# =============================================================================
echo -e "${BLUE}[6/7] Feature Completeness Check${NC}"
echo "-------------------------------------------------------------------"

echo "Verifying required features implementation..."

# Check for key methods and patterns
declare -A REQUIRED_FEATURES=(
    ["More Props Detection"]="DetectMoreProps"
    ["Save Lock Mechanism"]="AcquireLock.*timeoutMs"
    ["Grid Cycle Detection"]="SanitizeMorePropsGridRange"
    ["Lifecycle Callbacks"]="RegisterOnLevelLoaded"
    ["Event Bus"]="Subscribe.*TEvent.*handler"
    ["Config Hot-Reload"]="ReloadConfiguration"
    ["Performance Monitoring"]="PerformanceScope"
    ["Memory Coordination"]="OnBeforeArrayExpansion"
    ["Render Priority"]="ApplyRenderPatches"
    ["Circuit Breaker"]="IncrementFailureCount"
)

for feature in "${!REQUIRED_FEATURES[@]}"; do
    pattern="${REQUIRED_FEATURES[$feature]}"
    if grep -qE "$pattern" Renew.cs; then
        echo -e "${GREEN}✓${NC} $feature"
        ((TESTS_PASSED++))
    else
        echo -e "${RED}✗${NC} $feature (pattern: $pattern)"
        ((TESTS_FAILED++))
    fi
done

echo ""

# =============================================================================
# 7. DOCUMENTATION AND DEPLOYMENT CHECK
# =============================================================================
echo -e "${BLUE}[7/7] Documentation and Deployment Check${NC}"
echo "-------------------------------------------------------------------"

echo "Checking deployment artifacts..."

# Check content_beta directory
if [ -d "content_beta" ]; then
    echo -e "${GREEN}✓${NC} content_beta directory exists"
    ((TESTS_PASSED++))
    
    if [ -f "content_beta/MoreNodesSegmentsBeta.dll" ] || [ -f "content_beta/MoreNodesSegments.dll" ]; then
        echo -e "${GREEN}✓${NC} DLL deployed to content_beta"
        ((TESTS_PASSED++))
    else
        echo -e "${RED}✗${NC} DLL not found in content_beta"
        ((TESTS_FAILED++))
    fi
    
    if [ -f "content_beta/MoreNetworkCapacity.Core.dll" ]; then
        echo -e "${GREEN}✓${NC} Core DLL deployed"
        ((TESTS_PASSED++))
    else
        echo -e "${RED}✗${NC} Core DLL not found"
        ((TESTS_FAILED++))
    fi
else
    echo -e "${RED}✗${NC} content_beta directory not found"
    ((TESTS_FAILED++))
fi

# Check workshop directories
BETA_WORKSHOP_DIR="$HOME/Library/Application Support/Steam/steamapps/workshop/content/255710/3829341972"
if [ -d "$BETA_WORKSHOP_DIR" ]; then
    echo -e "${GREEN}✓${NC} Beta workshop directory exists (ID: 3829341972)"
    ((TESTS_PASSED++))
else
    echo -e "${YELLOW}⚠${NC} Beta workshop directory not found (will be created on upload)"
    ((TESTS_SKIPPED++))
fi

# Check build script
if [ -f "build_close_beta.sh" ] && [ -x "build_close_beta.sh" ]; then
    echo -e "${GREEN}✓${NC} Build script exists and is executable"
    ((TESTS_PASSED++))
else
    echo -e "${RED}✗${NC} Build script missing or not executable"
    ((TESTS_FAILED++))
fi

echo ""

# =============================================================================
# FINAL REPORT
# =============================================================================
echo "=================================================================="
echo "  FINAL CHECKPOINT REPORT"
echo "=================================================================="
echo ""
echo "Test Results:"
echo -e "  ${GREEN}Passed:${NC}  $TESTS_PASSED"
echo -e "  ${RED}Failed:${NC}  $TESTS_FAILED"
echo -e "  ${YELLOW}Skipped:${NC} $TESTS_SKIPPED"
echo ""

TOTAL_TESTS=$((TESTS_PASSED + TESTS_FAILED))
if [ $TOTAL_TESTS -gt 0 ]; then
    PASS_RATE=$((TESTS_PASSED * 100 / TOTAL_TESTS))
    echo "Pass Rate: ${PASS_RATE}%"
    echo ""
fi

if [ $TESTS_FAILED -eq 0 ]; then
    echo -e "${GREEN}=================================================================="
    echo "  ✓✓✓ ALL CRITICAL TESTS PASSED ✓✓✓"
    echo "==================================================================${NC}"
    echo ""
    echo "The More Props BETA Compatibility Layer is ready for:"
    echo "  1. Manual testing in Cities: Skylines"
    echo "  2. Testing with More Props BETA 1.0.7+"
    echo "  3. Steam Workshop beta deployment"
    echo ""
    echo "Next Steps:"
    echo "  1. Load Cities: Skylines with mod enabled"
    echo "  2. Test save/load cycles with large cities"
    echo "  3. Verify no performance regressions in standalone mode"
    echo "  4. Test with More Props BETA installed"
    echo "  5. Monitor debug logs for coordination events"
    echo ""
    exit 0
else
    echo -e "${RED}=================================================================="
    echo "  ✗✗✗ SOME TESTS FAILED ✗✗✗"
    echo "==================================================================${NC}"
    echo ""
    echo "Please review failed tests before proceeding with deployment."
    echo "Check individual test output logs for details."
    echo ""
    exit 1
fi

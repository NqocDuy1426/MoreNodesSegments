#!/bin/bash
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "$SCRIPT_DIR"

GAME_DIR="$HOME/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed"
CECIL_DLL="/Library/Frameworks/Mono.framework/Versions/6.12.0/lib/mono/gac/Mono.Cecil/0.11.1.0__0738eb9f132ed756/Mono.Cecil.dll"

echo "=========================================================="
echo "🚀 Building More Nodes/Segments [Close Beta] v5.0.25..."
echo "=========================================================="

# 0. Compile and run test suite
echo "0️⃣ Running Test Suite..."
mcs -r:"$CECIL_DLL" -r:"0Harmony.dll" run_tests.cs -out:run_tests.exe
MONO_PATH=".:$MONO_PATH" mono run_tests.exe
echo "✅ Test Suite passed 100%."

# 1. Compile MoreNodesSegmentsBeta.dll
echo "1️⃣ Compiling MoreNodesSegmentsBeta.dll..."
mcs -sdk:2 -target:library \
    -define:CLOSE_BETA \
    -r:"$GAME_DIR/Assembly-CSharp.dll" \
    -r:"$GAME_DIR/ICities.dll" \
    -r:"$GAME_DIR/ColossalManaged.dll" \
    -r:"$GAME_DIR/UnityEngine.dll" \
    -r:"0Harmony.dll" \
    -resource:MoreNetworkCapacity.Core.dll \
    -out:MoreNodesSegmentsBeta.dll \
    Renew.cs

echo "✅ Compiled MoreNodesSegmentsBeta.dll successfully."

# 2. Deploy to content_beta
echo "2️⃣ Deploying to content_beta..."
mkdir -p "$SCRIPT_DIR/content_beta"
rm -f "$SCRIPT_DIR/content_beta/MoreNodesSegments.dll"
cp MoreNodesSegmentsBeta.dll "$SCRIPT_DIR/content_beta/"
cp MoreNetworkCapacity.Core.dll "$SCRIPT_DIR/content_beta/"
[ -f preview-image.png ] && cp preview-image.png "$SCRIPT_DIR/content_beta/PreviewImage.png"

# 3. Deploy to local workshop cache if exists
BETA_WORKSHOP_DIR="$HOME/Library/Application Support/Steam/steamapps/workshop/content/255710/3829341972"
if [ -d "$BETA_WORKSHOP_DIR" ]; then
    echo "📦 Updating local workshop cache ($BETA_WORKSHOP_DIR)..."
    rm -f "$BETA_WORKSHOP_DIR/MoreNodesSegments.dll"
    cp MoreNodesSegmentsBeta.dll "$BETA_WORKSHOP_DIR/"
    cp MoreNetworkCapacity.Core.dll "$BETA_WORKSHOP_DIR/"
    [ -f preview-image.png ] && cp preview-image.png "$BETA_WORKSHOP_DIR/PreviewImage.png"
fi

echo "=========================================================="
echo "🎉 CLOSE BETA BUILD COMPLETE!"
ls -lh "$SCRIPT_DIR/content_beta"
echo "=========================================================="

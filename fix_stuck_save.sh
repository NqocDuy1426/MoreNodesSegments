#!/bin/bash
# Fix for Cities Skylines stuck save packaging
# Run this script when save hangs with loading cursor

echo "🔍 Checking for stuck save files..."

TEMP_DIR="$HOME/Library/Application Support/Colossal Order/Cities_Skylines/Saves/Temp"
SAVE_DIR="$HOME/Library/Application Support/Colossal Order/Cities_Skylines/Saves"

cd "$TEMP_DIR" 2>/dev/null || {
    echo "❌ Temp directory not found!"
    exit 1
}

# Find the most recent .ccs file
LATEST_CCS=$(ls -t *.ccs 2>/dev/null | head -1)

if [ -z "$LATEST_CCS" ]; then
    echo "❌ No .ccs files found in Temp folder"
    echo "   Save may have already completed, or hasn't started yet"
    exit 1
fi

echo "✅ Found: $LATEST_CCS"
echo "📊 Size: $(du -h "$LATEST_CCS" | cut -f1)"
echo "🕐 Modified: $(stat -f "%Sm" -t "%Y-%m-%d %H:%M:%S" "$LATEST_CCS")"

# Extract filename without extension
BASENAME="${LATEST_CCS%.ccs}"
TIMESTAMP=$(date +%Y%m%d_%H%M%S)
OUTPUT_NAME="${BASENAME}_Fixed_${TIMESTAMP}.crp"

echo ""
echo "📦 Converting to .crp format..."
cp "$LATEST_CCS" "$SAVE_DIR/$OUTPUT_NAME"

if [ $? -eq 0 ]; then
    echo "✅ SUCCESS!"
    echo "📁 Saved to: $OUTPUT_NAME"
    echo ""
    echo "You can now:"
    echo "  1. Press ESC to exit the stuck save dialog in game"
    echo "  2. Load the fixed save from the load menu"
    echo ""
    ls -lh "$SAVE_DIR/$OUTPUT_NAME"
else
    echo "❌ Failed to copy file"
    exit 1
fi

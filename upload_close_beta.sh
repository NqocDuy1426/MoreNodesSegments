#!/bin/bash
set -e
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "$SCRIPT_DIR"

echo "=========================================================="
echo "📦 Uploading More Nodes/Segments [Close Beta] (Friends Only)..."
echo "=========================================================="

arch -x86_64 /usr/local/Caskroom/steamcmd/1788292693/MacOS/steamcmd.sh \
    +login ngocduy1426 "DuyHUST2006@" \
    +workshop_build_item "$SCRIPT_DIR/workshop_close_beta.vdf" \
    +quit

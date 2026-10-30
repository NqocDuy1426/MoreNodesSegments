with open("slm_methods.txt") as f:
    text = f.read()

for part in text.split("=== SpeedLimitManager."):
    if not part.strip(): continue
    lines = part.splitlines()
    name = lines[0]
    if name.startswith("GetGameSpeedLimit") or name.startswith("CacheSegmentSpeeds"):
        print(f"--- {name} ---")
        for l in lines[1:50]:
            print(l)

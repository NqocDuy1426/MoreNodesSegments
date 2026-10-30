with open("/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp", "rb") as f:
    f.seek(657) # SimulationManager.Data start
    # Let us search all occurrences of "PathManager" in the entire file
    data = f.read()

# Let us find where "PathManager" occurs as a string
enc_pm = bytes.fromhex("d0 11 13 f4 e5 14 0d f3 06 fe 0d")
pos = 0
while True:
    idx = data.find(enc_pm, pos)
    if idx == -1: break
    print(f"Found encoded PathManager at pos {657+idx} (0x{657+idx:x})")
    # check 10 bytes before
    before = data[max(0, idx-10):idx]
    print("  before:", " ".join(f"{b:02x}" for b in before))
    pos = idx + len(enc_pm)

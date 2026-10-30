import sys, struct

def read_string(f):
    length = 0
    shift = 0
    while True:
        b = f.read(1)
        if not b: return ""
        val = b[0]
        length |= (val & 0x7F) << shift
        if (val & 0x80) == 0:
            break
        shift += 7
    return f.read(length).decode("utf-8", errors="replace")

with open("/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp", "rb") as f:
    f.seek(370) # Data start
    # Let's search for "TrafficManager" in the stream
    data = f.read(56325319)
    print("Read data chunk, size:", len(data))
    
    # Search for TM:PE keys in serializable data storage
    idx = 0
    tmpe_keys = []
    while True:
        idx = data.find(b"TrafficManager", idx)
        if idx == -1: break
        # Extract surrounding string
        start = max(0, idx - 10)
        end = min(len(data), idx + 80)
        snippet = data[idx:end].split(b'\x00')[0]
        try:
            s = snippet.decode('ascii', errors='ignore')
            if s not in tmpe_keys:
                tmpe_keys.append(s)
        except:
            pass
        idx += 14

    print("Found TM:PE keys/references:")
    for k in tmpe_keys:
        print(" ", k)

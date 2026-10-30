import struct

path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp"
with open(path, "rb") as f:
    f.seek(370) # Skip header
    data = f.read(50000)

pos = 0
# Read version
v0, v1 = struct.unpack_from(">HH", data, pos)
pos += 4
print(f"Version: {v0}.{v1}")

# Let's find strings in the data
import re
strings = re.findall(b"[a-zA-Z0-9_.+`<>]{4,}", data[:20000])
for s in strings:
    st = s.decode('ascii', errors='ignore')
    if "Manager" in st or "AI" in st or "Vehicle" in st or "Simulation" in st:
        print("Found:", st)

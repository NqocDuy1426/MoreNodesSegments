import struct

with open("/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp", "rb") as f:
    f.seek(657)
    data = f.read(45760920 - 657)

# Let's count how many times 0xFE appears followed by a valid unique string
# Unique string format:
# b0, b1, b2
# if b0 != 255:
#   len = (b0 << 16) | (b1 << 8) | b2
#   if (len & 0x800000) != 0: len &= 0x7FFFFF, ascii chars
#   else: unicode chars

print("Data length between 657 and PathManager:", len(data))

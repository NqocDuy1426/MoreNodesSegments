import struct

with open("/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp", "rb") as f:
    f.seek(370)
    data = f.read()

# In DataSerializer:
# ReadSharedString():
# byte tag = ReadByte()
# if tag == 255 -> null
# if tag == 254 -> ReadUniqueString(), add to m_stringList
# else -> index = (tag << 8) | ReadByte(), return m_stringList[index]

# Let us find ALL occurrences of 0xFE followed by string in the entire save file
# But unique strings also appear in ReadUniqueString() directly (not via ReadSharedString)
# How to distinguish?
# Notice: BeginSerialize / EndSerialize on LoadingProfiler calls:
# WriteSharedString(name)!

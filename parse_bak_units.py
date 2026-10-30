# Let's find PathManager in bak_original
import struct

with open("/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp.bak_original", "rb") as f:
    data = f.read()

# In bak_original, we know it ended at 58979323 with 00 2B!
# And at 58978740 was the 7th-to-last unit!
end_pos = 58979323
print("Searching for units in bak_original before 58979323...")

# Let's trace backwards from 58978740 to see how many units there were

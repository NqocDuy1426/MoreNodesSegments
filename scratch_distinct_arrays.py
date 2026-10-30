with open("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/all_array_accesses_detailed.txt") as f:
    text = f.read()

blocks = text.split("\n\n")
arrays = set()
for b in blocks:
    if not b.strip(): continue
    lines = b.strip().split("\n")
    # find array source
    arr = "unknown"
    for l in reversed(lines[1:]):
        l_str = l.strip()
        if "ldloc" in l_str or "ldfld" in l_str or "ldsfld" in l_str:
            arr = l_str
            break
    arrays.add((lines[0], arr))

for header, arr in sorted(arrays):
    print(header, "-->", arr)

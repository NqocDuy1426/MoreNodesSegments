with open("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/all_array_accesses_detailed.txt") as f:
    text = f.read()

blocks = text.split("\n\n")
for b in blocks:
    if not b.strip(): continue
    lines = b.strip().split("\n")
    # check if any preceding line is ldloc.1, ldloc.2, ldloc.3
    is_main = False
    for l in lines[1:]:
        if any(x in l for x in ["ldloc.1", "ldloc.2", "ldloc.3"]):
            is_main = True
            break
    if not is_main:
        print(b)
        print("-" * 40)

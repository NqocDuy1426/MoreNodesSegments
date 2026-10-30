with open("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/net_des_full.txt") as f:
    lines = f.readlines()

calls = set()
for l in lines:
    if "call " in l or "callvirt " in l:
        part = l.strip().split("call ")[-1].split("callvirt ")[-1]
        calls.add(part)

for c in sorted(calls):
    print(c)

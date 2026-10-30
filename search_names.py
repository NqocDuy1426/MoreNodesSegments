with open("/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp", "rb") as f:
    data = f.read()
    for name in ["Cầu Đức Anh", "Cao tốc Không Gian", "Đại lộ Cối Xay Gió"]:
        bname = name.encode("utf-8")
        pos = data.find(bname)
        print(f"Name '{name}': found={pos != -1} at pos={pos}")

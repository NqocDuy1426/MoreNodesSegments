import sys, struct

def read_string(f):
    # .NET BinaryReader.ReadString: 7-bit encoded int length then utf-8 bytes
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

def parse_crp(filepath):
    print("========================================")
    print("File:", filepath)
    with open(filepath, "rb") as f:
        magic = f.read(4)
        if magic != b"CRAP":
            print("Not a CRP file! Magic:", magic)
            return
        ver = struct.unpack("<H", f.read(2))[0]
        pkg_name = read_string(f)
        author = read_string(f)
        pkg_ver = struct.unpack("<I", f.read(4))[0]
        main_asset = read_string(f)
        asset_count = struct.unpack("<i", f.read(4))[0]
        data_start = struct.unpack("<q", f.read(8))[0]
        print(f"Version: {ver}, PackageName: {pkg_name}, Author: {author}, PkgVer: {pkg_ver}")
        print(f"MainAsset: {main_asset}, AssetCount: {asset_count}, DataStart: {data_start}")
        for i in range(asset_count):
            name = read_string(f)
            checksum = read_string(f)
            atype = struct.unpack("<i", f.read(4))[0]
            offset = struct.unpack("<q", f.read(8))[0]
            size = struct.unpack("<q", f.read(8))[0]
            abs_offset = data_start + offset
            print(f"  [{i}] '{name}' (type={atype}): offset={offset} (abs={abs_offset}), size={size}")

parse_crp("/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp")
parse_crp("/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp.bak_original")

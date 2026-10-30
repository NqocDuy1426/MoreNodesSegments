import sys, struct

def read_str(f):
    b = f.read(1)
    if not b: return ""
    l = b[0]
    if l & 0x80:
        l2 = f.read(1)[0]
        l = (l & 0x7F) | (l2 << 7)
    return f.read(l).decode('utf-8', 'ignore')

path = sys.argv[1]
with open(path, 'rb') as f:
    magic = f.read(4)
    ver = struct.unpack('<H', f.read(2))[0]
    pkg_name = read_str(f)
    author = read_str(f)
    pkg_ver = struct.unpack('<I', f.read(4))[0]
    main_asset = read_str(f)
    count = struct.unpack('<i', f.read(4))[0]
    offset_base = struct.unpack('<q', f.read(8))[0]
    print(f"Pkg: '{pkg_name}', Ver: {ver}, Count: {count}, Base: {offset_base}")
    for i in range(count):
        name = read_str(f)
        chk = read_str(f)
        t = struct.unpack('<i', f.read(4))[0]
        off = struct.unpack('<q', f.read(8))[0]
        sz = struct.unpack('<q', f.read(8))[0]
        print(f"  [{i}] Name='{name}', Type={t}, AbsOff={offset_base + off}, Size={sz}")


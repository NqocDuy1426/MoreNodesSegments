import struct

def parse_units(filepath, start_pos, end_pos):
    with open(filepath, "rb") as f:
        f.seek(start_pos)
        unit_count = 0
        while f.tell() < end_pos:
            pos = f.tell()
            b = f.read(1)
            if not b: break
            path_flags = b[0]
            lane_types = f.read(1)[0]
            veh_types = struct.unpack("<I", f.read(4))[0]
            pos_count = f.read(1)[0]
            
            # ReadUInt24
            u24_bytes = f.read(3)
            next_pu = u24_bytes[0] | (u24_bytes[1] << 8) | (u24_bytes[2] << 16)
            build_idx = struct.unpack("<I", f.read(4))[0]
            length = struct.unpack("<f", f.read(4))[0]
            speed = f.read(1)[0]
            
            # positions: 4 bytes each (ushort segment, byte offset, byte lane)
            positions = []
            for k in range(pos_count):
                seg = struct.unpack("<H", f.read(2))[0]
                off = f.read(1)[0]
                lane = f.read(1)[0]
                positions.append((seg, off, lane))
            
            unit_count += 1
            if unit_count <= 5 or unit_count >= 118195:
                print(f"Unit #{unit_count} at offset {pos}: next={next_pu}, buildIdx={build_idx}, len={length:.1f}, posCount={pos_count}")
        
        print(f"Total units parsed: {unit_count}, ending offset: {f.tell()}")

parse_units("/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp", 46184575, 54370769)

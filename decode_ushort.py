import struct

def decode_ushort_stream(filepath, start_pos):
    with open(filepath, "rb") as f:
        f.seek(start_pos)
        
        m_prevValue = 0
        m_prevDelta = 0
        m_sameCount = 0
        m_closeCount = 0
        m_fullCount = 0
        
        read_count = 0
        active_indices = []
        
        while True:
            # Replicate UShort.Read()
            if m_sameCount > 0:
                m_prevValue = (m_prevValue + m_prevDelta) & 0xFFFF
                m_sameCount -= 1
                val = m_prevValue
            elif m_closeCount > 0:
                b = f.read(1)
                if not b: break
                delta_b = struct.unpack("b", b)[0]
                m_prevDelta = (m_prevDelta + delta_b) & 0xFFFF
                if m_prevDelta >= 0x8000: m_prevDelta -= 0x10000
                m_prevValue = (m_prevValue + m_prevDelta) & 0xFFFF
                m_closeCount -= 1
                val = m_prevValue
            elif m_fullCount > 0:
                b2 = f.read(2)
                if len(b2) < 2: break
                u16 = struct.unpack("<H", b2)[0]
                m_prevDelta = (u16 - m_prevValue) & 0xFFFF
                if m_prevDelta >= 0x8000: m_prevDelta -= 0x10000
                m_prevValue = (m_prevValue + m_prevDelta) & 0xFFFF
                m_fullCount -= 1
                val = m_prevValue
            else:
                b = f.read(1)
                if not b: break
                code = b[0]
                if (code & 0x80) != 0:
                    m_sameCount = (code & 0x7F) - 1
                elif (code & 0x40) != 0:
                    m_closeCount = (code & 0x3F) - 1
                else:
                    m_fullCount = code - 1
                continue # continue loop to process count
            
            read_count += 1
            if val != 0:
                active_indices.append((read_count, val, f.tell()))
                
            if read_count == 524287:
                pos524k = f.tell()
                print(f"Reached 524,287 items at stream offset {pos524k}!")
                print(f"Total active items up to 524,287: {len(active_indices)}")
                print(f"Last 5 active indices: {active_indices[-5:]}")
                # check m_sameCount, m_closeCount, m_fullCount
                print(f"Remaining counts: same={m_sameCount}, close={m_closeCount}, full={m_fullCount}")
                break

decode_ushort_stream("/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp", 45760922)

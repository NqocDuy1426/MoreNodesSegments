# Phân tích Chuyên sâu Cơ chế Kẹt Xe & Deadlock tại Ring Road 2 (Save QuangNinh backup3)

## 1. Bản chất Hiện tượng
- **Vị trí:** Đường Vành đai 2 (Ring Road 2 - cao tốc trên cao 4 làn) đoạn nhập làn từ nhánh "Cầu Dốc Ánh" (on-ramp 1 làn).
- **Triệu chứng:** Hàng trăm xe nối đuôi nhau đứng bất động ("cứ lì ra đấy") trước nút giao dù phía trước đường cao tốc hoàn toàn thông thoáng 100%. Xe không chịu despawn dù game **không hề bật** tính năng cấm despawn ("disable despawn" = OFF).

---

<!--  -->

## 2. Nguyên nhân Gốc rễ ở Mức Bit & Engine Code

### A. Lỗi So sánh Tuyệt đối của Engine Vanilla (`m_blockCounter == 100/150`)
Trong mã nguồn gốc `CarAI.SimulationStep` của Cities: Skylines (disassembly tại `IL_02F8` / `bne.un`):
```csharp
int blockLimit = (privateServiceIndex == -1) ? 150 : 100;
if (data.m_blockCounter == blockLimit) // SO SÁNH TUYỆT ĐỐI (==), KHÔNG PHẢI >=
{
    VehicleManager.instance.ReleaseVehicle(vehicleID);
}
```
- Khi một phương tiện đứng yên (`velocity.sqrMagnitude < 0.01f`), `m_blockCounter` tăng dần từ 1 đến 255.
- Do chu kỳ mô phỏng xe chạy theo từng lát cắt (16 ticks) kết hợp với các frame skip hoặc hook của TM:PE, giá trị `m_blockCounter` có thể nhảy vọt qua mốc 100/150 (ví dụ từ 99 nhảy lên 101, hoặc từ 149 lên 151).
- Khi đã vượt qua mốc này (`m_blockCounter >= 151`), điều kiện `m_blockCounter == 100/150` **vĩnh viễn không bao giờ thỏa mãn nữa**!
- Engine coi xe này như bị lọt lưới, biến nó thành xe "ma bất tử" đứng chôn chân vĩnh viễn ở `m_blockCounter = 255`.

### B. Xung đột Nhập làn Mặc định (Mutual Yield Deadlock khi nâng cấp 3 lên 4 làn)
- Khi nâng cấp Ring Road 2 từ 3 làn lên 4 làn, chỉ số làn (lane index) thay đổi: làn ngoài cùng bên phải (Lane 3) tiếp giáp trực tiếp với nhánh on-ramp 1 làn Cầu Dốc Ánh.
- Do không can thiệp TM:PE ("mặc định là được vượt, không có nhường đường"), thuật toán va chạm mặc định `CarAI.CheckOtherVehicles` và `MayChangeSegment` tính toán hộp va chạm (bounding box) chồng lấn giữa xe trên Ring Road 2 và xe đang từ Cầu Dốc Ánh lao lên.
- Vì không có biển ưu tiên, cả 2 xe đều phanh lại nhường đối phương (`maxSpeed = 0`). Cả 2 đều không chịu tiến vào nút giao -> bế tắc song phương (Mutual Block).

### C. Lỗ hổng trong bộ lọc `StuckVehicleRecovery` trước đây
Ở phiên bản trước, `StuckVehicleRecovery` tại dòng 13483 có điều kiện:![alt text](https://127.0.0.1:49567/static/artifacts/dd70b9c0-c10d-4ad5-b74e-3db8f9ba3dd9/.user_uploaded/media_1792279309729.jpg?csrf%3D84fbc559-161e-4933-aad2-0b1818ae9601)
```csharp
if (buffer[i].m_blockCounter >= 255 && buffer[i].m_path == 0)
```
Do các phương tiện trên cao tốc đều đang chạy lộ trình hợp lệ (`m_path != 0`), điều kiện `buffer[i].m_path == 0` trả về `false`, khiến bộ dọn xe kẹt của mod hoàn toàn bỏ qua chúng.

### D. Các hàm Postfix chống kẹt cao tốc chưa từng được đăng ký Harmony
Mod đã chuẩn bị sẵn `VehicleBehaviorManager_MayChangeSegment` và `CarAI_CalculateSegmentPosition` để khôi phục tốc độ `maxSpeed >= 2.0f` trên cao tốc, nhưng chúng bị bỏ quên chưa được đăng ký trong `TMPEPatches.Apply`.

---

## 3. Các Biện pháp Đã Khắc phục Triệt để trong v4.3.6

1. **Cơ chế Phục hồi Xe Kẹt Mọi Trạng thái (`m_blockCounter >= 180`):**
   - Đổi điều kiện thu hồi xe: Bất kỳ phương tiện đang lưu thông nào (Spawned, Created, không nằm trong các cờ dừng hợp lệ như đỗ xe/đang đón khách) bị kẹt `m_blockCounter >= 180` sẽ được giải phóng lập tức:
     - Tự động hủy đường đi tồn đọng (`PathManager.ReleasePath`).
     - Gọi `VehicleManager.ReleaseVehicle`.
   - Ngay khi xe đầu đàn bị kẹt được giải phóng, toàn bộ đoàn xe phía sau sẽ lập tức nhận diện đường thoáng và lăn bánh bình thường.

2. **Hook `TM:PE VehicleBehaviorManager.MayChangeSegment` & `MayDespawn`:**
   - Đăng ký cả 2 overload (14 tham số và 12 tham số) của `MayChangeSegment`: Đối với các đoạn cao tốc (`isHighway`), ép `__result = true` và `maxSpeed >= 2.0f`, ngăn chặn triệt để tình trạng 2 xe nhường nhau dẫn đến tốc độ = 0.
   - Đăng ký `MayDespawn` postfix đảm bảo TM:PE không chặn despawn khi xe đã vượt ngưỡng chịu đựng.

3. **Hook `CarAI.CalculateSegmentPosition` (13 & 8 tham số):**
   - Đăng ký kèm `RestoreVehicleSpeedIfNeeded`, tự động khôi phục tốc độ tối thiểu cho phương tiện trên cao tốc nếu bị tụt về 0 ngoài ý muốn mà không phải do đèn đỏ.

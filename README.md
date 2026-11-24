# HƯỚNG DẪN MỞ VÀ CHẠY PROJECT: CHRONO MUSEUM VR

Dự án **Chrono Museum - Bảo Tàng Thời Không VR** đã được thiết lập và cấu hình hoàn chỉnh cho Unity 6 (6000.3.26f1) với **XR Interaction Toolkit 3.5**, tương thích hoàn toàn kính thực tế ảo **Meta Quest 2 / 3 / Pro** và **PCVR / XR Device Simulator**.

---

## 📍 1. VỊ TRÍ PROJECT TRÊN MÁY
Project đã được đăng ký sẵn trong **Unity Hub**:
* **Thư mục project độc lập:** `D:\unity\VR_ChronoMuseum`
* *(Dự phòng: Toàn bộ code cũng được tích hợp sẵn tại `D:\unity\Thuchanh\Assets\ChronoMuseum`)*

---

## 🚀 2. CÁCH MỞ PROJECT BẰNG UNITY HUB
1. Mở **Unity Hub** trên máy tính của bạn.
2. Tại danh sách **Projects**, bạn sẽ thấy dự án **VR_ChronoMuseum** đã xuất hiện sẵn.
3. Nhấp chuột vào **VR_ChronoMuseum** để Unity Editor khởi chạy và nạp các asset.
4. *(Nếu vì lý do gì chưa thấy trong danh sách: bấm nút **Add** -> **Add project from disk** -> chọn thư mục `D:\unity\VR_ChronoMuseum`).*

---

## 🏛️ 3. MỞ SCENE VÀ TEST TRẢI NGHIỆM
1. Trong cửa sổ **Project** của Unity, vào thư mục:
   `Assets / ChronoMuseum / Scenes /`
2. Nhấp đúp chuột để mở scene: **`ChronoMuseum_MainScene`**.
3. **Chạy thử ngay trong Editor:**
   * Bấm nút **Play (▶)** ở thanh công cụ phía trên Unity.
   * **Nếu không có kính VR gắn vào:** Hệ thống **XR Device Simulator** tích hợp sẵn của Unity sẽ tự động kích hoạt:
     * Dùng chuột phải + phím `W/A/S/D` để di chuyển góc nhìn.
     * Dùng phím `G` để chuyển chế độ tay cầm ảo (nắm / nhặt cổ vật).
     * Bấm chuột trái để tương tác tia ngắm (Raycast / Teleport).
   * **Nếu có kính VR (Meta Quest):**
     * Kết nối qua dây cáp **Quest Link (Oculus Link)** hoặc **AirLink / Virtual Desktop**.
     * Bấm **Play**, đeo kính vào và bắt đầu khám phá!

---

## 🎮 4. CÁC TÍNH NĂNG ĐỘT PHÁ ĐÃ ĐƯỢC LẬP TRÌNH SẴN
1. **MuseumCuratorManager.cs:** Quản lý tiến trình phục chế cả 4 đại sảnh (Ai Cập, Kỷ Jura, Đông Sơn, Phục Hưng), phát âm thanh chiến thắng, thông báo UI và kích hoạt hiệu ứng pháo hoa khi hoàn thành.
2. **ArtifactPiece.cs & XRGrabInteractable:** Các mảnh vỡ cổ vật trôi nổi, phát sáng và xoay nhẹ trong không gian; người chơi dùng tay cầm VR cầm nắm 360 độ cực kỳ chân thực.
3. **ArtifactRestorationPedestal.cs:** Bệ phục chế thông minh tự động hút và khớp mảnh vỡ (Snap zone). Khi ghép đủ mảnh, bệ biến hình thành bảo vật nguyên vẹn rực sáng ánh hoàng kim.
4. **LivingExhibitController.cs (Điểm sáng tạo nhất):** Kích hoạt "Triển lãm Sống" - khi phục hồi xong, toàn bộ không gian phòng biến đổi thành hiện trường lịch sử sống động kèm âm thanh vòm 3D (tiếng khủng long gầm, tiếng tụng kinh Ai Cập cổ, tiếng trống đồng vang dội).
5. **CuratorScanner.cs:** Thiết bị quét cổ ngữ tia UV cầm tay, chiếu tia sáng laser vào các góc khuất để giải mã bí mật lịch sử.
6. **WristTabletUI.cs & ExhibitInfoPlacard.cs:** Máy tính bảng gắn trên cổ tay hiển thị checklist nhiệm vụ và bảng chú giải thông tin bách khoa 3D.
7. **VRMuseumSceneBuilder.cs:** Trình tự động khởi tạo và căn chỉnh toàn bộ 4 đại sảnh nếu bạn muốn tạo scene mới với 1 cú click (`Right Click -> Build VR Museum Environment`).

---

## 🏆 5. GỢI Ý BÁO CÁO / THUYẾT TRÌNH VỚI GIẢNG VIÊN
* Nhấn mạnh: *"Project giải quyết triệt để khuyết điểm lớn nhất của bảo tàng truyền thống là **'Cấm sờ vào hiện vật'**, cho phép người dùng **tự tay cầm nắm, phục chế và bước qua Cổng Thời Không để sống trong lịch sử** bằng công nghệ VR."*

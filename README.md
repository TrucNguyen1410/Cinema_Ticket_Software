# 🎬 Phần Mềm Quản Lý Bán Vé Rạp Chiếu Phim (Cinema Ticket Software)

Đây là dự án ứng dụng Desktop quản lý rạp chiếu phim và bán vé nội bộ, được xây dựng bằng **C# Windows Forms**. Phần mềm cung cấp giải pháp toàn diện từ khâu quản lý phim, lên lịch chiếu cho đến việc bán vé trực tiếp tại quầy.

## ✨ Tính Năng Nổi Bật

Hệ thống được chia làm 2 phân quyền với các chức năng riêng biệt:

### 👨‍💼 Dành cho Quản Lý (Admin)
* **Thống Kê & Báo Cáo:** Xem doanh thu, số lượng vé bán/hoàn theo Ngày, Tuần, Tháng, Năm hoặc theo Phim qua biểu đồ trực quan. Hỗ trợ xuất báo cáo ra file Excel (CSV).
* **Quản Lý Phim:** Thêm, sửa, xóa, cập nhật thông tin phim.
* **Quản Lý Suất Chiếu:** Lên lịch chiếu trực quan, tự động kiểm tra và cảnh báo nếu xếp trùng lịch tại cùng một rạp.
* **Quản Lý Giao Dịch:** Hỗ trợ tính năng **Đổi vé** (tự động tính tiền chênh lệch) và **Hoàn vé** cho khách hàng.
* **Quản Lý Nhân Sự:** Tạo, cấp quyền và quản lý tài khoản đăng nhập của nhân viên bán vé.

### 👩‍💻 Dành cho Nhân Viên Bán Vé (Employee)
* **Giao diện bán vé trực quan:** Chọn ngày chiếu linh hoạt, hiển thị danh sách các suất chiếu trong ngày.
* **Sơ đồ ghế ngồi động:** Hiển thị trực quan sơ đồ phòng chiếu, phân biệt rõ ràng **Ghế Thường**, **Ghế VIP (+10k)** và **Ghế Đã Đặt**.
* **Thanh toán linh hoạt:** Hỗ trợ thanh toán bằng Tiền mặt hoặc Chuyển khoản (ví điện tử Momo, ZaloPay, Vietcombank...).

## 🛠 Công Nghệ Sử Dụng
* **Ngôn ngữ lập trình:** C#
* **Framework:** .NET Windows Forms
* **Cơ sở dữ liệu:** Microsoft SQL Server
* **Thư viện tích hợp:** System.Windows.Forms.DataVisualization.Charting (Biểu đồ)

## 📸 Giao Diện Gợi Ý
*(Bạn có thể chụp ảnh màn hình phần mềm của mình và kéo thả vào đây để Github tự tạo link ảnh nhé!)*

## 🚀 Hướng Dẫn Cài Đặt & Chạy Dự Án

Làm theo các bước sau để chạy phần mềm trên máy tính của bạn:

**Bước 1: Tải mã nguồn về máy**
```bash
git clone [https://github.com/TrucNguyen1410/Cinema_Ticket_Software.git](https://github.com/TrucNguyen1410/Cinema_Ticket_Software.git)

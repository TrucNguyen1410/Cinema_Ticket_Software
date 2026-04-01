using System;
using System.Windows.Forms;

namespace CinemaTicket
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // 1. Khởi tạo Form Đăng nhập 1 (Dự kiến dành cho Quản lý)
            Login login1 = new Login();
            login1.Text = "Đăng nhập - Quản lý";
            login1.StartPosition = FormStartPosition.CenterScreen;

            // 2. Khởi tạo Form Đăng nhập 2 (Dự kiến dành cho Nhân viên)
            Login login2 = new Login();
            login2.Text = "Đăng nhập - Bán vé";
            login2.StartPosition = FormStartPosition.Manual;
            // Dời form thứ 2 sang một góc (hoặc cách form 1 một đoạn) để không bị đè lên nhau
            login2.Location = new System.Drawing.Point(100, 100);

            // 3. Hiển thị Form Đăng nhập 2 (Show không chặn)
            login2.Show();

            // 4. Chạy vòng lặp chính trên Form Đăng nhập 1
            // Lưu ý: Nếu tắt hẳn (nhấn dấu X màu đỏ) Form Đăng nhập 1, toàn bộ chương trình (cả Form 2) sẽ tắt hết
            
            Application.Run(login1);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CinemaTicket
{
    public partial class FormSeatDesigner : Form
    {
        private int _roomId;
        private HashSet<string> selectedSeats = new HashSet<string>();

        // Cấu hình kích thước ghế
        private int seatSize = 40;
        private int gap = 8;
        private int aisleGap = 30; // Khoảng cách lối đi ở giữa

        public FormSeatDesigner(int roomId)
        {
            _roomId = roomId;
            InitializeComponent();
        }

        private void FormSeatDesigner_Load(object sender, EventArgs e)
        {
            // Thiết lập giao diện
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.WhiteSmoke;
            lblTitle.Text = $"SƠ ĐỒ GHẾ - RẠP {_roomId}";

            DrawSeats();
            DrawLegend();
        }

        // ================= LẤY CẤU HÌNH RẠP TỪ DB (MÔ PHỎNG) =================
        // Hàm này trả về: (Số dòng, Số cột, Tổng ghế thực tế của rạp)
        private (int rows, int cols, int totalSeats) GetRoomConfig(int roomId)
        {
            switch (roomId)
            {
                case 1:
                case 2:
                    return (10, 10, 100); // 10x10 = 100
                case 3:
                    return (10, 14, 140); // 10x14 = 140
                case 4:
                    return (10, 16, 155); // 10x16 = 160 (Sẽ ẩn 5 ghế cuối cho đúng 155)
                case 5:
                    return (14, 18, 250); // 14x18 = 252 (Sẽ ẩn 2 ghế cuối)
                case 6: // IMAX
                case 7: // 4DX
                    return (18, 25, 450); // 18x25 = 450
                default:
                    return (10, 10, 100); // Mặc định
            }
        }

        // ================= VẼ GHẾ =================
        private void DrawSeats()
        {
            pnlSeats.Controls.Clear();

            // 1. Lấy thông tin rạp
            var config = GetRoomConfig(_roomId);
            int rows = config.rows;
            int cols = config.cols;
            int maxSeats = config.totalSeats;

            // 2. Tính toán vị trí để căn giữa màn hình
            // Tổng chiều rộng = (số ghế * kích thước) + (khoảng cách khe) + (lối đi giữa)
            int totalWidth = (cols * seatSize) + ((cols - 1) * gap) + aisleGap;
            int startX = (pnlSeats.Width - totalWidth) / 2;
            // Nếu rạp to quá thì startX có thể âm, set về 20 để không bị mất góc
            if (startX < 20) startX = 20;
            int startY = 50;

            int seatCounter = 0; // Biến đếm để kiểm soát tổng số ghế

            // 3. Vòng lặp vẽ ghế
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    // Kiểm soát số lượng ghế chính xác theo DB (Ví dụ Rạp 4 có 155 ghế thì đến ghế 156 sẽ dừng)
                    seatCounter++;
                    if (seatCounter > maxSeats) break;

                    // Tạo tên ghế: Hàng (A,B,C) + Cột (1,2,3)
                    // (char)('A' + r) sẽ tự tăng A, B, C theo vòng lặp r
                    // (c + 1) sẽ tự tăng 1, 2, 3 theo vòng lặp c -> Đảm bảo không trùng và reset mỗi hàng
                    string seatCode = $"{(char)('A' + r)}{c + 1}";

                    Button btn = new Button
                    {
                        Width = seatSize,
                        Height = seatSize,
                        Text = seatCode,
                        Font = new Font("Segoe UI", 7, FontStyle.Regular),
                        BackColor = Color.LightGray,
                        FlatStyle = FlatStyle.Flat,
                        Tag = seatCode,
                        Cursor = Cursors.Hand
                    };
                    btn.FlatAppearance.BorderSize = 0;

                    // TÍNH TOÁN VỊ TRÍ (X, Y)
                    int xPos = startX + (c * (seatSize + gap));
                    int yPos = startY + (r * (seatSize + gap));

                    // Tạo lối đi ở giữa (Nếu cột hiện tại lớn hơn hoặc bằng một nửa số cột)
                    if (c >= cols / 2)
                    {
                        xPos += aisleGap;
                    }

                    btn.Left = xPos;
                    btn.Top = yPos;

                    // SET MÀU GHẾ VIP
                    // Quy ước: 3 hàng cuối cùng là VIP (Rạp càng to thì hàng VIP càng lùi về sau)
                    if (r >= rows - 3)
                    {
                        btn.BackColor = Color.OrangeRed; // Màu ghế VIP
                        btn.ForeColor = Color.White;
                        btn.Tag = "VIP|" + seatCode; // Lưu dấu hiệu VIP vào tag
                    }
                    else
                    {
                        btn.Tag = "NORMAL|" + seatCode;
                    }

                    btn.Click += Seat_Click;
                    pnlSeats.Controls.Add(btn);
                }
            }
        }

        // ================= XỬ LÝ CLICK GHẾ =================
        private void Seat_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            // Tách Tag để lấy loại ghế và mã ghế. VD: "VIP|H10"
            string[] parts = btn.Tag.ToString().Split('|');
            string type = parts[0];
            string seatCode = parts[1];

            // Nếu ghế đã đặt (Màu xám đậm) -> Không làm gì
            if (btn.BackColor == Color.DimGray) return;

            if (selectedSeats.Contains(seatCode))
            {
                // BỎ CHỌN: Trả về màu cũ
                selectedSeats.Remove(seatCode);
                if (type == "VIP")
                    btn.BackColor = Color.OrangeRed;
                else
                    btn.BackColor = Color.LightGray;
            }
            else
            {
                // CHỌN: Đổi màu xanh
                selectedSeats.Add(seatCode);
                btn.BackColor = Color.DodgerBlue;
            }

            // Cập nhật Label hiển thị
            lblSelected.Text = selectedSeats.Count > 0
                ? $"Đang chọn ({selectedSeats.Count} ghế): {string.Join(", ", selectedSeats)}"
                : "Chưa chọn ghế nào";
        }

        // ================= VẼ CHÚ THÍCH (LEGEND) =================
        private void DrawLegend()
        {
            pnlLegend.Controls.Clear();
            // Căn giữa chú thích
            int centerX = (pnlLegend.Width - 500) / 2;
            if (centerX < 10) centerX = 10;

            AddLegendItem(centerX, Color.LightGray, "Ghế thường");
            AddLegendItem(centerX + 120, Color.OrangeRed, "Ghế VIP");
            AddLegendItem(centerX + 240, Color.DodgerBlue, "Đang chọn");
            AddLegendItem(centerX + 360, Color.DimGray, "Đã đặt");
        }

        private void AddLegendItem(int x, Color color, string text)
        {
            Panel p = new Panel
            {
                Width = 20,
                Height = 20,
                BackColor = color,
                Left = x,
                Top = 20,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lbl = new Label
            {
                Text = text,
                Left = x + 25,
                Top = 22,
                AutoSize = true,
                Font = new Font("Segoe UI", 9)
            };

            pnlLegend.Controls.Add(p);
            pnlLegend.Controls.Add(lbl);
        }

        // Sự kiện khi thay đổi kích thước Form để vẽ lại cho cân đối
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            // Chỉ vẽ lại nếu panel đã được khởi tạo
            if (pnlSeats != null && pnlSeats.Controls.Count > 0)
            {
                DrawSeats();
                DrawLegend();
            }
        }
    }
}
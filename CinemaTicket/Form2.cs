using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace CinemaTicket
{
    public partial class Form2 : Form
    {
        private int _currentShowID = -1;
        private int _currentRoomID = -1;
        private decimal _baseTicketPrice = 0;
        private DateTime _currentDate = DateTime.Today; // Biến lưu ngày đang xem

        // Lưu danh sách ghế đang chọn và giá tương ứng (Thường/VIP)
        private Dictionary<string, decimal> _selectedSeats = new Dictionary<string, decimal>();
        private HashSet<string> _bookedSeats = new HashSet<string>();

        // Các control tạo động
        private Panel pnlMapContainer;
        private Panel pnlLegend;
        private Button btnPay;

        // Control chọn ngày (Nút bấm + Lịch nổi)
        private Button btnSelectDate;
        private MonthCalendar calShowDate;

        // Cấu hình hiển thị ghế
        private int seatSize = 45;
        private int gap = 6;
        private int aisleGap = 40;

        public Form2()
        {
            InitializeComponent();
        }

        private async void Form2_Load(object sender, EventArgs e)
        {
            // Hiển thị ngày tháng hiện tại trên Header
            if (lblDate != null)
            {
                lblDate.Text = DateTime.Now.ToString("dddd, dd/MM/yyyy");
            }

            // TỰ ĐỘNG HIỂN THỊ TÊN NHÂN VIÊN TỪ SESSION
            if (lblStaff != null)
            {
                if (!string.IsNullOrEmpty(Session.FullName))
                {
                    lblStaff.Text = "Nhân viên: " + Session.FullName;
                }
                else
                {
                    lblStaff.Text = "Nhân viên: Bán vé 01 (Khách)";
                }
            }

            // Khởi tạo các control động (Sơ đồ ghế, Nút thanh toán, Nút chọn ngày)
            SetupDynamicControls();

            // Mặc định load phim của ngày hôm nay
            _currentDate = DateTime.Today;
            await LoadDataSafeAsync(_currentDate);
        }

        // ================= SETUP GIAO DIỆN DYNAMIC =================
        private void SetupDynamicControls()
        {
            // 1. THIẾT LẬP NÚT CHỌN NGÀY NẰM CHÍNH XÁC DƯỚI CÙNG GÓC TRÁI
            if (flpShowtimes != null && flpShowtimes.Parent != null)
            {
                Control originalParent = flpShowtimes.Parent;

                // Tạo một Panel bọc lại để đảm bảo nút luôn nằm chính xác dưới danh sách phim
                Panel pnlWrapper = new Panel();
                pnlWrapper.Location = flpShowtimes.Location;
                pnlWrapper.Size = flpShowtimes.Size;
                pnlWrapper.Dock = flpShowtimes.Dock;
                pnlWrapper.Anchor = flpShowtimes.Anchor;

                // Đổi chỗ flpShowtimes bằng pnlWrapper trên Form
                int index = originalParent.Controls.IndexOf(flpShowtimes);
                originalParent.Controls.Remove(flpShowtimes);

                originalParent.Controls.Add(pnlWrapper);
                originalParent.Controls.SetChildIndex(pnlWrapper, index);

                // Tạo Panel chứa nút bấm nằm ở đáy
                Panel pnlBottomAction = new Panel();
                pnlBottomAction.Dock = DockStyle.Bottom;
                pnlBottomAction.Height = 55;
                pnlBottomAction.BackColor = Color.White;
                pnlBottomAction.Padding = new Padding(5);

                btnSelectDate = new Button();
                btnSelectDate.Text = "📅 Chọn ngày xem suất chiếu";
                btnSelectDate.Dock = DockStyle.Fill;
                btnSelectDate.BackColor = Color.DodgerBlue;
                btnSelectDate.ForeColor = Color.White;
                btnSelectDate.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                btnSelectDate.Cursor = Cursors.Hand;
                btnSelectDate.FlatStyle = FlatStyle.Flat;
                btnSelectDate.FlatAppearance.BorderSize = 0;

                pnlBottomAction.Controls.Add(btnSelectDate);

                // Nhét nút bấm và danh sách phim vào Wrapper
                pnlWrapper.Controls.Add(pnlBottomAction);
                pnlWrapper.Controls.Add(flpShowtimes);

                flpShowtimes.Dock = DockStyle.Fill; // Danh sách phim lấp đầy phần trên
                pnlBottomAction.BringToFront(); // Đẩy nút bấm xuống sát đáy

                // TẠO BỘ LỊCH NỔI (Popup Calendar)
                calShowDate = new MonthCalendar();
                calShowDate.Visible = false;
                calShowDate.MaxSelectionCount = 1;

                // Add lịch vào Form chính để nó nổi đè lên mọi thứ, không bị cắt xén
                this.Controls.Add(calShowDate);

                // Sự kiện khi bấm vào nút Xanh
                btnSelectDate.Click += (s, e) =>
                {
                    if (calShowDate.Visible)
                    {
                        calShowDate.Visible = false;
                    }
                    else
                    {
                        // Tính toán vị trí để cuốn lịch nổi lên ngay phía trên nút bấm xanh
                        Point screenPt = pnlBottomAction.PointToScreen(Point.Empty);
                        Point formPt = this.PointToClient(screenPt);

                        calShowDate.Location = new Point(
                            formPt.X + (pnlBottomAction.Width - calShowDate.Width) / 2,
                            formPt.Y - calShowDate.Height);

                        calShowDate.BringToFront();
                        calShowDate.Visible = true;
                        calShowDate.Focus();
                    }
                };

                // Sự kiện khi người dùng click chọn 1 ngày trên lịch
                calShowDate.DateSelected += async (s, e) =>
                {
                    calShowDate.Visible = false; // Ẩn lịch đi
                    _currentDate = calShowDate.SelectionStart; // Cập nhật biến ngày đang xem
                    btnSelectDate.Text = "📅 Ngày đang xem: " + _currentDate.ToString("dd/MM/yyyy");
                    await LoadDataSafeAsync(_currentDate); // Tải lại danh sách phim
                };

                // Tự động ẩn lịch nếu click ra chỗ khác
                calShowDate.Leave += (s, e) => { calShowDate.Visible = false; };
            }

            // 2. THIẾT LẬP SƠ ĐỒ GHẾ VÀ CHÚ THÍCH Ở GIỮA
            Control center = this.Controls.Find("pnlCenter", true).FirstOrDefault();
            if (center != null)
            {
                pnlLegend = new Panel();
                pnlLegend.Height = 50;
                pnlLegend.Dock = DockStyle.Bottom;
                pnlLegend.BackColor = Color.White;
                pnlLegend.BorderStyle = BorderStyle.FixedSingle;
                center.Controls.Add(pnlLegend);

                pnlMapContainer = new Panel();
                pnlMapContainer.Dock = DockStyle.Fill;
                pnlMapContainer.AutoScroll = true;
                pnlMapContainer.BackColor = Color.WhiteSmoke;
                center.Controls.Add(pnlMapContainer);

                pnlMapContainer.BringToFront();
                DrawLegend();
            }

            // 3. THIẾT LẬP NÚT THANH TOÁN BÊN PHẢI
            Control right = this.Controls.Find("pnlRight", true).FirstOrDefault();
            if (right != null)
            {
                btnPay = new Button();
                btnPay.Text = "THANH TOÁN";
                btnPay.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                btnPay.BackColor = Color.Red;
                btnPay.ForeColor = Color.White;
                btnPay.Height = 60;
                btnPay.Dock = DockStyle.Bottom;
                btnPay.Cursor = Cursors.Hand;
                btnPay.FlatStyle = FlatStyle.Flat;
                btnPay.FlatAppearance.BorderSize = 0;
                btnPay.Click += BtnPay_Click;
                right.Controls.Add(btnPay);
                btnPay.BringToFront();
            }
        }

        // ================= XỬ LÝ THANH TOÁN & CHỌN PHƯƠNG THỨC =================
        private void BtnPay_Click(object sender, EventArgs e)
        {
            if (_selectedSeats.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ghế trước khi thanh toán!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal totalAmount = _selectedSeats.Values.Sum();

            // Tạo form popup chọn phương thức thanh toán
            Form frmPay = new Form();
            frmPay.Text = "Xác nhận thanh toán";
            frmPay.Size = new Size(400, 320);
            frmPay.StartPosition = FormStartPosition.CenterParent;
            frmPay.FormBorderStyle = FormBorderStyle.FixedDialog;
            frmPay.MaximizeBox = false;

            Label lblTien = new Label { Text = $"TỔNG TIỀN: {totalAmount:N0} VNĐ", Location = new Point(20, 20), AutoSize = true, Font = new Font("Arial", 14, FontStyle.Bold), ForeColor = Color.Red };
            Label lblMethod = new Label { Text = "Phương thức:", Location = new Point(20, 70), AutoSize = true, Font = new Font("Arial", 10) };

            ComboBox cbMethod = new ComboBox { Location = new Point(130, 68), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Arial", 10) };
            cbMethod.Items.AddRange(new string[] { "Tiền mặt", "Chuyển khoản / Ví ĐT" });
            cbMethod.SelectedIndex = 0;

            Label lblBank = new Label { Text = "Ngân hàng/Ví:", Location = new Point(20, 110), AutoSize = true, Font = new Font("Arial", 10), Visible = false };
            ComboBox cbBank = new ComboBox { Location = new Point(130, 108), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Arial", 10), Visible = false };
            cbBank.Items.AddRange(new string[] { "MoMo", "ZaloPay", "VNPay", "Vietcombank", "MBBank", "Techcombank", "Agribank" });
            cbBank.SelectedIndex = 0;

            // Xử lý bật/tắt ô chọn Ngân hàng tùy theo phương thức
            cbMethod.SelectedIndexChanged += (s, ev) =>
            {
                bool isOnline = (cbMethod.Text == "Chuyển khoản / Ví ĐT");
                lblBank.Visible = isOnline;
                cbBank.Visible = isOnline;
            };

            Button btnConfirm = new Button { Text = "HOÀN TẤT THANH TOÁN", Location = new Point(90, 190), Size = new Size(200, 50), BackColor = Color.Red, ForeColor = Color.White, Font = new Font("Arial", 10, FontStyle.Bold), Cursor = Cursors.Hand };

            btnConfirm.Click += async (s, ev) =>
            {
                string payMethod = cbMethod.Text;
                if (cbMethod.Text == "Chuyển khoản / Ví ĐT")
                {
                    payMethod += $" ({cbBank.Text})";
                }

                try
                {
                    using (SqlConnection con = DB.GetConnection())
                    {
                        await con.OpenAsync();
                        foreach (var item in _selectedSeats)
                        {
                            string sql = @"INSERT INTO Bookings (ShowID, RoomID, SeatNumber, Price, BookingTime, Status, AccountID) 
                                           VALUES (@sid, @rid, @seat, @price, GETDATE(), 'PAID', @acc)";
                            using (SqlCommand cmd = new SqlCommand(sql, con))
                            {
                                cmd.Parameters.AddWithValue("@sid", _currentShowID);
                                cmd.Parameters.AddWithValue("@rid", _currentRoomID);
                                cmd.Parameters.AddWithValue("@seat", item.Key);
                                cmd.Parameters.AddWithValue("@price", item.Value);
                                cmd.Parameters.AddWithValue("@acc", Session.AccountID > 0 ? (object)Session.AccountID : DBNull.Value);
                                await cmd.ExecuteNonQueryAsync();
                            }
                        }
                    }
                    MessageBox.Show($"Thanh toán thành công {totalAmount:N0}đ!\nPhương thức: {payMethod}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frmPay.Close();

                    // Gọi tải lại dữ liệu sau khi thanh toán xong
                    await ReloadData(_currentDate);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi thanh toán: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            frmPay.Controls.AddRange(new Control[] { lblTien, lblMethod, cbMethod, lblBank, cbBank, btnConfirm });
            frmPay.ShowDialog();
        }

        private async void btnReload_Click(object sender, EventArgs e)
        {
            await ReloadData(_currentDate);
        }

        private async Task ReloadData(DateTime filterDate)
        {
            if (btnReload != null) { btnReload.Text = "Đang tải..."; btnReload.Enabled = false; }
            _currentShowID = -1;
            _currentRoomID = -1;
            _selectedSeats.Clear();
            _bookedSeats.Clear();
            flpShowtimes.Controls.Clear();
            if (pnlMapContainer != null) pnlMapContainer.Controls.Clear();
            lstOrder.Items.Clear();
            lblTotal.Text = "0 đ";
            lblSelectedShow.Text = "Vui lòng chọn suất chiếu";

            await LoadDataSafeAsync(filterDate);

            if (btnReload != null) { btnReload.Text = "🔄 Tải lại"; btnReload.Enabled = true; }
        }

        // ================= LOAD DỮ LIỆU SUẤT CHIẾU =================
        private async Task LoadDataSafeAsync(DateTime filterDate)
        {
            try
            {
                await LoadShowtimesByDateAsync(filterDate);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        private async Task LoadShowtimesByDateAsync(DateTime date)
        {
            flpShowtimes.Controls.Clear();
            string sql = @"SELECT s.ShowID, s.RoomID, m.Title, r.RoomName, s.StartTime, 
                           DATEADD(MINUTE, s.Duration, s.StartTime) EndTime, s.TicketPrice
                           FROM ShowTimes s 
                           JOIN Movies m ON s.MovieID = m.MovieID 
                           JOIN Rooms r ON s.RoomID = r.RoomID
                           WHERE CAST(s.StartTime AS DATE) = CAST(@d AS DATE) 
                           ORDER BY s.StartTime";

            using (SqlConnection con = DB.GetConnection())
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@d", date);
                    using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                    {
                        while (await rd.ReadAsync())
                        {
                            flpShowtimes.Controls.Add(CreateShowtimeCard(rd));
                        }
                    }
                }
            }

            if (flpShowtimes.Controls.Count == 0)
            {
                Label lblEmpty = new Label { Text = "Không có suất chiếu nào trong ngày này.", AutoSize = true, ForeColor = Color.Gray, Margin = new Padding(10) };
                flpShowtimes.Controls.Add(lblEmpty);
            }
        }

        private Panel CreateShowtimeCard(SqlDataReader rd)
        {
            int showId = (int)rd["ShowID"];
            int roomId = (int)rd["RoomID"];
            decimal price = Convert.ToDecimal(rd["TicketPrice"]);

            Panel p = new Panel();
            p.Width = flpShowtimes.Width - 25;
            p.Height = 110;
            p.BorderStyle = BorderStyle.FixedSingle;
            p.Margin = new Padding(5);
            p.Cursor = Cursors.Hand;
            p.Tag = new Tuple<int, int, decimal>(showId, roomId, price);

            p.Controls.Add(new Label { Text = rd["Title"].ToString(), Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(5, 5), AutoSize = true });
            p.Controls.Add(new Label { Text = $"Rạp: {rd["RoomName"]}", Location = new Point(5, 30), AutoSize = true });
            p.Controls.Add(new Label { Text = $"Giờ: {((DateTime)rd["StartTime"]):HH:mm} - {((DateTime)rd["EndTime"]):HH:mm}", Location = new Point(5, 50), AutoSize = true });
            p.Controls.Add(new Label { Text = $"Giá: {price:N0} đ", Location = new Point(5, 70), AutoSize = true, ForeColor = Color.Red });

            p.Click += Showtime_Click;
            foreach (Control c in p.Controls)
            {
                c.Click += Showtime_Click;
            }
            return p;
        }

        private async void Showtime_Click(object sender, EventArgs e)
        {
            Panel card = sender as Panel ?? ((Control)sender).Parent as Panel;
            if (card == null) return;

            foreach (Control c in flpShowtimes.Controls)
            {
                if (c is Panel) c.BackColor = Color.White;
            }
            card.BackColor = Color.LightBlue;

            var tag = (Tuple<int, int, decimal>)card.Tag;
            _currentShowID = tag.Item1;
            _currentRoomID = tag.Item2;
            _baseTicketPrice = tag.Item3;

            lblSelectedShow.Text = $"Suất chiếu: {card.Controls[0].Text} - Rạp {_currentRoomID}";
            _selectedSeats.Clear();
            lstOrder.Items.Clear();
            lblTotal.Text = "0 đ";

            await LoadBookedSeatsAsync();
            DrawSeatMap(_currentRoomID);
        }

        private async Task LoadBookedSeatsAsync()
        {
            _bookedSeats.Clear();
            // Tải tất cả ghế đã được bán hoặc khách đổi sang
            string sql = "SELECT SeatNumber FROM Bookings WHERE ShowID=@id AND Status IN ('PAID', 'EXCHANGED')";
            using (SqlConnection con = DB.GetConnection())
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", _currentShowID);
                    using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                    {
                        while (await rd.ReadAsync())
                        {
                            _bookedSeats.Add(rd["SeatNumber"].ToString());
                        }
                    }
                }
            }
        }

        // ================= VẼ SƠ ĐỒ GHẾ VÀ MÀN HÌNH =================
        private void DrawSeatMap(int roomId)
        {
            if (pnlMapContainer == null) return;
            pnlMapContainer.Controls.Clear();

            // Cấu hình mẫu cho các rạp
            int rows = 10, cols = 10;
            if (roomId == 3) { rows = 10; cols = 14; }
            else if (roomId >= 4) { rows = 10; cols = 16; }

            int totalWidth = (cols * seatSize) + ((cols - 1) * gap) + aisleGap;
            int startX = Math.Max(20, (pnlMapContainer.Width - totalWidth) / 2);

            // --- VẼ MÀN HÌNH (Khôi phục lại đầy đủ như bản gốc) ---
            Panel pnlScreen = new Panel();
            pnlScreen.Size = new Size(totalWidth + 100, 50);
            pnlScreen.Location = new Point(startX - 50, 10);
            pnlScreen.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                int w = pnlScreen.Width; int h = pnlScreen.Height;
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddBezier(0, h, w / 4, 0, 3 * w / 4, 0, w, h);
                    path.AddLine(w, h, 0, h);
                    using (SolidBrush brush = new SolidBrush(Color.FromArgb(50, Color.DodgerBlue))) g.FillPath(brush, path);
                    using (Pen pen = new Pen(Color.DodgerBlue, 4)) g.DrawBezier(pen, 0, h, w / 4, 0, 3 * w / 4, 0, w, h);
                }
                TextRenderer.DrawText(g, "Màn hình", new Font("Segoe UI", 10, FontStyle.Bold), new Rectangle(0, 20, w, h), Color.Navy, TextFormatFlags.HorizontalCenter);
            };
            pnlMapContainer.Controls.Add(pnlScreen);

            // --- VẼ GHẾ ---
            int startY = 80; // Dịch tọa độ Y xuống dưới cái màn hình
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    string seatCode = $"{(char)('A' + r)}{c + 1}";
                    Button btn = new Button();
                    btn.Width = seatSize;
                    btn.Height = seatSize;
                    btn.Text = seatCode;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.Cursor = Cursors.Hand;

                    int x = startX + (c * (seatSize + gap));
                    if (c >= cols / 2) x += aisleGap;
                    int y = startY + (r * (seatSize + gap));

                    btn.Location = new Point(x, y);

                    if (_bookedSeats.Contains(seatCode))
                    {
                        btn.BackColor = Color.Gray;
                        btn.Enabled = false;
                    }
                    else
                    {
                        bool isVIP = (r >= rows - 3);
                        btn.BackColor = isVIP ? Color.OrangeRed : Color.LightGray;
                        btn.ForeColor = isVIP ? Color.White : Color.Black;
                        btn.Tag = (isVIP ? "VIP|" : "NORMAL|") + seatCode;

                        btn.Click += (s, ev) =>
                        {
                            if (_selectedSeats.ContainsKey(seatCode))
                            {
                                _selectedSeats.Remove(seatCode);
                                btn.BackColor = isVIP ? Color.OrangeRed : Color.LightGray;
                            }
                            else
                            {
                                decimal price = _baseTicketPrice + (isVIP ? 10000 : 0);
                                _selectedSeats.Add(seatCode, price);
                                btn.BackColor = Color.DodgerBlue;
                            }
                            RefreshOrder();
                        };
                    }
                    pnlMapContainer.Controls.Add(btn);
                }
            }
        }

        private void RefreshOrder()
        {
            lstOrder.Items.Clear();
            decimal total = 0;
            foreach (var item in _selectedSeats)
            {
                total += item.Value;
                lstOrder.Items.Add($"Ghế {item.Key} - {item.Value:N0} đ");
            }
            lblTotal.Text = total.ToString("N0") + " đ";
        }

        private void DrawLegend()
        {
            pnlLegend.Controls.Clear();
            Action<int, Color, string> addItem = (x, col, txt) => {
                pnlLegend.Controls.Add(new Panel { Width = 20, Height = 20, BackColor = col, Left = x, Top = 15, BorderStyle = BorderStyle.FixedSingle });
                pnlLegend.Controls.Add(new Label { Text = txt, Left = x + 25, Top = 17, AutoSize = true });
            };
            addItem(20, Color.LightGray, "Ghế thường");
            addItem(150, Color.OrangeRed, "Ghế VIP (+10k)");
            addItem(320, Color.DodgerBlue, "Đang chọn");
            addItem(480, Color.Gray, "Đã đặt");
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
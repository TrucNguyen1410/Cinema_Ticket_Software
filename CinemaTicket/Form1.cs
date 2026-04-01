using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraScheduler;
using Microsoft.Data.SqlClient;
using System.Windows.Forms.DataVisualization.Charting;

namespace CinemaTicket
{
    public partial class Form1 : Form
    {
        private dynamic chartRevenue;
        private DataTable currentStatData;

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            InitScheduler();
            InitChartDynamic();

            // =========================================================
            // TỰ ĐỘNG TẠO TAB QUẢN LÝ TÀI KHOẢN
            // =========================================================
            TabControl mainTabControl = null;
            if (tabStats != null && tabStats.Parent is TabControl)
            {
                mainTabControl = (TabControl)tabStats.Parent;
            }
            else
            {
                foreach (Control ctrl in this.Controls)
                {
                    if (ctrl is TabControl) { mainTabControl = (TabControl)ctrl; break; }
                }
            }

            if (mainTabControl != null)
            {
                TabPage tabAccount = new TabPage("Tài khoản NV");
                tabAccount.BackColor = Color.White;

                Panel panAccTop = new Panel { Dock = DockStyle.Top, Height = 100, BackColor = Color.WhiteSmoke };

                Label lblUser = new Label { Text = "Tài khoản:", Location = new Point(20, 20), AutoSize = true };
                TextBox txtAccUser = new TextBox { Location = new Point(100, 18), Width = 150 };

                Label lblPass = new Label { Text = "Mật khẩu:", Location = new Point(270, 20), AutoSize = true };
                TextBox txtAccPass = new TextBox { Location = new Point(350, 18), Width = 150 };

                Label lblName = new Label { Text = "Họ Tên:", Location = new Point(20, 60), AutoSize = true };
                TextBox txtAccName = new TextBox { Location = new Point(100, 58), Width = 150 };

                Label lblRole = new Label { Text = "Chức vụ:", Location = new Point(270, 60), AutoSize = true };
                ComboBox cbAccRole = new ComboBox { Location = new Point(350, 58), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
                cbAccRole.Items.AddRange(new string[] { "Employee", "Admin" });
                cbAccRole.SelectedIndex = 0;

                Button btnAddAcc = new Button { Text = "Tạo tài khoản", Location = new Point(530, 18), Size = new Size(120, 30), BackColor = Color.LightGreen, Cursor = Cursors.Hand, Font = new Font("Arial", 9, FontStyle.Bold) };
                Button btnDelAcc = new Button { Text = "Xóa tài khoản", Location = new Point(530, 58), Size = new Size(120, 30), BackColor = Color.LightCoral, Cursor = Cursors.Hand, Font = new Font("Arial", 9, FontStyle.Bold) };

                panAccTop.Controls.AddRange(new Control[] { lblUser, txtAccUser, lblPass, txtAccPass, lblName, txtAccName, lblRole, cbAccRole, btnAddAcc, btnDelAcc });

                DataGridView dgvAcc = new DataGridView { Name = "dgvAccounts", Dock = DockStyle.Fill, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, AllowUserToAddRows = false, ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect, BackgroundColor = Color.White };

                tabAccount.Controls.Add(dgvAcc);
                tabAccount.Controls.Add(panAccTop);

                mainTabControl.TabPages.Add(tabAccount);
                mainTabControl.SelectedIndex = 0;

                Func<Task> LoadAccountsData = async () =>
                {
                    using (SqlConnection con = DB.GetConnection())
                    {
                        await con.OpenAsync();
                        using (SqlCommand cmd = new SqlCommand("SELECT AccountID, Username, FullName AS 'Họ tên', Role AS 'Chức vụ' FROM Account", con))
                        using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                        {
                            DataTable dt = new DataTable(); dt.Load(rd); dgvAcc.DataSource = dt;
                        }
                    }
                };

                btnAddAcc.Click += async (s, ev) =>
                {
                    if (string.IsNullOrWhiteSpace(txtAccUser.Text) || string.IsNullOrWhiteSpace(txtAccPass.Text) || string.IsNullOrWhiteSpace(txtAccName.Text))
                    { MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                    try
                    {
                        using (SqlConnection con = DB.GetConnection())
                        {
                            await con.OpenAsync();
                            SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Account WHERE Username=@u", con);
                            checkCmd.Parameters.AddWithValue("@u", txtAccUser.Text.Trim());
                            if ((int)await checkCmd.ExecuteScalarAsync() > 0)
                            { MessageBox.Show("Tên tài khoản này đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                            SqlCommand cmd = new SqlCommand("INSERT INTO Account (Username, Password, FullName, Role) VALUES (@u, @p, @fn, @r)", con);
                            cmd.Parameters.AddWithValue("@u", txtAccUser.Text.Trim());
                            cmd.Parameters.AddWithValue("@p", txtAccPass.Text.Trim());
                            cmd.Parameters.AddWithValue("@fn", txtAccName.Text.Trim());
                            cmd.Parameters.AddWithValue("@r", cbAccRole.SelectedItem.ToString());
                            await cmd.ExecuteNonQueryAsync();
                        }
                        MessageBox.Show("Tạo tài khoản thành công!", "Thông báo");
                        txtAccUser.Clear(); txtAccPass.Clear(); txtAccName.Clear();
                        await LoadAccountsData();
                    }
                    catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
                };

                btnDelAcc.Click += async (s, ev) =>
                {
                    if (dgvAcc.CurrentRow == null) { MessageBox.Show("Vui lòng chọn tài khoản cần xóa!"); return; }
                    int accId = Convert.ToInt32(dgvAcc.CurrentRow.Cells["AccountID"].Value);
                    string accUser = dgvAcc.CurrentRow.Cells["Username"].Value.ToString();

                    if (accUser.ToLower() == "admin") { MessageBox.Show("Không thể xóa tài khoản Admin hệ thống!"); return; }

                    if (MessageBox.Show($"Bạn có chắc muốn xóa tài khoản: {accUser}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            using (SqlConnection con = DB.GetConnection())
                            {
                                await con.OpenAsync();
                                SqlCommand cmd = new SqlCommand("DELETE FROM Account WHERE AccountID = @id", con);
                                cmd.Parameters.AddWithValue("@id", accId);
                                await cmd.ExecuteNonQueryAsync();
                            }
                            MessageBox.Show("Đã xóa tài khoản thành công!");
                            await LoadAccountsData();
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message.Contains("REFERENCE") || ex.Message.Contains("FK_"))
                                MessageBox.Show("Không thể xóa tài khoản này vì nhân viên đã bán vé.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            else MessageBox.Show("Lỗi: " + ex.Message);
                        }
                    }
                };
                await LoadAccountsData();
            }

            if (dgvBookings != null) dgvBookings.CellFormatting += dgvBookings_CellFormatting;

            if (cbStatType != null)
            {
                cbStatType.Items.Clear();
                cbStatType.Items.Add("Theo Ngày (Trong tháng)");
                cbStatType.Items.Add("Theo Tuần (Trong tháng)");
                cbStatType.Items.Add("Theo Tháng (Trong năm)");
                cbStatType.Items.Add("Theo Năm");
                cbStatType.Items.Add("Theo Phim (Top Doanh Thu)");
                cbStatType.SelectedIndex = 0;
            }

            try
            {
                await Task.WhenAll(
                    LoadMoviesAsync(),
                    LoadRoomsAsync(),
                    LoadShowsAsync(),
                    LoadMovieToFilterAsync(),
                    LoadBookingFiltersAsync()
                );
            }
            catch (Exception ex) { MessageBox.Show("Lỗi khởi động: " + ex.Message); }
        }

        // =========================================================
        // TẢI BỘ LỌC BOOKING
        // =========================================================
        private async Task LoadBookingFiltersAsync()
        {
            using (SqlConnection con = DB.GetConnection())
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("SELECT RoomID, RoomName FROM Rooms", con))
                using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                {
                    DataTable dt = new DataTable(); dt.Load(rd);
                    DataRow row = dt.NewRow(); row["RoomID"] = 0; row["RoomName"] = "--- Tất cả rạp ---"; dt.Rows.InsertAt(row, 0);
                    cbFilterRoomBooking.DataSource = dt; cbFilterRoomBooking.DisplayMember = "RoomName"; cbFilterRoomBooking.ValueMember = "RoomID";
                }
                using (SqlCommand cmd = new SqlCommand("SELECT MovieID, Title FROM Movies", con))
                using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                {
                    DataTable dt = new DataTable(); dt.Load(rd);
                    DataRow row = dt.NewRow(); row["MovieID"] = 0; row["Title"] = "--- Tất cả phim ---"; dt.Rows.InsertAt(row, 0);
                    cbFilterMovieBooking.DataSource = dt; cbFilterMovieBooking.DisplayMember = "Title"; cbFilterMovieBooking.ValueMember = "MovieID";
                }
            }
        }

        private void dgvBookings_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvBookings.Columns[e.ColumnIndex].Name == "Trạng thái" && e.Value != null)
            {
                string status = e.Value.ToString();
                if (status == "Đã Thanh Toán") { e.CellStyle.ForeColor = Color.Green; e.CellStyle.Font = new Font(dgvBookings.Font, FontStyle.Bold); }
                else if (status == "Đã Hoàn Vé") { e.CellStyle.ForeColor = Color.Red; e.CellStyle.Font = new Font(dgvBookings.Font, FontStyle.Bold); }
                else if (status == "Đã Đổi Vé") { e.CellStyle.ForeColor = Color.Blue; e.CellStyle.Font = new Font(dgvBookings.Font, FontStyle.Bold); }
            }
        }

        // =========================================================
        // CHỨC NĂNG LỌC VÉ
        // =========================================================
        private async void btnFilterBooking_Click(object sender, EventArgs e)
        {
            DateTime date = dtBookingFilter.Value.Date;
            using (SqlConnection con = DB.GetConnection())
            {
                await con.OpenAsync();
                string sql = @"SELECT b.BookingID, m.Title 'Phim', r.RoomName 'Rạp', b.SeatNumber 'Ghế', s.StartTime 'Suất', b.Price 'Giá', b.BookingTime 'Ngày Đặt', 
                               CASE b.Status WHEN 'PAID' THEN N'Đã Thanh Toán' WHEN 'REFUNDED' THEN N'Đã Hoàn Vé' WHEN 'EXCHANGED' THEN N'Đã Đổi Vé' ELSE b.Status END AS 'Trạng thái' 
                               FROM Bookings b JOIN ShowTimes s ON b.ShowID = s.ShowID JOIN Movies m ON s.MovieID = m.MovieID JOIN Rooms r ON s.RoomID = r.RoomID 
                               WHERE CAST(s.StartTime AS DATE) = @d ";

                if (cbFilterRoomBooking.SelectedValue != null && (int)cbFilterRoomBooking.SelectedValue > 0) sql += " AND r.RoomID = @roomId ";
                if (cbFilterMovieBooking.SelectedValue != null && (int)cbFilterMovieBooking.SelectedValue > 0) sql += " AND m.MovieID = @movieId ";
                if (!string.IsNullOrWhiteSpace(txtSearchBooking.Text)) sql += " AND (CAST(b.BookingID AS VARCHAR) LIKE @search OR b.SeatNumber LIKE @search OR m.Title LIKE @search) ";
                sql += " ORDER BY s.StartTime";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@d", date);
                    if (cbFilterRoomBooking.SelectedValue != null && (int)cbFilterRoomBooking.SelectedValue > 0) cmd.Parameters.AddWithValue("@roomId", cbFilterRoomBooking.SelectedValue);
                    if (cbFilterMovieBooking.SelectedValue != null && (int)cbFilterMovieBooking.SelectedValue > 0) cmd.Parameters.AddWithValue("@movieId", cbFilterMovieBooking.SelectedValue);
                    if (!string.IsNullOrWhiteSpace(txtSearchBooking.Text)) cmd.Parameters.AddWithValue("@search", "%" + txtSearchBooking.Text.Trim() + "%");

                    using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                    {
                        DataTable dt = new DataTable(); dt.Load(rd); dgvBookings.DataSource = dt;
                    }
                }
            }
        }

        // =========================================================
        // CHỨC NĂNG HOÀN VÉ
        // =========================================================
        private void BtnRefund_Click(object sender, EventArgs e)
        {
            if (dgvBookings.CurrentRow == null) { MessageBox.Show("Vui lòng chọn 1 vé để hoàn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (dgvBookings.CurrentRow.Cells["Trạng thái"].Value.ToString() == "Đã Hoàn Vé") { MessageBox.Show("Vé này đã được hoàn trước đó!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if ((Convert.ToDateTime(dgvBookings.CurrentRow.Cells["Suất"].Value) - DateTime.Now).TotalHours < 1) { MessageBox.Show("Chỉ hoàn vé trước ít nhất 1 tiếng!", "Quy định", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            int bookingId = Convert.ToInt32(dgvBookings.CurrentRow.Cells["BookingID"].Value);
            decimal price = Convert.ToDecimal(dgvBookings.CurrentRow.Cells["Giá"].Value);

            Form frmRefund = new Form { Text = "Thông tin hoàn vé", Size = new Size(400, 350), StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false };
            Label lblTien = new Label { Text = $"Số tiền hoàn: {price:N0} VNĐ", Location = new Point(20, 20), AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold), ForeColor = Color.Red };
            Label lblName = new Label { Text = "Họ tên KH:", Location = new Point(20, 60), AutoSize = true }; TextBox txtName = new TextBox { Location = new Point(120, 58), Width = 230 };
            Label lblPhone = new Label { Text = "SĐT:", Location = new Point(20, 90), AutoSize = true }; TextBox txtPhone = new TextBox { Location = new Point(120, 88), Width = 230 };
            Label lblEmail = new Label { Text = "Email:", Location = new Point(20, 120), AutoSize = true }; TextBox txtEmail = new TextBox { Location = new Point(120, 118), Width = 230 };
            Label lblMethod = new Label { Text = "Phương thức:", Location = new Point(20, 150), AutoSize = true };
            ComboBox cbMethod = new ComboBox { Location = new Point(120, 148), Width = 230, DropDownStyle = ComboBoxStyle.DropDownList }; cbMethod.Items.AddRange(new string[] { "Tiền mặt", "Chuyển khoản" }); cbMethod.SelectedIndex = 0;
            Label lblAccount = new Label { Text = "STK Nhận:", Location = new Point(20, 180), AutoSize = true }; TextBox txtAccount = new TextBox { Location = new Point(120, 178), Width = 230, Enabled = false };

            cbMethod.SelectedIndexChanged += (s, ev) => { txtAccount.Enabled = (cbMethod.Text == "Chuyển khoản"); };
            Button btnConfirm = new Button { Text = "Xác nhận Hoàn", Location = new Point(120, 230), Size = new Size(150, 40), BackColor = Color.Orange, Font = new Font("Arial", 9, FontStyle.Bold), Cursor = Cursors.Hand };

            btnConfirm.Click += (s, ev) =>
            {
                if (cbMethod.Text == "Chuyển khoản" && string.IsNullOrWhiteSpace(txtAccount.Text)) { MessageBox.Show("Nhập số tài khoản!", "Lỗi"); return; }
                try
                {
                    using (SqlConnection con = DB.GetConnection())
                    {
                        con.Open();
                        new SqlCommand($"UPDATE Bookings SET Status = 'REFUNDED' WHERE BookingID = {bookingId}", con).ExecuteNonQuery();
                    }
                    MessageBox.Show($"Đã hoàn {price:N0} VNĐ thành công!\nThông tin KH: {txtName.Text}", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frmRefund.Close(); btnFilterBooking.PerformClick();
                }
                catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
            };

            frmRefund.Controls.AddRange(new Control[] { lblTien, lblName, txtName, lblPhone, txtPhone, lblEmail, txtEmail, lblMethod, cbMethod, lblAccount, txtAccount, btnConfirm });
            frmRefund.ShowDialog();
        }

        // =========================================================
        // CHỨC NĂNG ĐỔI VÉ
        // =========================================================
        private void BtnExchange_Click(object sender, EventArgs e)
        {
            if (dgvBookings.CurrentRow == null) { MessageBox.Show("Vui lòng chọn 1 vé để đổi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            string status = dgvBookings.CurrentRow.Cells["Trạng thái"].Value.ToString();
            if (status == "Đã Hoàn Vé" || status == "Đã Đổi Vé") { MessageBox.Show("Vé đã hủy hoặc đã đổi!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if ((Convert.ToDateTime(dgvBookings.CurrentRow.Cells["Suất"].Value) - DateTime.Now).TotalHours < 1) { MessageBox.Show("Chỉ đổi vé trước ít nhất 1 tiếng!", "Quy định", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            int bookingId = Convert.ToInt32(dgvBookings.CurrentRow.Cells["BookingID"].Value);
            decimal oldPrice = Convert.ToDecimal(dgvBookings.CurrentRow.Cells["Giá"].Value);

            Form frmExchange = new Form { Text = "Đổi suất chiếu", Size = new Size(480, 300), StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false };
            Label lblOld = new Label { Text = $"Giá vé cũ: {oldPrice:N0} VNĐ", Location = new Point(20, 20), AutoSize = true, Font = new Font("Arial", 9, FontStyle.Bold) };
            Label lblShow = new Label { Text = "Chọn suất mới:", Location = new Point(20, 60), AutoSize = true };
            ComboBox cbNewShow = new ComboBox { Location = new Point(130, 58), Width = 310, DropDownStyle = ComboBoxStyle.DropDownList };
            Label lblSeat = new Label { Text = "Ghế mới:", Location = new Point(20, 100), AutoSize = true };
            TextBox txtNewSeat = new TextBox { Location = new Point(130, 98), Width = 80, ReadOnly = true };
            Button btnChooseSeat = new Button { Text = "Chọn ghế", Location = new Point(220, 96), Size = new Size(90, 26), BackColor = Color.LightYellow, Cursor = Cursors.Hand };
            Label lblDiff = new Label { Text = "Chênh lệch: 0 VNĐ", Location = new Point(20, 140), AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold), ForeColor = Color.Blue };
            Button btnConfirm = new Button { Text = "Xác nhận Đổi", Location = new Point(130, 190), Size = new Size(150, 40), BackColor = Color.LightSkyBlue, Font = new Font("Arial", 9, FontStyle.Bold), Cursor = Cursors.Hand };

            DataTable dtShows = new DataTable();
            using (SqlConnection con = DB.GetConnection())
            {
                con.Open();
                string sql = "SELECT s.ShowID, s.RoomID, r.TotalSeats, m.Title + ' - ' + r.RoomName + ' - ' + FORMAT(s.StartTime, 'dd/MM HH:mm') as ShowInfo, s.TicketPrice FROM ShowTimes s JOIN Movies m ON s.MovieID=m.MovieID JOIN Rooms r ON s.RoomID=r.RoomID WHERE s.StartTime > GETDATE()";
                using (SqlDataReader rd = new SqlCommand(sql, con).ExecuteReader()) dtShows.Load(rd);
            }
            cbNewShow.DataSource = dtShows; cbNewShow.DisplayMember = "ShowInfo"; cbNewShow.ValueMember = "ShowID";

            cbNewShow.SelectedIndexChanged += (s, ev) =>
            {
                txtNewSeat.Clear();
                if (cbNewShow.SelectedItem != null)
                {
                    decimal newPrice = Convert.ToDecimal(((DataRowView)cbNewShow.SelectedItem)["TicketPrice"]);
                    decimal diff = newPrice - oldPrice;
                    lblDiff.Text = diff > 0 ? $"Khách CẦN BÙ THÊM: {diff:N0} VNĐ" : (diff < 0 ? $"Hoàn lại cho khách: {Math.Abs(diff):N0} VNĐ" : "Đổi ngang giá (Chênh lệch: 0 VNĐ)");
                }
            };

            btnChooseSeat.Click += (s, ev) =>
            {
                if (cbNewShow.SelectedItem == null) return;
                DataRowView drv = (DataRowView)cbNewShow.SelectedItem;
                int showId = Convert.ToInt32(drv["ShowID"]), totalSeats = Convert.ToInt32(drv["TotalSeats"]);
                Form frmSeatMap = new Form { Text = "Sơ đồ ghế Rạp", Size = new Size(560, 450), StartPosition = FormStartPosition.CenterParent, AutoScroll = true, MaximizeBox = false };

                System.Collections.Generic.List<string> bookedSeats = new System.Collections.Generic.List<string>();
                using (SqlConnection con = DB.GetConnection())
                {
                    con.Open();
                    using (SqlDataReader rd = new SqlCommand($"SELECT SeatNumber FROM Bookings WHERE ShowID = {showId} AND Status IN ('PAID', 'EXCHANGED')", con).ExecuteReader())
                    { while (rd.Read()) bookedSeats.Add(rd["SeatNumber"].ToString().ToUpper()); }
                }

                int cols = 10;
                for (int i = 0; i < totalSeats; i++)
                {
                    string seatName = $"{(char)('A' + (i / cols))}{(i % cols) + 1}";
                    Button btnSeat = new Button { Text = seatName, Width = 45, Height = 40, Location = new Point(20 + (i % cols) * 50, 20 + (i / cols) * 50), Cursor = Cursors.Hand };
                    if (bookedSeats.Contains(seatName)) { btnSeat.BackColor = Color.LightGray; btnSeat.Enabled = false; }
                    else { btnSeat.BackColor = Color.LightGreen; btnSeat.Click += (ss, eev) => { txtNewSeat.Text = seatName; frmSeatMap.Close(); }; }
                    frmSeatMap.Controls.Add(btnSeat);
                }
                frmSeatMap.ShowDialog();
            };

            btnConfirm.Click += (s, ev) =>
            {
                if (string.IsNullOrWhiteSpace(txtNewSeat.Text)) { MessageBox.Show("Vui lòng chọn ghế mới!", "Lỗi"); return; }
                DataRowView drv = (DataRowView)cbNewShow.SelectedItem;
                if (MessageBox.Show(lblDiff.Text + $"\n\nTiếp tục đổi vé sang ghế {txtNewSeat.Text}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection con = DB.GetConnection())
                        {
                            con.Open();
                            SqlCommand cmd = new SqlCommand("UPDATE Bookings SET ShowID = @sid, SeatNumber = @seat, Price = @price, Status = 'EXCHANGED' WHERE BookingID = @id", con);
                            cmd.Parameters.AddWithValue("@sid", Convert.ToInt32(drv["ShowID"])); cmd.Parameters.AddWithValue("@seat", txtNewSeat.Text);
                            cmd.Parameters.AddWithValue("@price", Convert.ToDecimal(drv["TicketPrice"])); cmd.Parameters.AddWithValue("@id", bookingId);
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Đổi vé thành công!", "Hoàn tất"); frmExchange.Close(); btnFilterBooking.PerformClick();
                    }
                    catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
                }
            };
            frmExchange.Controls.AddRange(new Control[] { lblOld, lblShow, cbNewShow, lblSeat, txtNewSeat, btnChooseSeat, lblDiff, btnConfirm });
            if (dtShows.Rows.Count > 0) frmExchange.ShowDialog(); else MessageBox.Show("Hiện không có suất chiếu nào sắp tới để đổi!");
        }

        // =========================================================
        // THỐNG KÊ (BIỂU ĐỒ & EXPORT)
        // =========================================================
        private void InitChartDynamic()
        {
            try
            {
                if (chartRevenue != null) { tabStats.Controls.Remove(chartRevenue); chartRevenue = null; }
                chartRevenue = new Chart();
                var chartArea = new ChartArea("ChartArea1");
                chartArea.AxisX.Interval = 1; chartArea.AxisX.MajorGrid.Enabled = false;
                chartArea.AxisX.IsLabelAutoFit = true; chartArea.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.LabelsAngleStep45 | LabelAutoFitStyles.WordWrap;
                chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
                chartRevenue.ChartAreas.Add(chartArea);
                chartRevenue.Legends.Add(new Legend("Legend1") { Docking = Docking.Right, Alignment = StringAlignment.Center });
                chartRevenue.Palette = ChartColorPalette.BrightPastel; chartRevenue.Dock = DockStyle.Fill;
                tabStats.Controls.Add(chartRevenue); chartRevenue.BringToFront();
            }
            catch { tabStats.Controls.Add(new Label { Text = "Lỗi thư viện Chart.", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, ForeColor = Color.Red }); }
        }

        private async void btnStat_Click(object sender, EventArgs e)
        {
            if (chartRevenue == null) return;
            string type = cbStatType.SelectedItem.ToString(); DateTime selectedDate = dtStatDate.Value;
            chartRevenue.Series.Clear(); currentStatData = new DataTable();

            using (SqlConnection con = DB.GetConnection())
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("", con);
                string sql = "";

                if (type.Contains("Phim"))
                {
                    chartRevenue.ChartAreas[0].AxisX.LabelStyle.Enabled = true;
                    int selectedMovieID = cbFilterMovie.SelectedValue != null ? Convert.ToInt32(cbFilterMovie.SelectedValue) : 0;
                    sql = @"SELECT TOP 10 m.Title AS Label, COUNT(CASE WHEN b.Status IN ('PAID', 'EXCHANGED') THEN 1 END) AS VeBan, 
                            SUM(CASE WHEN b.Status IN ('PAID', 'EXCHANGED') THEN b.Price ELSE 0 END) AS DoanhThu, COUNT(CASE WHEN b.Status = 'REFUNDED' THEN 1 END) AS VeHoan, 
                            SUM(CASE WHEN b.Status = 'REFUNDED' THEN b.Price ELSE 0 END) AS TienHoan FROM Bookings b JOIN ShowTimes s ON b.ShowID = s.ShowID 
                            JOIN Movies m ON s.MovieID = m.MovieID {0} GROUP BY m.MovieID, m.Title ORDER BY DoanhThu DESC";
                    sql = string.Format(sql, selectedMovieID == 0 ? "" : "WHERE m.MovieID = @mid");
                    if (selectedMovieID != 0) cmd.Parameters.AddWithValue("@mid", selectedMovieID);
                }
                else
                {
                    chartRevenue.ChartAreas[0].AxisX.LabelStyle.Enabled = true; chartRevenue.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
                    string dateCond = "", groupBy = "";
                    if (type.Contains("Ngày")) { dateCond = "WHERE MONTH(BookingTime)=@m AND YEAR(BookingTime)=@y"; groupBy = "CAST(DAY(BookingTime) AS VARCHAR)"; cmd.Parameters.AddWithValue("@m", selectedDate.Month); cmd.Parameters.AddWithValue("@y", selectedDate.Year); }
                    else if (type.Contains("Tuần")) { dateCond = "WHERE MONTH(BookingTime)=@m AND YEAR(BookingTime)=@y"; groupBy = "CAST(DATEPART(week, BookingTime) AS VARCHAR)"; cmd.Parameters.AddWithValue("@m", selectedDate.Month); cmd.Parameters.AddWithValue("@y", selectedDate.Year); }
                    else if (type.Contains("Tháng")) { dateCond = "WHERE YEAR(BookingTime)=@y"; groupBy = "CAST(MONTH(BookingTime) AS VARCHAR)"; cmd.Parameters.AddWithValue("@y", selectedDate.Year); }
                    else if (type.Contains("Năm")) { groupBy = "CAST(YEAR(BookingTime) AS VARCHAR)"; }
                    sql = $@"SELECT {groupBy} AS Label, COUNT(CASE WHEN Status IN ('PAID', 'EXCHANGED') THEN 1 END) AS VeBan, SUM(CASE WHEN Status IN ('PAID', 'EXCHANGED') THEN Price ELSE 0 END) AS DoanhThu,
                             COUNT(CASE WHEN Status = 'REFUNDED' THEN 1 END) AS VeHoan, SUM(CASE WHEN Status = 'REFUNDED' THEN Price ELSE 0 END) AS TienHoan FROM Bookings {dateCond} GROUP BY {groupBy} ORDER BY CAST({groupBy} AS INT)";
                }

                cmd.CommandText = sql;
                using (SqlDataReader rd = await cmd.ExecuteReaderAsync()) currentStatData.Load(rd);

                if (currentStatData.Rows.Count == 0) { MessageBox.Show("Chưa có dữ liệu.", "Thông báo"); return; }

                Series sDoanhThu = new Series("Doanh Thu (VNĐ)") { ChartType = SeriesChartType.Column, IsValueShownAsLabel = true, LabelFormat = "N0", Color = Color.SteelBlue, IsXValueIndexed = true }; sDoanhThu.SmartLabelStyle.Enabled = true;
                Series sHoan = new Series("Tiền Hoàn (VNĐ)") { ChartType = SeriesChartType.Column, IsValueShownAsLabel = true, LabelFormat = "N0", Color = Color.Tomato, IsXValueIndexed = true }; sHoan.SmartLabelStyle.Enabled = true;
                chartRevenue.Series.Add(sDoanhThu); chartRevenue.Series.Add(sHoan);

                foreach (DataRow row in currentStatData.Rows)
                {
                    string labelStr = row["Label"].ToString();
                    if (type.Contains("Tuần")) labelStr = "Tuần " + labelStr; else if (type.Contains("Tháng")) labelStr = "Tháng " + labelStr;
                    sDoanhThu.Points.AddXY(labelStr, Convert.ToDouble(row["DoanhThu"])); sHoan.Points.AddXY(labelStr, Convert.ToDouble(row["TienHoan"]));
                }
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (currentStatData == null || currentStatData.Rows.Count == 0) { MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo"); return; }
            SaveFileDialog sfd = new SaveFileDialog { Filter = "CSV File (*.csv)|*.csv", FileName = $"BaoCao_{DateTime.Now:yyyyMMdd_HHmmss}.csv" };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StringBuilder sb = new StringBuilder(); sb.Append('\uFEFF');
                    sb.AppendLine("Mục Thống Kê,Số Vé Đã Bán,Doanh Thu (VNĐ),Số Vé Đã Hoàn,Tiền Hoàn (VNĐ)");
                    string type = cbStatType.SelectedItem.ToString();
                    foreach (DataRow row in currentStatData.Rows)
                    {
                        string label = row["Label"].ToString();
                        if (type.Contains("Tuần")) label = "Tuần " + label; else if (type.Contains("Tháng")) label = "Tháng " + label;
                        // FIX LỖI CÚ PHÁP ĐÃ GẶP TRƯỚC ĐÂY Ở ĐOẠN NÀY (CS1003)
                        label = label.Replace("\"", "\"\"");
                        sb.AppendLine($"\"{label}\",{row["VeBan"]},{row["DoanhThu"]},{row["VeHoan"]},{row["TienHoan"]}");
                    }
                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8); MessageBox.Show("Đã xuất báo cáo thành công!");
                }
                catch (Exception ex) { MessageBox.Show("Lỗi khi lưu file: " + ex.Message); }
            }
        }

        private void cbStatType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbStatType.SelectedItem == null) return;
            bool isMovie = cbStatType.SelectedItem.ToString().Contains("Theo Phim");
            lblFilterMovie.Visible = isMovie; cbFilterMovie.Visible = isMovie;
            label12.Visible = !isMovie; dtStatDate.Visible = !isMovie;
        }

        // =========================================================
        // CÁC HÀM CƠ BẢN QUẢN LÝ PHIM VÀ LỊCH CHIẾU
        // =========================================================
        private void InitScheduler()
        {
            if (dataStorage == null) dataStorage = new SchedulerDataStorage(this.components);
            schedulerControl1.DataStorage = dataStorage; schedulerControl1.ActiveViewType = SchedulerViewType.Day; schedulerControl1.Start = DateTime.Today;
            schedulerControl1.DayView.ShowAllDayArea = false; schedulerControl1.DayView.VisibleTime = new TimeOfDayInterval(TimeSpan.FromHours(8), TimeSpan.FromHours(27));
            schedulerControl1.DayView.TimeRulers.Clear(); schedulerControl1.DayView.TimeRulers.Add(new TimeRuler()); schedulerControl1.DayView.TimeScale = TimeSpan.FromMinutes(60);
        }

        private async Task LoadMovieToFilterAsync()
        {
            using (SqlConnection con = DB.GetConnection())
            {
                await con.OpenAsync();
                using (SqlDataReader rd = await new SqlCommand("SELECT MovieID, Title FROM Movies", con).ExecuteReaderAsync())
                {
                    DataTable dt = new DataTable(); dt.Load(rd);
                    DataRow row = dt.NewRow(); row["MovieID"] = 0; row["Title"] = "--- Tất cả phim ---"; dt.Rows.InsertAt(row, 0);
                    cbFilterMovie.DataSource = dt; cbFilterMovie.DisplayMember = "Title"; cbFilterMovie.ValueMember = "MovieID";
                }
            }
        }

        private async Task LoadMoviesAsync()
        {
            using (SqlConnection con = DB.GetConnection())
            {
                await con.OpenAsync();
                using (SqlDataReader rd = await new SqlCommand("SELECT MovieID, Title, Duration, Description FROM Movies", con).ExecuteReaderAsync())
                {
                    DataTable dt = new DataTable(); dt.Load(rd); dgvMovies.DataSource = dt;
                    cbShowMovie.DataSource = dt; cbShowMovie.DisplayMember = "Title"; cbShowMovie.ValueMember = "MovieID";
                }
            }
        }

        private async Task LoadRoomsAsync()
        {
            using (SqlConnection con = DB.GetConnection())
            {
                await con.OpenAsync();
                using (SqlDataReader rd = await new SqlCommand("SELECT RoomID, RoomName FROM Rooms", con).ExecuteReaderAsync())
                {
                    DataTable dt = new DataTable(); dt.Load(rd); cbRoom.DataSource = dt; cbRoom.DisplayMember = "RoomName"; cbRoom.ValueMember = "RoomID";
                }
            }
        }

        private async Task LoadShowsAsync()
        {
            schedulerControl1.BeginUpdate(); dataStorage.Appointments.Clear();
            try
            {
                using (SqlConnection con = DB.GetConnection())
                {
                    await con.OpenAsync();
                    using (SqlDataReader rd = await new SqlCommand("SELECT st.ShowID, st.StartTime, st.Duration, st.TicketPrice, m.Title, r.RoomName, r.TotalSeats FROM ShowTimes st JOIN Movies m ON st.MovieID = m.MovieID JOIN Rooms r ON st.RoomID = r.RoomID", con).ExecuteReaderAsync())
                    {
                        while (await rd.ReadAsync())
                        {
                            Appointment apt = dataStorage.CreateAppointment(AppointmentType.Normal);
                            apt.Start = (DateTime)rd["StartTime"]; apt.End = apt.Start.AddMinutes(Convert.ToInt32(rd["Duration"]));
                            apt.Subject = rd["Title"].ToString(); apt.Description = $"Rạp: {rd["RoomName"]}\nGhế: {rd["TotalSeats"]}\nGiá: {Convert.ToDecimal(rd["TicketPrice"]):N0} đ";
                            apt.CustomFields["ShowID"] = rd["ShowID"]; dataStorage.Appointments.Add(apt);
                        }
                    }
                }
            }
            catch { }
            finally { schedulerControl1.EndUpdate(); schedulerControl1.Refresh(); }
        }

        private async void btnAddShow_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtShowPrice.Text)) return;
            try
            {
                DateTime newStart = dtShowDate.Value.Date + dtShowTime.Value.TimeOfDay;
                int duration = Convert.ToInt32(((DataRowView)cbShowMovie.SelectedItem)["Duration"]);
                DateTime newEnd = newStart.AddMinutes(duration);
                int roomId = Convert.ToInt32(cbRoom.SelectedValue);

                using (SqlConnection con = DB.GetConnection())
                {
                    await con.OpenAsync();
                    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM ShowTimes WHERE RoomID = @r AND (@newStart < DATEADD(minute, Duration, StartTime)) AND (@newEnd > StartTime)", con);
                    checkCmd.Parameters.AddWithValue("@r", roomId); checkCmd.Parameters.AddWithValue("@newStart", newStart); checkCmd.Parameters.AddWithValue("@newEnd", newEnd);
                    if ((int)await checkCmd.ExecuteScalarAsync() > 0) { MessageBox.Show("Trùng lịch chiếu rạp này!"); return; }

                    SqlCommand cmd = new SqlCommand("INSERT INTO ShowTimes(MovieID, RoomID, StartTime, Duration, TicketPrice) VALUES(@m,@r,@t,@d,@p)", con);
                    cmd.Parameters.AddWithValue("@m", cbShowMovie.SelectedValue); cmd.Parameters.AddWithValue("@r", roomId);
                    cmd.Parameters.AddWithValue("@t", newStart); cmd.Parameters.AddWithValue("@d", duration); cmd.Parameters.AddWithValue("@p", decimal.Parse(txtShowPrice.Text));
                    await cmd.ExecuteNonQueryAsync();
                }
                MessageBox.Show("Thêm suất chiếu thành công!"); await LoadShowsAsync();
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (schedulerControl1.SelectedAppointments.Count == 0) return;
            try
            {
                Appointment apt = schedulerControl1.SelectedAppointments[0];
                int id = Convert.ToInt32(apt.CustomFields["ShowID"]);
                if (MessageBox.Show("Xác nhận xóa suất chiếu này?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    using (SqlConnection con = DB.GetConnection())
                    {
                        await con.OpenAsync(); new SqlCommand($"DELETE FROM ShowTimes WHERE ShowID={id}", con).ExecuteNonQuery();
                    }
                    dataStorage.Appointments.Remove(apt);
                }
            }
            catch { }
        }

        private void dgvMovies_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtId.Text = dgvMovies.Rows[e.RowIndex].Cells["MovieID"].Value.ToString();
                txtTitle.Text = dgvMovies.Rows[e.RowIndex].Cells["Title"].Value.ToString();
                txtDuration.Text = dgvMovies.Rows[e.RowIndex].Cells["Duration"].Value.ToString();
                if (dgvMovies.Columns.Contains("Description")) txtDescription.Text = dgvMovies.Rows[e.RowIndex].Cells["Description"].Value?.ToString();
            }
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            try { using (SqlConnection con = DB.GetConnection()) { await con.OpenAsync(); SqlCommand cmd = new SqlCommand("INSERT INTO Movies(Title,Duration,Description) VALUES(@t,@d,@ds)", con); cmd.Parameters.AddWithValue("@t", txtTitle.Text); cmd.Parameters.AddWithValue("@d", int.Parse(txtDuration.Text)); cmd.Parameters.AddWithValue("@ds", txtDescription.Text); await cmd.ExecuteNonQueryAsync(); } await LoadMoviesAsync(); } catch { }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            try { using (SqlConnection con = DB.GetConnection()) { await con.OpenAsync(); SqlCommand cmd = new SqlCommand("UPDATE Movies SET Title=@t,Duration=@d,Description=@ds WHERE MovieID=@id", con); cmd.Parameters.AddWithValue("@id", txtId.Text); cmd.Parameters.AddWithValue("@t", txtTitle.Text); cmd.Parameters.AddWithValue("@d", txtDuration.Text); cmd.Parameters.AddWithValue("@ds", txtDescription.Text); await cmd.ExecuteNonQueryAsync(); } await LoadMoviesAsync(); } catch { }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text)) return;
            if (MessageBox.Show($"Xóa phim: {txtTitle.Text} và các dữ liệu liên quan?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;

            try
            {
                using (SqlConnection con = DB.GetConnection())
                {
                    await con.OpenAsync(); SqlTransaction tran = con.BeginTransaction();
                    try
                    {
                        int id = int.Parse(txtId.Text);
                        new SqlCommand($"DELETE FROM Bookings WHERE ShowID IN (SELECT ShowID FROM ShowTimes WHERE MovieID = {id})", con, tran).ExecuteNonQuery();
                        new SqlCommand($"DELETE FROM ShowTimes WHERE MovieID = {id}", con, tran).ExecuteNonQuery();
                        new SqlCommand($"DELETE FROM Movies WHERE MovieID = {id}", con, tran).ExecuteNonQuery();
                        tran.Commit(); txtId.Clear(); txtTitle.Clear(); txtDuration.Clear(); txtDescription.Clear();
                        await LoadMoviesAsync(); await LoadShowsAsync(); MessageBox.Show("Đã xóa phim!");
                    }
                    catch { tran.Rollback(); }
                }
            }
            catch { }
        }

        private void panelShowTop_Paint(object sender, PaintEventArgs e) { }

        private void cbFilterRoomBooking_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lblFRoom_Click(object sender, EventArgs e)
        {

        }
    }
}
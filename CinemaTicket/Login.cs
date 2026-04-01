using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace CinemaTicket
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();

            // Gắn sự kiện ấn phím Enter ở ô Mật khẩu thì sẽ tự đăng nhập
            this.txtPassword.KeyDown += new KeyEventHandler(this.txtPassword_KeyDown);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection con = DB.GetConnection())
                {
                    con.Open();
                    // Truy vấn kiểm tra tài khoản
                    string sql = "SELECT * FROM Account WHERE Username = @user AND Password = @pass";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@user", username);
                        cmd.Parameters.AddWithValue("@pass", password);

                        using (SqlDataReader rd = cmd.ExecuteReader())
                        {
                            if (rd.Read())
                            {
                                // 1. Lưu thông tin người dùng vào Session toàn cục
                                Session.AccountID = Convert.ToInt32(rd["AccountID"]);
                                Session.Username = rd["Username"].ToString();
                                Session.FullName = rd["FullName"].ToString();
                                Session.Role = rd["Role"].ToString();

                                MessageBox.Show($"Đăng nhập thành công!\nXin chào: {Session.FullName} ({Session.Role})", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // 2. Ẩn Form Đăng Nhập đi
                                this.Hide();

                                // 3. Kiểm tra Quyền (Role) để mở Form tương ứng
                                if (Session.Role == "Admin")
                                {
                                    Form1 frmAdmin = new Form1();
                                    // Khi tắt Form1, sẽ hiển thị lại Form Login (như kiểu Đăng xuất)
                                    frmAdmin.FormClosed += (s, args) => this.Show();
                                    frmAdmin.Show();
                                }
                                else if (Session.Role == "Employee")
                                {
                                    // Mở Form của nhân viên (Form2)
                                    Form2 frmEmployee = new Form2();
                                    frmEmployee.FormClosed += (s, args) => this.Show();
                                    frmEmployee.Show();
                                }

                                // Xóa mật khẩu để bảo mật nếu người dùng đăng xuất ra
                                txtPassword.Clear();
                            }
                            else
                            {
                                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message, "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Hàm hỗ trợ: Ấn Enter ở ô Password sẽ kích hoạt nút Đăng nhập
        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(sender, e);
            }
        }
    }
}